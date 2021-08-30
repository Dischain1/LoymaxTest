using System;
using System.ComponentModel.DataAnnotations;

namespace LoymaxTest.Models
{
    public class AddAccountModel
    {
        [Required]
        [StringLength(50)]
        public string FirstName { get; set; }

        [Required]
        [StringLength(50)]
        public string LastName { get; set; }

        [StringLength(50)]
        public string Panronimic { get; set; }

        public DateTime DateOfBirth { get; set; }
    }
}