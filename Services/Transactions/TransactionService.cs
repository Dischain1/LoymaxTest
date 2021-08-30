using Data;
using Services.Transactions;
using Services.Transactions.Models;
using System;

namespace Services
{
    public class TransactionService : ITransactionService
    {
        LoymaxTestContext _context;

        public TransactionService(LoymaxTestContext context)
        {
            _context = context;
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
