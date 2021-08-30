using Services.Transactions.Models;

namespace Services.Transactions
{
    public interface ITransactionService
    {
        public TransactionResult AddTransaction(AddTransactionDto transactionDto);
    }
}