namespace westcoast_education.api.ViewModels;

public class TeacherPostViewModel : PersonViewModel
{
    public IList<string> Competence { get; set; } = new List<string>();
}
