using Data;
using Services.Accounts;
using Services.Accounts.Interfaces;
using Services.Common;
using Services.Transactions.Interfaces;
using Services.Transactions.Models;

namespace Services.Transactions
{
    public class TransactionValidator : ITransactionValidator
    {
        private readonly IAccountService _accountService;
        private readonly LoymaxTestContext _context;

        public TransactionValidator(LoymaxTestContext context,
            IAccountService accountService)
        {
            _accountService = accountService;
            _context = context;
        }

        public ValidationResult Validate(AddTransactionDto dto, bool isUsedInsideTransaction)
        {
            // Common logic
            var accountExist = _accountService.AccountExist(dto.AccountId);
            if (!accountExist)
                return ValidationResult.NotValidResult($"Error on validating transaction. {ErrorMessages.AccountDoesNotExist(dto.AccountId)}");

            if (dto.Amount <= 0)
            {
                var negativeAmountError = $"Provided amount to deposit or withdraw should be positive. Provided value is {dto.Amount}";
                return ValidationResult.NotValidResult(negativeAmountError);
            }

            // ToDo check decimal scale of 2
            // IF dto.Amount == 10.111111 => It is NOT valid

            switch (dto.Type)
            {
                case Data.Enums.TransactionType.Deposit:
                    // Nothing more to check.
                    break;

                case Data.Enums.TransactionType.Withdrawal:
                    var balance = _accountService.CalculateBalance(dto.AccountId, isUsedInsideTransaction: isUsedInsideTransaction);
                    var insufficientFunds = balance - dto.Amount < 0;
                    if (insufficientFunds)
                    {
                        var insufficientFundsError = $"Insufficient funds to withdraw. Balance: {balance}. Withdrawal: {dto.Amount}, Account Id:{dto.AccountId}.";
                        return ValidationResult.NotValidResult(insufficientFundsError);
                    }
                    break;

                default:
                    var transactionTypeError = $"Unknown transaction type used. Transaction type: {dto.Type}, Account Id:{dto.AccountId}";
                    return ValidationResult.NotValidResult(transactionTypeError);
            }

            return ValidationResult.ValidResult();
        }
    }
}
