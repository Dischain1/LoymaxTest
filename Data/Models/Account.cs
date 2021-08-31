using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Data.Models
{
    public class Account
    {
        public Account()
        {
            this.Transactions = new HashSet<Transaction>();
        }

        [Key]
        public int Id { get; set; }
        public DateTime RegistrationDate { get; set; }

        [Required]
        [MaxLength(50)]
        public string FirstName { get; set; }

        [Required]
        [MaxLength(50)]
        public string LastName { get; set; }

        [MaxLength(50)]
        public string Patronimic { get; set; }

        public DateTime DateOfBirth { get; set; }

        public virtual ICollection<Transaction> Transactions { get; set; }
    }
}