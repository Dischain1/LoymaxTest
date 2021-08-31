using Services.Transactions.Models;

namespace Services.Transactions.Interfaces
{
    public interface ITransactionService
    {
        public TransactionResult AddTransaction(AddTransactionDto transactionDto);
    }
}