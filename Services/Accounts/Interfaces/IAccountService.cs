using Services.Accounts.Models;

namespace Services.Accounts.Interfaces
{
    public interface IAccountService
    {
        public int AddAccount(AddAccountDto account);
        public decimal CalculateBalance(int accountId, bool isUsedInsideTransaction = false);
        public decimal CalculateBalanceInMemory(int accountId);
        public bool AccountExist(int accountId);
    }
}