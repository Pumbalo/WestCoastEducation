using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace westcoast_education.web.ViewModels;

public class CourseUpdateViewModel
{
    public int CoursesId { get; set; }
    [Required(ErrorMessage = "Course Name is requried!")]
    [DisplayName("Course Name")]
    public string CourseName { get; set; } = "";
    [Required(ErrorMessage = "Course Title is requried!")]
    [DisplayName("Course Title")]
    public string CourseTitel { get; set; } = "";
    [Required(ErrorMessage = "Start Date is requried!")]
    [DisplayName("Start Date")]
    public string StartDate { get; set; } = "";
    [Required(ErrorMessage = "Course Length is requried!")]
    [DisplayName("Course Length")]
    public string CourseLength { get; set; } = "";
    [Required(ErrorMessage = "On Site is requried!")]
    [DisplayName("On Site")]
    public bool OnSite { get; set; }
    [Required(ErrorMessage = "Course Points is requried!")]
    [DisplayName("Course Points")]
    public int CoursePoints { get; set; }
}
