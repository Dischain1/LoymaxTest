using System;

namespace Services.Accounts.Models
{
    public class AddAccountDto
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Patronymic { get; set; }
        public DateTime DateOfBirth { get; set; }
    }
}