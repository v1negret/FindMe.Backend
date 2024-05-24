using System.ComponentModel.DataAnnotations;

namespace FindMe.Models.Identity.Dto;

public class LoginDto
{
    [Required]
    [DataType(DataType.EmailAddress)]
    public string Email { get; set; } = string.Empty;
    [Required]
    [DataType(DataType.Password)]
    public string Password { get; set; } = string.Empty;
}