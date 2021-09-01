using Services.Common;
using Services.Transactions.Models;
using System.Threading.Tasks;

namespace Services.Transactions.Interfaces
{
    public interface ITransactionValidator
    {
        public Task<ValidationResult> Validate(AddTransactionDto transactionAddDto);
    }
}