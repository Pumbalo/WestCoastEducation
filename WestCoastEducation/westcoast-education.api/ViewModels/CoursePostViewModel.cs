using System.ComponentModel.DataAnnotations;

namespace westcoast_education.api.ViewModels;

public class CoursePostViewModel
{
    [Required(ErrorMessage = "Course Name is missing")]
    [StringLength(128, MinimumLength = 6)]
    public string CourseName { get; set; } = "";
    [Required(ErrorMessage = "Course Title is missing")]
    public string CourseTitle { get; set; } = "";
    [Required(ErrorMessage = "Start Date is missing")]
    public string StartDate { get; set; } = "";
    [Required(ErrorMessage = "Course Length is missing")]
    public string CourseLength { get; set; } = "";
    [Required(ErrorMessage = "On Site is missing")]
    public bool OnSite { get; set; }
    [Required(ErrorMessage = "Course Points is missing")]
    public int CoursePoints { get; set; }
}
