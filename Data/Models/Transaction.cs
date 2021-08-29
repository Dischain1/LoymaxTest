using Data.Enums;
using System;
using System.ComponentModel.DataAnnotations;

namespace Data.Models
{
    public partial class Transaction
    {
        [Key]
        public long Id { get; set; }

        public DateTime Date { get; set; }

        [EnumDataType(typeof(TransactionType))]
        public int Type { get; set; }

        [Range(0, double.PositiveInfinity)]
        public decimal Amount { get; set; }

        public TransactionType GetTransactionType() => (TransactionType)Type;
    }
}
