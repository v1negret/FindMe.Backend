using FindMe.Models.Dto.Form;
using FindMe.Models.Dto.Interest;
using FindMe.Models.Results;

namespace FindMe.Services.Interfaces;

public interface IInterestsService
{
    public Task<InterestsServiceResult> AddToUser(AddInterestsToUserDto request);
    public Task<GetInterestDto> Get(int interestId);
    public Task<GetAllInterestsDto> GetAll();
    public Task<GetAllInterestsForUserDto> GetAllForUser(string userId);
    public Task<InterestsServiceResult> Create(CreateInterestDto request);
    public Task<InterestsServiceResult> Delete(DeleteInterestDto request);
    public Task<InterestsServiceResult> Update(UpdateInterestDto request);
}