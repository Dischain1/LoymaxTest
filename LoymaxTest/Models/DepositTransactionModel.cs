using System.ComponentModel.DataAnnotations;

namespace LoymaxTest.Models
{
    public class DepositTransactionModel
    {
        private const decimal MaxDeposit = 1000000;

        public int AccountId { get; set; }

        [Range(0, (double)MaxDeposit)]
        public decimal Deposit { get; set; }
    }
}
