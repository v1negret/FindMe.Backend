using FindMe.Models.Dto.Form;
using FindMe.Models.Dto.Interest;
using FindMe.Services.Interfaces;
using FindMe.WebApi.Filters.Attributes;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace FindMe.WebApi.Controllers;

[ApiController]
[Authorize]
[Route("api/[controller]")]
public class FormController : ControllerBase
{
    private readonly IFormService _formService;
    private readonly IInterestsService _interestsService;
    private readonly UserManager<IdentityUser> _userManager;
    public FormController(IFormService formService, UserManager<IdentityUser> userManager, IInterestsService interestsService)
    {
        _formService = formService;
        _userManager = userManager;
        _interestsService = interestsService;
    }

    [HttpGet("find/")]
    public async Task<IActionResult> FindByInterests()
    {
        var user = await _userManager.FindByEmailAsync(HttpContext.User.Identity.Name);
        var form = await _formService.GetFormForCurrentUser(user.Id);
        if (form is null)
            return BadRequest("Для начала необходимо создать анкету");
        
        var interests = await _interestsService.GetAllForUser(user.Id);
        if (!interests.Result)
            return BadRequest("Для начала добавьте интересы к анкете.");
        
        var response = await _formService.GetByInterests(user.Id);
        if (response is null)
            return BadRequest("Подходящих анкет не найдено. Попробуйте изменить интересы.");
        
        return Ok(response);
    }

    [HttpGet("get/{formId}")]
    [ValidateModel]
    public async Task<IActionResult> Get([FromRoute]string formId)
    {
        var response = await _formService.Get(formId);
        if (response is null) return BadRequest("Интерес не был найден.");
        return Ok(response);
    }

    [HttpGet("get/")]
    public async Task<IActionResult> GetForCurrentUser()
    {
        var user = await _userManager.GetUserAsync(User);
        var response = await _formService.GetFormForCurrentUser(user.Id);

        if (response is null) return BadRequest("Скорее всего, вы ещё не создали анкету 😢.");
        return Ok(response);
    }

    [HttpPost("create")]
    [ValidateModel]
    public async Task<IActionResult> Create(CreateFormDto request)
    {
        var currentUser = await _userManager.GetUserAsync(User);
        request.UserId = currentUser.Id;
        var response = await _formService.Create(request);
        if (!response.Succeeded) return BadRequest(response.Message);
        
        return Ok(response);
    }
    [HttpPatch("update")]
    [ValidateModel]
    public async Task<IActionResult> Update([FromBody]UpdateFormDto request)
    {
        var response = await _formService.Update(request);
        if (!response.Succeeded) return BadRequest(response.Message);
        
        return Ok(response);
    }
    
    [HttpDelete("delete")]
    [ValidateModel]
    public async Task<IActionResult> Delete([FromBody]DeleteFormDto request)
    {
        var response = await _formService.Delete(request);
        if (!response.Succeeded) return BadRequest(response.Message);
        
        return Ok(response);
    }
    [HttpDelete("delete/{id}")]
    [ValidateModel]
    public async Task<IActionResult> DeleteById([FromRoute]string id)
    {
        var response = await _formService.DeleteById(id);
        if (!response.Succeeded) return BadRequest(response.Message);
        
        return Ok(response);
    }
}