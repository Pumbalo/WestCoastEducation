

namespace westcoast_education.web.Models
{
    public class Courses
    {

        public int CoursesId { get; set; }
        public string CourseName { get; set; } = "";
        public string CourseTitel { get; set; } = "";
        public string StartDate { get; set; } = "";
        public string CourseLength { get; set; } = "";
        public bool OnSite { get; set; }
        public int CoursePoints { get; set; }
    }
}