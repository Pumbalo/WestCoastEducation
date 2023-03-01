
namespace westcoast_education.web.Models
{

    public class Teachers
    {
        public int TeachersId { get; set; }
        public string FirstName { get; set; } = "";
        public string LastName { get; set; } = "";
        public string Email { get; set; } = "";
        public string PhoneNumber { get; set; } = "";
        public string Adress { get; set; } = "";
        public bool IsStudent { get; set; } = false;
    }
}