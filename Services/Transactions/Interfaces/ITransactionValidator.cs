
using Services.Transactions.Models;

namespace Services.Transactions
{
    public interface ITransactionValidator
    {
        public ValidationReslut ValidateTransaction(AddTransactionDto transactionDto);
    }
}