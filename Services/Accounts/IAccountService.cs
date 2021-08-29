using Services.Accounts.Models;

namespace Services.Accounts
{
    public interface IAccountService
    {
        public int AddAccount(AddAccountDto account);
        public decimal GetBalance(int accountId);
    }
}