using System.ComponentModel.DataAnnotations;

namespace FindMe.Models.Dto.Simp;

public class DeleteSimpDto
{
    [Required]
    public string FromUserId { get; set; }
    [Required]
    public string ToUserId { get; set; }
}