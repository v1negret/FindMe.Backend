using FindMe.Models.Dto.Simp;
using FindMe.Models.Results;

namespace FindMe.Services.Interfaces;

public interface ISimpService
{
    public Task<SimpServiceResult> Add(AddSimpDto request);
    public Task<SimpServiceResult> Delete(DeleteSimpDto request);
    public Task<SimpServiceResult> Update(UpdateSimpDto requets);
    public Task<SimpServiceResult> Get();
    public Task<List<GetSimpDto>> GetIncoming(string userId);
    public Task<List<GetSimpDto>> GetOutgoing(string userId);
}