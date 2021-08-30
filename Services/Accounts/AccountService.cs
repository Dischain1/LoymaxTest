﻿using Data;
using Data.Enums;
using Data.Models;
using Microsoft.EntityFrameworkCore;
using Services.Accounts;
using Services.Accounts.Models;
using System;
using System.Data;
using System.Linq;

namespace Services
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
                Panronimic = account.Panronimic,
                DateOfBirth = account.DateOfBirth,
                RegistrationDate = DateTime.Now.ToUniversalTime(),
            };

            _context.Accounts.Add(entity);
            _context.SaveChanges();

            return entity.Id;
        }

        public decimal GetBalance(int accountId)
        {
            var depositTransactions = _context.Transactions
                .Where(x => x.AccountId == accountId)
                .Where(x => x.Type == (int)TransactionType.Deposit);

            var withdrawalTransactions = _context.Transactions
                .Where(x => x.AccountId == accountId)
                .Where(x => x.Type == (int)TransactionType.Withdrawal);

            using var transaction = _context.Database.BeginTransaction(IsolationLevel.RepeatableRead);
            var totalDeposit = depositTransactions.Sum(x => x.Amount);
            var totalWithdrawal = depositTransactions.Sum(x => x.Amount);
            transaction.Commit();
            return totalDeposit - totalWithdrawal;
        }
    }
}
