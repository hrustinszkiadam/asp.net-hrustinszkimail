using System.ComponentModel.DataAnnotations;
using hrustinszkimail.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace hrustinszkimail.Pages;

public class CreateModel : PageModel
{
    public List<Email> Emails { get; set; } = []; 

    [BindProperty]
    public Email Email { get; set; } = new();
    
    public IActionResult OnPost()
    {
        Emails = Email.LoadEmails("data/emails.csv");

        if(Emails.Any(e => e.Address == Email.Address))
        {
            ModelState.AddModelError("Email.Prefix", "Email already exists");
            return Page();
        }

        if (!ModelState.IsValid)
        {
            return Page();
        }

        Emails.Add(Email);
        Email.SaveEmails("data/emails.csv", Emails);

        return RedirectToPage("/Index");
    }
}
