using westcoast_education.api.Models;

namespace westcoast_education.api.ViewModels;

public class PersonViewModel
{
    public string FirstName { get; set; } = "";
    public string LastName { get; set; } = "";
    public string Email { get; set; } = "";
    public string PhoneNumber { get; set; } = "";
    public string Adress { get; set; } = "";
    public string? ZipCode { get; set; }
    public string? City { get; set; }
}
