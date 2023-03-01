using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace westcoast_education.api.Models
{
    public class Person
    {
        public Guid Id { get; set; }
        public string FirstName { get; set; } = "";
        public string LastName { get; set; } = "";
        public string Email { get; set; } = "";
        public string PhoneNumber { get; set; } = "";
        public string Adress { get; set; } = "";
        public string? ZipCode { get; set; }
        public string? City { get; set; }
        public string? ProfileAvatar { get; set; }
        public TitleEnum Title { get; set; }
    }
}