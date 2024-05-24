using Microsoft.AspNetCore.Identity;

namespace FindMe.Models;

public class SimpRequest
{
    public string Id { get; set; } = new Guid().ToString();
    public string FromUserId { get; set; }
    public string ToUserId { get; set; }
    public IdentityUser? FromUser { get; set; }
    public IdentityUser? ToUser { get; set; }
}