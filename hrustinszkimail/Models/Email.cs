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
    public const string Domain = "@hrustinszkimail.com";

    [Required, RegularExpression("^[a-zA-Z0-9._%+-]+$", ErrorMessage = "Invalid characters")]
    public string Prefix { get; set; }

    [Required, EmailAddress]
    public string Address { get => $"{Prefix}{Domain}"; }

    [Required, StringLength(30, MinimumLength = 2)]
    public string FirstName { get; set; }

    [Required, StringLength(30, MinimumLength = 2)]
    public string LastName { get; set; }

    [Required, EnumDataType(typeof(GenderType))]
    public GenderType Gender { get; set; }

    public Email()
    {
        FirstName = string.Empty;
        LastName = string.Empty;
        Prefix = string.Empty;
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
                Prefix = parts[0].Split('@')[0],
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