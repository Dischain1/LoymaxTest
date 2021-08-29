using Services.Transactions;
using Services.Transactions.Models;
using System;

namespace Services
{
    public class TransactionService : ITransactionService
    {
        public TransactionService()
        {

        }

        public DepositFeedback Deposit()
        {
            throw new NotImplementedException();
        }

        public WithdrawalFeedback Withdraw()
        {
            throw new NotImplementedException();
        }
    }
}
