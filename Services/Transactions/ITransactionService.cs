using Services.Transactions.Models;

namespace Services.Transactions
{
    public interface ITransactionService
    {
        public DepositFeedback Deposit();

        public WithdrawalFeedback Withdraw();
    }
}