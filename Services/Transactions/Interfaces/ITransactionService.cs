using Services.Transactions.Models;
using System.Threading.Tasks;

namespace Services.Transactions.Interfaces
{
    public interface ITransactionService
    {
        public Task<AddTransactionResult> SaveTransaction(AddTransactionDto transactionDto);
    }
}