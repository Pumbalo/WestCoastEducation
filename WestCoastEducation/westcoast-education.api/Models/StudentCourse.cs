namespace westcoast_education.api.Models;

public class StudentCourse
{
    public CourseStatusEnum Status { get; set; }
    public Guid CourseId { get; set; }
    public CourseModel? Course { get; set; }

    public Guid StudentId { get; set; }
    public StudentModel? Student { get; set; }
}
