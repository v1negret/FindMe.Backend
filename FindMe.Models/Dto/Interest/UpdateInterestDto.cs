using System.ComponentModel.DataAnnotations;

namespace FindMe.Models.Dto.Interest;

public class UpdateInterestDto
{
    [Required]
    public int Id { get; set; }
    [Required]
    public string Name { get; set; } = String.Empty;
}