using System.ComponentModel.DataAnnotations;

namespace westcoast_education.api.Models;

public class CompetenceModel
{
    public Guid Id { get; set; }
    public string? CompetenceName { get; set; }
    public ICollection<TeacherModel>? Teacher { get; set; }
}
