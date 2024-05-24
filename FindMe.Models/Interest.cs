using System.ComponentModel.DataAnnotations;

namespace FindMe.Models;

public class Interest
{
    public int Id { get; set; }
    [MinLength(3, ErrorMessage="Название должно иметь длинну больше 3 символов")]
    public string Name { get; set; } = string.Empty;
}