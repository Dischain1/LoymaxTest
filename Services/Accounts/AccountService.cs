using Data;
using Services.Accounts;
using Services.Accounts.Models;
using System;

namespace Services
{
    public class AccountService : IAccountService
    {
        LoymaxTestContext _context;

        public AccountService(LoymaxTestContext context)
        {
            _context = context;
        }

        public int AddAccount(AddAccountDto account)
        {
            throw new NotImplementedException();
        }

        public decimal GetBalance(int accountId)
        {
            throw new NotImplementedException();
        }
    }
}
