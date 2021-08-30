using Data;
using Data.Models;
using Microsoft.EntityFrameworkCore;
using Services.Transactions;
using Services.Transactions.Models;
using System;
using System.Data;

namespace Services
{
    public class TransactionService : ITransactionService
    {
        private readonly LoymaxTestContext _context;

        public TransactionService(LoymaxTestContext context)
        {
            _context = context;
        }

        public TransactionResult AddTransaction(AddTransactionDto transactionDto)
        {
            Transaction transactionEntity = new Transaction
            {
                Type = (int)transactionDto.Type,
                AccountId = transactionDto.AccountId,
                Amount = transactionDto.Amount,
                Date = DateTime.Now.ToUniversalTime()
            };

            try
            {
                using var transaction = _context.Database.BeginTransaction(IsolationLevel.RepeatableRead);
                _context.Transactions.Add(transactionEntity);
                transaction.Commit();

                return TransactionResult.CreateSuccededResult();
            }
            catch (Exception e)
            {
                return TransactionResult.CreateFailedResult(e.Message);
            }
        }
    }
}
