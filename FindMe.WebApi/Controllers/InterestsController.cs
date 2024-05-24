using FindMe.Models.Dto.Interest;
using FindMe.Models.Results;
using FindMe.Services.Interfaces;
using FindMe.WebApi.Filters.Attributes;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace FindMe.WebApi.Controllers;

[ApiController]
[Authorize(Roles = "User, Admin")]
[Route("api/[controller]")]
public class InterestsController : ControllerBase
{
    private readonly IInterestsService _interestsService;
    private readonly IFormService _formService;
    private readonly UserManager<IdentityUser> _userManager;
    public InterestsController(IInterestsService interestsService, UserManager<IdentityUser> userManager, IFormService formService)
    {
        _interestsService = interestsService;
        _userManager = userManager;
        _formService = formService;
    }

    [HttpGet("get/{interestId}")]
    [ValidateModel]
    public async Task<IActionResult> Get([FromRoute]int interestId)
    {
        var result = await _interestsService.Get(interestId);
        if (result is null) return BadRequest("Интерес не был найден");
        
        return Ok(result);
    }

    [HttpGet("user/get/")]
    public async Task<IActionResult> GetForCurrentUser()
    {
        var user = await _userManager.FindByEmailAsync(HttpContext.User.Identity.Name);
        var interests = await _interestsService.GetAllForUser(user.Id);
        if (!interests.Result)
            return BadRequest("У пользователя нет интересов.");
        return Ok(interests);
    }
    
    [HttpGet("all/")]
    public async Task<IActionResult> GetAll()
    {
        var result = await _interestsService.GetAll();
        if (result.Result is false) return BadRequest("Интересы не были найдены");
        
        return Ok(result);
    }

    [HttpPost("add/")]
    [Authorize(Roles = "Admin")]
    [ValidateModel]
    public async Task<IActionResult> Add([FromBody]CreateInterestDto request)
    {
        var result = await _interestsService.Create(request);
        if (!result.Succeeded) return BadRequest(result.Message);
        
        return Ok();
    }

    [HttpDelete("remove/")]
    [ValidateModel]
    public async Task<IActionResult> Remove([FromBody]DeleteInterestDto request)
    {
        var result = await _interestsService.Delete(request);
        if (!result.Succeeded) return BadRequest(result.Message);
        return Ok();
    }

    [HttpPatch("update/")]
    [ValidateModel]
    public async Task<IActionResult> Update([FromBody]UpdateInterestDto request)
    {
        var result = await _interestsService.Update(request);
        if (!result.Succeeded) return BadRequest(result.Message);
        return Ok();
    }

    [HttpPost("user/add/")]
    [ValidateModel]
    public async Task<IActionResult> AddToCurrentUser([FromBody]AddInterestsToUserDto request)
    {
        var user = await _userManager.FindByEmailAsync(HttpContext.User.Identity.Name);
        var form = await _formService.GetFormForCurrentUser(user.Id);
        if (form is null)
            return BadRequest("Попытка добавить интересы к несуществующей анкете.");
        
        if (string.IsNullOrEmpty(request.FormId))
            request.FormId = form.Id;
        if (form.Id != request.FormId)
            return BadRequest("Попытка добавить интересы к анкете другого пользователя.");
        
        var response = await _interestsService.AddToUser(request);
        if (!response.Succeeded)
            return BadRequest(response.Message);
        
        return Ok();
    }
}