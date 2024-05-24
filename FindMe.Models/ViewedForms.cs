using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace FindMe.Models;

[PrimaryKey(nameof(FormId),nameof(ViewedFormId))]
public class ViewedForms
{
    public string FormId { get; set; }
    public string ViewedFormId { get; set; }
    public Form? Form;
    public Form? ViewedForm;
}