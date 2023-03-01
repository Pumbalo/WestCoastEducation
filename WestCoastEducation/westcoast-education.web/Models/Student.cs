
namespace westcoast_education.web.Models;

public class Students
{
    public int StudentsId { get; set; }
    public string FirstName { get; set; } = "";
    public string LastName { get; set; } = "";
    public string Email { get; set; } = "";
    public string PhoneNumber { get; set; } = "";
    public string Adress { get; set; } = "";
    public bool IsStudent { get; set; } = true;
}