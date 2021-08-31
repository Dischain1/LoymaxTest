using System;
using System.Data;
using System.Linq;
using Data;
using Data.Enums;
using Data.Models;
using Microsoft.EntityFrameworkCore;
using Services.Accounts.Interfaces;
using Services.Accounts.Models;

namespace Services.Accounts
{
    public class AccountService : IAccountService
    {
        private readonly LoymaxTestContext _context;

        public AccountService(LoymaxTestContext context)
        {
            _context = context;
        }

        public bool AccountExist(int accountId)
        {
            return _context.Accounts.Any(x => x.Id == accountId);
        }

        public int AddAccount(AddAccountDto account)
        {
            var entity = new Account
            {
                FirstName = account.FirstName,
                LastName = account.LastName,
                Patronimic = account.Patronimic,
                DateOfBirth = account.DateOfBirth,
                RegistrationDate = DateTime.Now.ToUniversalTime(),
            };

            _context.Accounts.Add(entity);
            _context.SaveChanges();

            return entity.Id;
        }

        public decimal CalculateBalance(int accountId, bool isUsedInsideTransaction = false)
        {
            if (isUsedInsideTransaction) 
                return CalculateBalanceUsingTotalDepositAndWithdrawal(accountId);

            using var transaction = _context.Database.BeginTransaction(IsolationLevel.RepeatableRead);
            return CalculateBalanceUsingTotalDepositAndWithdrawal(accountId);
        }

        private decimal CalculateBalanceUsingTotalDepositAndWithdrawal(int accountId)
        {
            var totalDeposit = _context.Transactions
                .Where(x => x.AccountId == accountId)
                .Where(x => x.Type == (int)TransactionType.Deposit)
                .Sum(x => x.Amount);

            var totalWithdrawal = _context.Transactions
                .Where(x => x.AccountId == accountId)
                .Where(x => x.Type == (int)TransactionType.Withdrawal)
                .Sum(x => x.Amount);

            return totalDeposit - totalWithdrawal;
        }

        public decimal CalculateBalanceInMemory(int accountId)
        {
            var accountTransactions = _context.Transactions
                .Where(x => x.AccountId == accountId)
                .Select(x=>new { x.Type, x.Amount })
                .ToList();

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
