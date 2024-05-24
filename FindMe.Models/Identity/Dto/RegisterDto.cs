using System.ComponentModel.DataAnnotations;

namespace FindMe.Models.Identity.Dto;

public class RegisterDto
{
    [Required]
    [DataType(DataType.EmailAddress)]
    public string Email { get; set; } = string.Empty;
    [Required]
    [DataType(DataType.Password)]
    public string Password { get; set; } = String.Empty;
    [Required]
    [DataType(DataType.Password)]
    [Compare("Password", ErrorMessage = "Пароли не совпадают")]
    public string ConfirmPassword { get; set; } = String.Empty;
}