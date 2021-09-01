using Data;
using Ryadel.Components.Threading;
using Services.Transactions.Interfaces;
using Services.Transactions.Models;
using System;
using System.Threading.Tasks;
using Transaction = Data.Models.Transaction;

namespace Services.Transactions
{
    public class TransactionService : ITransactionService
    {
        private readonly LoymaxTestContext _context;
        private readonly ITransactionValidator _transactionValidator;

        private static readonly LockProvider<int> LockProvider = new LockProvider<int>();

        public TransactionService(LoymaxTestContext context,
            ITransactionValidator transactionValidator
            )
        {
            _context = context;
            _transactionValidator = transactionValidator;
        }

        public async Task<AddTransactionResult> SaveTransactionLockedByAccountId(AddTransactionDto transactionDto)
        {
            // lock for current account Id only
            await LockProvider.WaitAsync(transactionDto.AccountId);

            try
            {
                var addTransactionResult = await SaveTransaction(transactionDto);
                return addTransactionResult;
            }
            finally
            {
                // release the lock
                LockProvider.Release(transactionDto.AccountId);
            }
        }

        private async Task<AddTransactionResult> SaveTransaction(AddTransactionDto transactionDto)
        {
            await using var transaction = await _context.Database.BeginTransactionAsync();
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
