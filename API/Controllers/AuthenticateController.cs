using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using API.Authentication;
using Core.Enities.ProjectAggregate;
using Core.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;




namespace API.Controllers;
[ApiController]
[Route("api/[controller]")]
public class AuthenticateController : ControllerBase
{
    private readonly IAuthService _authService;
 
    public AuthenticateController(IAuthService authService)
    {
        _authService = authService;
    } 
    [HttpPost("Login")]
    public async Task<IActionResult> Login([FromBody] LoginModel model)
    {
        try
        {
            var loginResponse = await _authService.LoginAsync(model);
            return Ok(loginResponse);
        }
        catch (UnauthorizedAccessException)
        {
            return Unauthorized();
        }
    }




    [HttpPost("RegisterTranslator")]
    public async Task<IActionResult> Register([FromBody] RegisterModel model)
    {
        var result = await _authService.RegisterTranslatorAsync(model);
        return result ? Ok() : StatusCode(StatusCodes.Status500InternalServerError);
    }




    [HttpPost("RegisterManager")]
    public async Task<IActionResult> RegisterAdmin([FromBody] RegisterModel model)
    {
        var result = await _authService.RegisterManagerAsync(model);
        return result ? Ok() : StatusCode(StatusCodes.Status500InternalServerError);
    }




    [HttpGet("FetchUsers")]
    public async Task<IActionResult> FetchUsers()
    {
        var users = await _authService.FetchUsersAsync();
        return Ok(users);
    }
}