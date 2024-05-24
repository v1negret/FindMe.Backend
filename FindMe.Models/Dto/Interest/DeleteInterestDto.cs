using System.ComponentModel.DataAnnotations;

namespace FindMe.Models.Dto.Interest;

public class DeleteInterestDto
{
    [Required]
    public int Id { get; set; }
    [Required]
    public string Name { get; set; } = string.Empty;
}