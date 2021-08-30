using System;
using System.ComponentModel.DataAnnotations;

namespace Data.Models
{
    public class DepositTransactionModel
    {
        const decimal MaxDeposit = 1000000;

        public int AccountId { get; set; }

        [Range(0, (double)MaxDeposit)]
        public decimal Deposit { get; set; }
    }
}
