using Data.Enums;

namespace Services.Transactions.Models
{
    public class AddTransactionDto
    {
        public readonly TransactionType Type;
        public readonly int AccountId;
        public readonly decimal Amount;

        public AddTransactionDto(int accountId, decimal amount, TransactionType type)
        {
            AccountId = accountId;
            Amount = amount;
            Type = type;
        }
    }
}
