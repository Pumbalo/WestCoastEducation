namespace westcoast_education.web.ViewModels.Course;

public class CourseDetailsViewModel
{
    public Guid courseId { get; set; }
    public string CourseName { get; set; } = "";
    public string CourseTitle { get; set; } = "";
    public string StartDate { get; set; } = "";
    public string CourseLength { get; set; } = "";
    public bool OnSite { get; set; }
    public int CoursePoints { get; set; }
    public string? Status { get; set; }
    public List<StudentListViewModel>? Students { get; set; }
    public TeacherListViewModel? Teachers { get; set; }
}
