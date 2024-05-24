using FindMe.Models.Dto.Form;

namespace FindMe.Models.Dto.Simp;

public class GetSimpDto
{
    public string FromUserId { get; set; }
    public string ToUserId { get; set; }
    public GetFormDto Form { get; set; }
}