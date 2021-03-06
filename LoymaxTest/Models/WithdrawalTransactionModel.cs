using Services.Common;
using System.ComponentModel.DataAnnotations;

namespace LoymaxTest.Models
{
    public class WithdrawalTransactionModel
    {
        [Range(CommonConstants.LowestExistingAccountId, int.MaxValue)]
        public int AccountId { get; set; }

        [Range(0, (double)CommonConstants.WithdrawalLimit)]
        public decimal Withdrawal { get; set; }
    }
}
