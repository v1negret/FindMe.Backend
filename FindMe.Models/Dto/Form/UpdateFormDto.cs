using System.ComponentModel.DataAnnotations;

namespace FindMe.Models.Dto.Form;

public class UpdateFormDto
{
    [Required]
    public string Id { get; set; }
    [Required]
    [Range(18, 99, ErrorMessage = "Возраст должен быть от 18 до 99")]
    public byte Age { get; set; }
    [MinLength(2, ErrorMessage = "Минимальная длинна имени от 2 символов")]
    [MaxLength(16, ErrorMessage = "Максимальная длинна имени до 2 символов")]
    [Required(ErrorMessage = "Поле имени является обязательным")]
    public string Name { get; set; }
    [Required(ErrorMessage = "Поле описания являтся обязательным")]
    [MaxLength(500,ErrorMessage = "Максимальная длинна описания до 500 символов")]
    public string Description { get; set; }
}