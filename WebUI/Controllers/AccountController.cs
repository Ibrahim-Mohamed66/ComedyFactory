using Application.DTOS;
using Application.IServices;
using AutoMapper;
using Domain.Enums;
using IOC.Resources;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.Extensions.Localization;
using System;
using WebUI.Mapper;
using WebUI.Models;
using WebUI.ViewModels;

namespace WebUI.Controllers
{
    
    public class AccountController : Controller
    {
        private readonly IUserService _userService;
        private readonly IMapper _mapper;
        private readonly ILogger<AccountController> _logger;
        private readonly IStringLocalizer<SharedResource> _localizer;
        public AccountController(IUserService userService, IMapper mapper, ILogger<AccountController> logger, IStringLocalizer<SharedResource> localizer)
        {
            _userService = userService;
            _mapper = mapper;
            _logger = logger;
            _localizer = localizer;
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
                return View(model);
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
            var model = new RegisterVm()
            {
                GenderList = GetGenderList().ToList()
            };
            return View(model);
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
                var isEmailExist = await _userService.IsEmailExsitstAsync(model.Email);
                if (isEmailExist)
                {
                    _logger.LogError("Email Already Exists {model.email}",model.Email);
                    ModelState.AddModelError(string.Empty, "Email already exists. Please use a different email.");
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
                        return View(model);

                }

            }
            catch (Exception ex)
            {

                _logger.LogError(ex, "Register failed for user {Email}", model.Email);
                TempData["ErrorMessage"] = "Something went wrong while processing your request. Please try again later.";
                return View(model);
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
            return RedirectToAction("Index", "Home");
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
        private  IEnumerable<SelectListItem> GetGenderList()
        {
            var genders = Enum.GetValues(typeof(Gender))
                .Cast<Gender>()
                .Select(g => new SelectListItem
                {
                    Value = g.ToString(),
                    Text = g.Localize(_localizer)
                })
                .ToList();
            return genders;
        }
    }
    
}
