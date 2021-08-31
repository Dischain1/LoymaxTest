using System.Threading.Tasks;
using Services.Common;
using Services.Transactions.Models;

namespace Services.Transactions.Interfaces
{
    public interface ITransactionValidator
    {
        public Task<ValidationResult> Validate(AddTransactionDto transactionAddDto, bool isUsedInsideTransaction);
    }
}