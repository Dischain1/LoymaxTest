using Data.Enums;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Data.Models
{
    public partial class Transaction
    {
        [Key]
        public long Id { get; set; }

        public DateTime Date { get; set; }

        public int Type { get; set; }
        public decimal Amount { get; set; }

        public TransactionType GetTransactionType() => (TransactionType)Type;
    }
}
