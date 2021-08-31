using Data.Enums;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Data.Models
{
    public class Transaction
    {
        [Key]
        public long Id { get; set; }

        public DateTime Date { get; set; }

        public int Type { get; set; }
        public decimal Amount { get; set; }

        [ForeignKey("Account")]
        public int AccountId { get; set; }
        public virtual Account Account { get; set; }
    }
}
