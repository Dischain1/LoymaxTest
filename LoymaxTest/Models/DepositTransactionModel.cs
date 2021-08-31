using Services.Common;
using System.ComponentModel.DataAnnotations;

namespace LoymaxTest.Models
{
    public class DepositTransactionModel
    {
        [Range(CommonConstants.LowestExistingAccountId, int.MaxValue)]
        public int AccountId { get; set; }

        [Range(0, (double)CommonConstants.DepositLimit)]
        public decimal Deposit { get; set; }
    }
}
