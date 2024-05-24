using FindMe.Models.Dto.Form;
using FindMe.Models.Results;

namespace FindMe.Services.Interfaces;

public interface IFormService
{
    public Task<FormServiceResult> Create(CreateFormDto request);
    public Task<GetFormDto?> Get(string formId);
    public Task<GetFormDto?> GetByInterests(string userId);
    public Task<GetFormDto?> GetFormForCurrentUser(string userEmail);
    public Task<FormServiceResult> Update(UpdateFormDto request);
    public Task<FormServiceResult> Delete(DeleteFormDto request);
    public Task<FormServiceResult> DeleteById(string formId);
    
}