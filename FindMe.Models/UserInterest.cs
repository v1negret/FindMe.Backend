using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace FindMe.Models;

[PrimaryKey(nameof(FormId),nameof(InterestId))]
public class UserInterest
{
    public string FormId { get; set; } = String.Empty;
    public int InterestId { get; set; }
    public Form? Form;
    public Interest? Interest;
}