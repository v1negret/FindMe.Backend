using FindMe.Models.Dto.Simp;
using FindMe.Services.Interfaces;
using FindMe.WebApi.Filters.Attributes;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace FindMe.WebApi.Controllers;

[ApiController]
[Authorize]
[Route("api/[controller]")]
public class SimpController : ControllerBase
{
    private readonly ISimpService _simp;
    private readonly UserManager<IdentityUser> _userManager;
    public SimpController(ISimpService simp, UserManager<IdentityUser> userManager)
    {
        _simp = simp;
        _userManager = userManager;
    }

    [HttpPost("add/")]
    [ValidateModel]
    public async Task<IActionResult> Add([FromBody]AddSimpDto request)
    {
        if (request is null)
            return BadRequest();
        var currentUser = await _userManager.GetUserAsync(User);
        
        request.FromUserId = currentUser.Id;
        
        var result = await _simp.Add(request);
        if (!result.Succeeded)
            return BadRequest(result.Message);
        
        return Ok();
    }

    [HttpDelete("delete/")]
    [ValidateModel]
    public async Task<IActionResult> Delete([FromBody] DeleteSimpDto request)
    {
        if (request is null)
            return BadRequest();
        
        var currentUser = await _userManager.FindByEmailAsync(HttpContext.User.Identity.Name);
        if (currentUser.Id != request.ToUserId && currentUser.Id != request.FromUserId)
            return Forbid();
        
        var result = await _simp.Delete(request);
        if (!result.Succeeded)
            return BadRequest(result.Message);

        return Ok();
    }

    [HttpGet("get/in/")]
    public async Task<IActionResult> GetIncoming()
    {
        var currentUser = await _userManager.FindByEmailAsync(HttpContext.User.Identity.Name);
        var result = await _simp.GetIncoming(currentUser.Id);

        if (result is null)
            return BadRequest();

        return Ok(result);
    }
    
    [HttpGet("get/out/")]
    public async Task<IActionResult> GetOutgoing()
    {
        var currentUser = await _userManager.FindByEmailAsync(HttpContext.User.Identity.Name);
        var result = await _simp.GetOutgoing(currentUser.Id);
        
        if (result is null)
            return BadRequest();
        
        return Ok(result);
    }
}