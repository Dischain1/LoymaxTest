﻿using System.Threading.Tasks;
using Services.Accounts.Models;

namespace Services.Accounts.Interfaces
{
    public interface IAccountService
    {
        public Task<int> AddAccount(AddAccountDto account);
        public Task<decimal> CalculateBalance(int accountId, bool isUsedInsideTransaction = false);
        public Task<decimal> CalculateBalanceInMemory(int accountId);
        public Task<bool> AccountExist(int accountId);
    }
}