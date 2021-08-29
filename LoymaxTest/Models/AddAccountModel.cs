using System;
using System.ComponentModel.DataAnnotations;

namespace Data.Models
{
    public class AddAccountModel
    {
        [Required]
        [MaxLength(50)]
        public string FirstName { get; set; }

        [Required]
        [MaxLength(50)]
        public string LastName { get; set; }

        [MaxLength(50)]
        public string Panronimic { get; set; }

        public DateTime DateOfBirth { get; set; }

        public DateTime RegistrationDate { get; set; }
    }
}