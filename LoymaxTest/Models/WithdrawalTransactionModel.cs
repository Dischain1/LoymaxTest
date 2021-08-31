using System.ComponentModel.DataAnnotations;

namespace LoymaxTest.Models
{
    public class WithdrawalTransactionModel
    {
        private const decimal MaxWithdrawal = 1000000;

        public int AccountId { get; set; }

        [Range(0, (double)MaxWithdrawal)]
        public decimal Withdrawal { get; set; }
    }
}
