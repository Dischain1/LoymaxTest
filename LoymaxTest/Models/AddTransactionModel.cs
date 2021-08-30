using Data.Enums;
using System;
using System.ComponentModel.DataAnnotations;

namespace Data.Models
{
    public class AddTransactionModel : TransactionBaseModel
    {
        [EnumDataType(typeof(TransactionType))]
        public int Type { get; set; }

        public TransactionType GetTransactionType() => (TransactionType)Type;
    }

    public class TransactionBaseModel
    {
        public int AccountId { get; set; }

        [Range(0, double.PositiveInfinity)]
        public decimal Amount { get; set; }
    }
}
