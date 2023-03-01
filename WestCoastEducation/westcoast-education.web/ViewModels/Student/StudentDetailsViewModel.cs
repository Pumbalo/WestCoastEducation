namespace westcoast_education.web.ViewModels.Student;

public class StudentDetailsViewModel
{
    public Guid studentId { get; set; }
    public string FirstName { get; set; } = "";
    public string LastName { get; set; } = "";
    public string Email { get; set; } = "";
    public string PhoneNumber { get; set; } = "";
    public string Adress { get; set; } = "";
    public string ZipCode { get; set; } = "";
    public string City { get; set; } = "";
    public string ProfileAvatar { get; set; } = "";
    public ICollection<CourseListViewModel>? Courses { get; set; }
}

