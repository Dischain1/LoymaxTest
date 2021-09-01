using Data;
using Data.Enums;
using Data.Models;
using Microsoft.EntityFrameworkCore;
using Services.Accounts.Interfaces;
using Services.Accounts.Models;
using System;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace Services.Accounts
{
    public class AccountService : IAccountService
    {
        private readonly LoymaxTestContext _context;

        public AccountService(LoymaxTestContext context)
        {
            _context = context;
        }

        public async Task<decimal> GetBalance(int accountId)
        {
            return await _context.Accounts
                .Where(x => x.Id == accountId)
                .Select(x => x.CurrentBalance)
                .FirstAsync();
        }

        public async Task<bool> AccountExist(int accountId)
        {
            return await _context.Accounts.AnyAsync(x => x.Id == accountId);
        }

        public async Task<int> AddAccount(AddAccountDto account)
        {
            var entity = new Account
            {
                FirstName = account.FirstName,
                LastName = account.LastName,
                Patronymic = account.Patronymic,
                DateOfBirth = account.DateOfBirth,
                RegistrationDate = DateTime.Now.ToUniversalTime(),
            };

            await _context.Accounts.AddAsync(entity);
            await _context.SaveChangesAsync();

            return entity.Id;
        }

        public async Task<decimal> CalculateBalance(int accountId, bool isUsedInsideTransaction = false)
        {
            var totalDeposit = await _context.Transactions
                .Where(x => x.AccountId == accountId)
                .Where(x => x.Type == (int)TransactionType.Deposit)
                .SumAsync(x => x.Amount);

            var totalWithdrawal = await _context.Transactions
                .Where(x => x.AccountId == accountId)
                .Where(x => x.Type == (int)TransactionType.Withdrawal)
                .SumAsync(x => x.Amount);

            return totalDeposit - totalWithdrawal;
        }

        public async Task<decimal> CalculateBalanceInMemory(int accountId)
        {
            var accountTransactions = await _context.Transactions
                .Where(x => x.AccountId == accountId)
                .Select(x => new { x.Type, x.Amount })
                .ToListAsync();

            var sum = 0M;
            const int depositCode = (int)TransactionType.Deposit;
            const int withdrawalCode = (int)TransactionType.Withdrawal;

            foreach (var transaction in accountTransactions)
            {
                switch (transaction.Type)
                {
                    case depositCode:
                        sum += transaction.Amount;
                        break;
                    case withdrawalCode:
                        sum -= transaction.Amount;
                        break;
                    default:
                        throw new ArgumentOutOfRangeException(nameof(transaction.Type));
                }
            }

            return sum;
        }
    }
}
