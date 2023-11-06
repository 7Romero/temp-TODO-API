using Microsoft.AspNetCore.Authentication.OAuth;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using ToDo.API.Dto.User;
using ToDo.API.Infrastructure.Configuration;
using ToDo.Domain.Entity.Auth;

namespace ToDo.API.Controllers;

[Route("api/account")]
public class AuthController : AppBaseController
{
    private readonly UserManager<User> _userManager;
    private readonly SignInManager<User> _signInManager;
    private readonly AuthOptions _authenticationOptions;

    public AuthController(UserManager<User> userManager, SignInManager<User> signInManager, IOptions<AuthOptions> authenticationOptions)
    {
        _userManager = userManager;
        _signInManager = signInManager;
        _authenticationOptions = authenticationOptions.Value;
    }

    [HttpGet("getMe")]
    public async Task<IActionResult> GetMe()
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

        var user = await _userManager.FindByIdAsync(userId);
        var userRoles = await _userManager.GetRolesAsync(user);

        var userGetMe = new UserGetMeDto()
        {
            Id = user.Id,
            Username = user.UserName,
            Roles = userRoles,
        };

        return Ok(userGetMe);
    }

    [AllowAnonymous]
    [HttpPost("register")]
    public async Task<IActionResult> Register(UserRegisterDto userRegisterDto)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(new { errorText = "Invalid username or password." });
        }

        var checkUser = await _userManager.FindByEmailAsync(userRegisterDto.Email);

        if (checkUser != null)
        {
            return BadRequest(new { errorText = "User with this username already exists" });
        }

        var user = new User
        {
            UserName = userRegisterDto.Email,
            Email = userRegisterDto.Email,
        };

        var result = await _userManager.CreateAsync(user, userRegisterDto.Password);
        if (result.Succeeded)
        {
            await _signInManager.SignInAsync(user, false);
        }
        else
        {
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError(string.Empty, error.Description);
            }
        }

        return Ok(userRegisterDto);
    }

    [AllowAnonymous]
    [HttpPost("login")]
    public async Task<IActionResult> Login(UserAuthDto userAuthDto)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(new { errorText = "Invalid username or password." });
        }

        var identity = await GetIdentity(userAuthDto);

        if (identity == null)
        {
            return BadRequest(new { errorText = "Invalid username or password." });
        }

        var jwt = new JwtSecurityToken(
                            issuer: _authenticationOptions.Issuer,
                            audience: _authenticationOptions.Audience,
                            notBefore: DateTime.Now,
                            claims: identity.Claims,
                            expires: DateTime.Now.AddHours(_authenticationOptions.TokenLifetime),
                            signingCredentials: new SigningCredentials(_authenticationOptions.GetSymmetricSecurityKey(), SecurityAlgorithms.HmacSha256));
        var encodedJwt = new JwtSecurityTokenHandler().WriteToken(jwt);

        var response = new
        {
            token = encodedJwt,
        };

        return Json(response);
    }

    private async Task<ClaimsIdentity?> GetIdentity(UserAuthDto userAuthDto)
    {
        var result =
                await _signInManager.PasswordSignInAsync(userAuthDto.Username, userAuthDto.Password, false, false);

        if (!result.Succeeded)
        {
            return null;
        }

        var user = await _userManager.FindByNameAsync(userAuthDto.Username);
        var userRoles = await _userManager.GetRolesAsync(user);

        var claims = new List<Claim>()
        {
            new Claim(ClaimTypes.NameIdentifier, user.Id.ToString())
        };

        foreach (var role in userRoles)
        {
            claims.Add(new Claim(ClaimTypes.Role, role));
        }

        ClaimsIdentity claimsIdentity = new(claims, _authenticationOptions.SecretKey, ClaimTypes.NameIdentifier, ClaimTypes.Role);

        return claimsIdentity;
    }
}
