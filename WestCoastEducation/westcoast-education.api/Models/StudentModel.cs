using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace westcoast_education.api.Models;

public class StudentModel : Person
{
    public ICollection<StudentCourse>? StudentCourses { get; set; }
}