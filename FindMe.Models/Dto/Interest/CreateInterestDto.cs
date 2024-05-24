using System.ComponentModel.DataAnnotations;

namespace FindMe.Models.Dto.Interest;

public class CreateInterestDto
{
    [Required]
    public string Name { get; set; }
}