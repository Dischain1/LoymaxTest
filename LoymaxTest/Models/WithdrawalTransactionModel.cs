using System;
using System.ComponentModel.DataAnnotations;

namespace Data.Models
{
    public class WithdrawalTransactionModel
    {
        const decimal MaxWithdrawal = 1000000;

        public int AccountId { get; set; }

        [Range(0, (double)MaxWithdrawal)]
        public decimal Withdrawal { get; set; }
    }
}
