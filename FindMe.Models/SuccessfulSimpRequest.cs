namespace FindMe.Models;

public class SuccessfulSimpRequest
{
    public string Id { get; set; } = new Guid().ToString();
    public string SimpRequestId { get; set; }
}