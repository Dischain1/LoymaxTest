using System.Threading.Tasks;
using Services.Transactions.Models;

namespace Services.Transactions.Interfaces
{
    public interface ITransactionService
    {
        public Task<AddTransactionResult> SaveTransaction(AddTransactionDto transactionDto);
    }
}