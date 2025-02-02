using System.ComponentModel.DataAnnotations;

namespace hrustinszkimail.Models;

public enum GenderType {
    Male,
    Female,
    Other,
    PreferNotToSay
}

public class Email
{
    [Required, EmailAddress]
    public string Address { get; set; }

    [Required, StringLength(30, MinimumLength = 2)]
    public string FirstName { get; set; }

    [Required, StringLength(30, MinimumLength = 2)]
    public string LastName { get; set; }

    [Required]
    public GenderType Gender { get; set; }

    public Email()
    {
        FirstName = string.Empty;
        LastName = string.Empty;
        Address = string.Empty;
        Gender = GenderType.PreferNotToSay;
    }

    public bool IsValid(List<Email> emails)
    {
        return Validator.TryValidateObject(this, new ValidationContext(this), null, true) && !emails.Any(e => e.Address == Address);
    }
    
    public static List<Email> LoadEmails(string filePath) {
        List<Email> emails = [];
        if(!File.Exists(filePath)) {
            File.Create(filePath).Close();
            return emails;
        }

        string[] lines = File.ReadAllLines(filePath);
        foreach (string line in lines) {
            string[] parts = line.Split(';');
            if(parts.Length != 4) {
                continue;
            }
            Email email = new() {
                Address = parts[0],
                FirstName = parts[1],
                LastName = parts[2],
                Gender = Enum.Parse<GenderType>(parts[3])
            };

            if(email.IsValid(emails)) {
                emails.Add(email);
            }
        }

        return emails;
    }

    public static void SaveEmails(string filePath, List<Email> emails) {
        List<string> lines = emails.Select(e => $"{e.Address};{e.FirstName};{e.LastName};{e.Gender}").ToList();
        File.WriteAllLines(filePath, lines);
    }
}