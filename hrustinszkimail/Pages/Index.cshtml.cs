using hrustinszkimail.Models;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace hrustinszkimail.Pages;

public class IndexModel : PageModel
{
    public List<Email> Emails { get; set; } = [];

    public void OnGet()
    {
        Emails = Email.LoadEmails("data/emails.csv");
    }
}
