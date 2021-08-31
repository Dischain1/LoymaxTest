using Services.Accounts.Models;

namespace Services.Accounts.Interfaces
{
    public interface IAccountService
    {
        public int AddAccount(AddAccountDto account);
        public decimal GetBalance(int accountId);
        public bool AccountExist(int accountId);
    }
}