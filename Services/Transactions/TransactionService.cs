using Data;
using Microsoft.EntityFrameworkCore;
using Services.Transactions.Interfaces;
using Services.Transactions.Models;
using System;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Transactions;
using Transaction = Data.Models.Transaction;

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

        public async Task<AddTransactionResult> SaveTransaction(AddTransactionDto transactionDto)
        {
            // Validation and saving Account's deposit\withdrawal should be one EF transaction: 
            // Any new Account's deposits\withdrawals can affect correctness of validation result
            using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Required, new TransactionOptions
            {
                IsolationLevel = System.Transactions.IsolationLevel.ReadCommitted,
            }, TransactionScopeAsyncFlowOption.Enabled))
            {
                try
                {
                    var account = _context.Accounts.First();
                    account.FirstName += "1";
                    await _context.SaveChangesAsync();

                    var validationResult = await _transactionValidator.Validate(transactionDto, isUsedInsideTransaction: true);

                    if (!validationResult.Valid)
                    {
                        return AddTransactionResult.FailedResult(validationResult.Errors);
                    }

                    _context.Transactions.Add(new Transaction
                    {
                        Type = (int)transactionDto.Type,
                        AccountId = transactionDto.AccountId,
                        Amount = transactionDto.Amount,
                        Date = DateTime.Now
                    });

                    await _context.SaveChangesAsync();
                    scope.Complete();

                    return AddTransactionResult.SucceededResult();
                }
                catch (Exception exception)
                {
                    return AddTransactionResult.FailedResult(exception.Message);
                }
            }
        }
    }
}
