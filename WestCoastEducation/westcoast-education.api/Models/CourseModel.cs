using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace westcoast_education.api.Models;

public class CourseModel
{
    [Key]
    public Guid CourseId { get; set; }
    public string CourseName { get; set; } = "";
    public string CourseTitle { get; set; } = "";
    public string StartDate { get; set; } = "";
    public string CourseLength { get; set; } = "";
    public bool OnSite { get; set; }
    public int CoursePoints { get; set; }

    public ICollection<StudentCourse>? StudentCourse { get; set; }

    public Guid? TeacherId { get; set; }
    public TeacherModel? Teacher { get; set; }

}
