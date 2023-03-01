using System.ComponentModel.DataAnnotations;

namespace westcoast_education.api.Models;

public class TeacherModel : Person
{
    public ICollection<CourseModel>? Course { get; set; }
    public ICollection<CompetenceModel>? Competence { get; set; }
}
