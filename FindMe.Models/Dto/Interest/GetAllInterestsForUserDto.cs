namespace FindMe.Models.Dto.Interest;

public class GetAllInterestsForUserDto
{
    public bool Result { get; set; }
    public List<Models.Interest>? Interests { get; set; }
}