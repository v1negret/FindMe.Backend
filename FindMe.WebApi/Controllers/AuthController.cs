using FindMe.Models.Identity.Dto;
using FindMe.Services.Interfaces;
using FindMe.WebApi.Filters.Attributes;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace FindMe.WebApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly IAuthentificationService _authService;

    public AuthController(IAuthentificationService authService)
    {
        _authService = authService;
    }
    
    [HttpPost("register")]
    [ValidateModel]
    public async Task<IActionResult> Register(RegisterDto request)
    {
        var response = await _authService.Register(request);
        return Ok(response);
    }
    
    [HttpPost("login")]
    [ValidateModel]
    public async Task<IActionResult> Login(LoginDto request)
    {
        var response = await _authService.Login(request);
        return Ok(response);
    }
}