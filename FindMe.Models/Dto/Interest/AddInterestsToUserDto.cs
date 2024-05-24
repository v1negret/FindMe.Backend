using System.ComponentModel.DataAnnotations;

namespace FindMe.Models.Dto.Interest;

public class AddInterestsToUserDto
{
    [Required]
    public string? FormId { get; set; }
    [Required]
    public List<int> Interests { get; set; }
}