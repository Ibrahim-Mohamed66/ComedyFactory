using Application.DTOS;
using Application.IServices;
using AutoMapper;
using Data.Repositories.IRepositories;
using Domain.Models;
using Domain.StaticData;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;


namespace Application.Services;

public class UserService : IUserService
{
    private readonly IUserReadRepository _userReadRepository;
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly JWT _jwtSettings;
    private readonly IMapper _mapper;
    private readonly IRefreshTokenRepository _refreshTokenRepository;
    public UserService(IUserReadRepository userReadRepository,
                        UserManager<ApplicationUser> userManager,
                        IOptions<JWT> jwtSettings,
                        IRefreshTokenRepository refreshTokenRepository,
                        IMapper mapper)
    {
        _userReadRepository = userReadRepository;
        _userManager = userManager;
        _jwtSettings = jwtSettings.Value;
        _refreshTokenRepository = refreshTokenRepository;
        _mapper = mapper;
    }

    public TokenValidationParameters GetValidationParameters(bool validateLifetime = true)
    {
        return new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateIssuerSigningKey = true,
            ValidateLifetime = validateLifetime, // false when reading expired, true for normal validation
            ValidIssuer = _jwtSettings.Issuer,
            ValidAudience = _jwtSettings.Audience,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtSettings.SecretKey))
        };
    }

    public async Task<AuthResponseDto> GetRefreshToken(RefreshTokenRequestDto requestDto)
    {
        var principal = GetPrincipalFromExpiredToken(requestDto.AccessToken);
        if (principal == null)
        {
            return new AuthResponseDto
            {
                Success = false,
                Errors = new[] { "Invalid access token or refresh token." }
            };
            
        }
        var emailClaim = principal.FindFirst(ClaimTypes.Email);
        if (emailClaim == null)
        {
            return new AuthResponseDto
            {
                Success = false,
                Errors = new[] { "Invalid access token or refresh token." }
            };
        }
        var email = emailClaim.Value;
        var userExists = await _userManager.FindByEmailAsync(email);
        if (userExists == null)
        {
            return new AuthResponseDto
            {
                Success = false,
                Errors = new[] { "Invalid access token or refresh token." }
            };
        }
        var existingRefreshToken = await _refreshTokenRepository.GetRefreshTokenByUserIdAsync(userExists.Id);
        if(existingRefreshToken == null || existingRefreshToken.Token != requestDto.RefreshToken )
        {
            return new AuthResponseDto
            {
                Success = false,
                Errors = new[] { "Invalid access token or refresh token." }
            };
        }
        await _refreshTokenRepository.RevokeRefreshTokenAsync(existingRefreshToken);
        var accessToken = await GenerateJwtToken(userExists, (await _userManager.GetRolesAsync(userExists)).ToArray());
        var newRefreshToken = GenerateRefreshToken();
        newRefreshToken.UserId = userExists.Id;
        await _refreshTokenRepository.AddRefreshTokenAsync(newRefreshToken);
        return new AuthResponseDto
        {
            Success = true,
            Token = accessToken.Token,
            RefreshToken = newRefreshToken.Token,
            AccessTokenExpiration = accessToken.ExpiresAt,
            RefreshTokenExpiration = newRefreshToken.ExpiresAt
        };

    }

    public bool IsTokenExpired(string token)
    {
        try
        {
            var jwtToken = new JwtSecurityTokenHandler().ReadJwtToken(token);
            return jwtToken.ValidTo < DateTime.UtcNow;
        }
        catch
        {
            // Invalid token → treat as expired
            return true;
        }
    }


    public async Task<AuthResponseDto> LoginAsync(LoginDto loginDto)
    {
        var userExists = await _userManager.FindByEmailAsync(loginDto.Email);
        if (userExists == null || !await _userManager.CheckPasswordAsync(userExists, loginDto.Password))
        {
            return new AuthResponseDto
            {
                Success = false,
                Errors = new[] { "Wrong Password Or Email. Please Try Again" }
            };
        }
        var accessToken = await GenerateJwtToken(userExists, (await _userManager.GetRolesAsync(userExists)).ToArray());

        var hasValidRefreshToken = await _refreshTokenRepository.HasValidRefreshTokenAsync(userExists.Id);
        if (hasValidRefreshToken)
        {
            var tokenTobeRevoked = await _refreshTokenRepository.GetRefreshTokenByUserIdAsync(userExists.Id);
            await _refreshTokenRepository.RevokeRefreshTokenAsync(tokenTobeRevoked);
        }
        var refreshToken = GenerateRefreshToken();
        refreshToken.UserId = userExists.Id;
        await _refreshTokenRepository.AddRefreshTokenAsync(refreshToken);

        return new AuthResponseDto
        {
            Success = true,
            Token = accessToken.Token,
            RefreshToken = refreshToken.Token,
            AccessTokenExpiration = accessToken.ExpiresAt,
            RefreshTokenExpiration = refreshToken.ExpiresAt
        };

    }

    public async Task<AuthResponseDto> RegisterAsync(RegisterDto registerDto)
    {
        if (await _userManager.FindByEmailAsync(registerDto.Email) is not null)
        {
            return new AuthResponseDto
            {
                Success = false,
                Errors = new[] { "Email is already registered!" }
            };

        }

        if (registerDto == null)
        {
            return new AuthResponseDto
            {
                Success = false,
                Errors = new[] { "Invalid user data." }
            };
        }

        var user = _mapper.Map<ApplicationUser>(registerDto);
        var result = await _userManager.CreateAsync(user, registerDto.Password);
        if (!result.Succeeded)
        {
            var errors = result.Errors.Select(e => e.Description);
            return new AuthResponseDto
            {
                Success = false,
                Errors = new[] { $"User creation failed: {string.Join(", ", errors)}" }
            };
        }
        if (!string.IsNullOrEmpty(registerDto.Role))
        {
            await _userManager.AddToRoleAsync(user,registerDto.Role);
        }
        else
        {
            await _userManager.AddToRoleAsync(user, RoleNames.User);
        }
        var roles = await _userManager.GetRolesAsync(user);
        var accessToken = await GenerateJwtToken(user, roles.ToArray());
        var refreshToken = GenerateRefreshToken();
        refreshToken.UserId = user.Id;
        await _refreshTokenRepository.AddRefreshTokenAsync(refreshToken);
        return new AuthResponseDto
        {
            Success = true,
            Token = accessToken.Token,
            RefreshToken = refreshToken.Token,
            AccessTokenExpiration = accessToken.ExpiresAt,
            RefreshTokenExpiration = refreshToken.ExpiresAt
        };

    }
    #region JwtHelpers
    private async Task<AccessTokenDto> GenerateJwtToken(ApplicationUser user, params string[] roles)
    {


        var authClaims = new List<Claim>
        {
            new Claim(ClaimTypes.NameIdentifier, user.Id),
            new Claim(ClaimTypes.Email, user.Email),
            new Claim("FullName", user.FullName ?? string.Empty),
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
        };

        foreach (var role in roles)
            authClaims.Add(new Claim(ClaimTypes.Role, role));

        var authSecurityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtSettings.SecretKey));
        var credentials = new SigningCredentials(authSecurityKey, SecurityAlgorithms.HmacSha256);

        var token = new JwtSecurityToken(
            issuer: _jwtSettings.Issuer,
            audience: _jwtSettings.Audience,
            expires: DateTime.UtcNow.AddMinutes((_jwtSettings.AccessTokenExpirationMinutes)),
            claims: authClaims,
            signingCredentials: credentials
            );

        return new AccessTokenDto
        {
            Token = new JwtSecurityTokenHandler().WriteToken(token),
            ExpiresAt = DateTime.UtcNow.AddMinutes(_jwtSettings.AccessTokenExpirationMinutes)
        };
    }


    private RefreshToken GenerateRefreshToken()
    {
        var randomBytes = new byte[64];
        using var rng = RandomNumberGenerator.Create();
        rng.GetBytes(randomBytes);

        return new RefreshToken
        {
            Token = Convert.ToBase64String(randomBytes),
            ExpiresAt = DateTime.UtcNow.AddDays(_jwtSettings.RefreshTokenExpirationDays),
            CreatedAt = DateTime.UtcNow
        };
    }

    private ClaimsPrincipal? GetPrincipalFromExpiredToken(string token)
    {

        var tokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateIssuerSigningKey = true,
            ValidateLifetime = false,
            ValidIssuer = _jwtSettings.Issuer,
            ValidAudience = _jwtSettings.Audience,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtSettings.SecretKey))
        };

        var tokenHandler = new JwtSecurityTokenHandler();
        try
        {
            var principal = tokenHandler.ValidateToken(token, tokenValidationParameters, out SecurityToken securityToken);

            if (securityToken is not JwtSecurityToken jwtSecurityToken ||
                !jwtSecurityToken.Header.Alg.Equals(SecurityAlgorithms.HmacSha256, StringComparison.InvariantCultureIgnoreCase))
            {
                return null;
            }


            return principal;
        }
        catch
        {
            return null;
        }
    }

    public async Task<bool> IsEmailExsitstAsync(string email)
    {
        return await _userReadRepository.IsEmailExistAsync(email);
    }




    #endregion
}

