using System;

namespace Services.Accounts.Models
{
    public class AddAccountDto
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Panronimic { get; set; }
        public DateTime DateOfBirth { get; set; }
        public DateTime RegistrationDate { get; set; }
    }
}