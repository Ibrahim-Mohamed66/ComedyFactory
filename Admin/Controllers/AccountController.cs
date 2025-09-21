using Admin.ViewModels;
using Application.DTOS;
using Application.IServices;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Admin.Controllers;

public class AccountController : Controller
{

    private readonly IUserService _userService;
    private readonly IMapper _mapper;
    private readonly ILogger<AccountController> _logger;
    public AccountController(IUserService userService, IMapper mapper, ILogger<AccountController> logger)
    {
        _userService = userService;
        _mapper = mapper;
        _logger = logger;
    }
    [HttpGet]
    public IActionResult Login()
    {

        return View();

    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> LoginAsync(LoginVm model)
    {
        try
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            // Map LoginVm to LoginDto using AutoMapper
            var loginDto = _mapper.Map<LoginDto>(model);
            var result = await _userService.LoginAsync(loginDto);
            if (result.Success)
            {
                SetAuthTokens(result);
                _logger.LogInformation("User {Email} logged in successfully.", model.Email);
                TempData["Success"] = "Login Successful";

                return RedirectToAction("Index", "Home");
            }
            else
            {
                foreach (var error in result.Errors)
                {
                    _logger.LogWarning("Login failed for user {Email}: {Error}", model.Email, error);
                    ModelState.AddModelError(string.Empty, error ?? "An error occurred during login.");

                }
                return View(model);
            }

        }
        catch (Exception ex)
        {

            _logger.LogError(ex, "Login failed for user {Email}", model.Email);
            TempData["ErrorMessage"] = "Something went wrong while processing your request. Please try again later.";
            return RedirectToAction("Login");
        }
    }

    [HttpPost]
    [AllowAnonymous]
    public async Task<IActionResult> Refresh()
    {
        try
        {
            var accessToken = HttpContext.Session.GetString("AccessToken");
            var refreshToken = Request.Cookies["RefreshToken"];
            if (string.IsNullOrEmpty(accessToken) || string.IsNullOrEmpty(refreshToken))
                return Unauthorized(new { message = "Login required" });

            // 🔑 ignore access token validity, just validate refresh
            var result = await _userService.GetRefreshToken(new RefreshTokenRequestDto
            {
                AccessToken = accessToken,
                RefreshToken = refreshToken
            });

            if (result == null || string.IsNullOrEmpty(result.Token))
                return Unauthorized(new { message = "Login required" });

            SetAuthTokens(result);

            return Ok(new { result.Token, result.RefreshToken });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error during token refresh.");
            return StatusCode(500, new { message = "Failed to refresh token" });
        }
    }


    [HttpGet]
    public IActionResult Register()
    {
        return View();
    }
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> RegisterAsync(RegisterVm model)
    {
        try
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            var registerDto = _mapper.Map<RegisterDto>(model);
            var result = await _userService.RegisterAsync(registerDto);
            if (result.Success)
            {
                SetAuthTokens(result);
                _logger.LogInformation("User {Email} registered successfully.", model.Email);
                TempData["Success"] = "Register Successful";


                return RedirectToAction("Index", "Home");
            }
            else
            {
                foreach (var error in result.Errors)
                {
                    _logger.LogWarning("Registration failed for user {Email}: {Error}", model.Email, error);
                    ModelState.AddModelError(string.Empty, error ?? "An error occurred during registration.");
                }
                return RedirectToAction("Register");

            }

        }
        catch (Exception ex)
        {

            _logger.LogError(ex, "Register failed for user {Email}", model.Email);
            TempData["ErrorMessage"] = "Something went wrong while processing your request. Please try again later.";
            return RedirectToAction("Register");
        }

    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    [Authorize]
    public IActionResult Logout()
    {
        Response.Cookies.Delete("RefreshToken");
        HttpContext.Session.Remove("AccessToken");
        _logger.LogInformation("User logged out successfully.");
        return RedirectToAction("Login");
    }
    private void SetAuthTokens(AuthResponseDto result)
    {
        if (result.Token == null || result.RefreshToken == null)
        {
            _logger.LogError("Invalid AuthResponseDto: Token or RefreshToken is null.");
            throw new InvalidOperationException("Cannot set cookies with null tokens.");
        }
        HttpContext.Session.SetString("AccessToken", result.Token);
        HttpContext.Session.SetString("AccessTokenExpiration", result.AccessTokenExpiration.ToString());

        var refreshOptions = new CookieOptions
        {
            HttpOnly = true,
            Secure = true,
            SameSite = SameSiteMode.Strict,
            Expires = result.RefreshTokenExpiration, 
            IsEssential = true
        };
        Response.Cookies.Append("RefreshToken", result.RefreshToken!, refreshOptions);
        _logger.LogInformation("Authentication cookies set successfully.");
    }
}

