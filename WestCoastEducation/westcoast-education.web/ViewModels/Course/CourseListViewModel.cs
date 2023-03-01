namespace westcoast_education.web.ViewModels;

public class CourseListViewModel
{
    public Guid CourseId { get; set; }
    public string CourseName { get; set; } = "";
    public string CourseTitle { get; set; } = "";
    public string StartDate { get; set; } = "";
    public string CourseLength { get; set; } = "";
    public bool OnSite { get; set; }
    public int CoursePoints { get; set; }
    public string? Status { get; set; }
}