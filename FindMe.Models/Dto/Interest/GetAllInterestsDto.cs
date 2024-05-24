
namespace FindMe.Models.Dto.Interest;

public class GetAllInterestsDto
{
    public bool Result { get; set; }
    public List<Models.Interest>? Interests { get; set; }
}