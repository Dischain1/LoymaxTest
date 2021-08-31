using Data;
using Data.Models;
using Microsoft.EntityFrameworkCore;
using Services.Transactions.Interfaces;
using Services.Transactions.Models;
using System;
using System.Data;
using System.Threading.Tasks;

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
            await using (var transaction = await _context.Database.BeginTransactionAsync(IsolationLevel.Snapshot))
            {
                try
                {
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
                    await transaction.CommitAsync();

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
