using Data;
using Data.Models;
using Microsoft.EntityFrameworkCore;
using Services.Transactions.Models;
using System;
using System.Data;
using Services.Transactions.Interfaces;

namespace Services.Transactions
{
    public class TransactionService : ITransactionService
    {
        private readonly LoymaxTestContext _context;
        private readonly ITransactionValidator _transactionValidator;

        public TransactionService(LoymaxTestContext context,
            ITransactionValidator transactionValidator
            )
        {
            _context = context;
            _transactionValidator = transactionValidator;
        }

        public TransactionResult AddTransaction(AddTransactionDto transactionDto)
        {
            var transactionEntity = new Transaction
            {
                Type = (int)transactionDto.Type,
                AccountId = transactionDto.AccountId,
                Amount = transactionDto.Amount
            };

            try
            {
                // Validation and saving Account's transaction should be one EF transaction. 
                // Any new Account's transactions can affect correctness of validation result
                using var transaction = _context.Database.BeginTransaction(IsolationLevel.RepeatableRead);
                var validationResult = _transactionValidator.Validate(transactionDto, isUsedInsideTransaction: true);

                if (validationResult.Valid)
                {
                    _context.Transactions.Add(transactionEntity);
                    _context.SaveChanges();
                    transaction.Commit();
                }
                else
                {
                    transaction.Rollback();
                    return TransactionResult.FailedResult(validationResult.Errors);
                }

                return TransactionResult.SucceededResult();
            }
            catch (Exception e)
            {
                return TransactionResult.FailedResult(e.Message);
            }
        }
    }
}
