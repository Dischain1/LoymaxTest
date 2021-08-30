using Data;
using Services.Accounts;
using Services.Common;
using Services.Transactions.Models;
using System;

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

        public ValidationReslut ValidateTransaction(AddTransactionDto dto)
        {
            // Common logic
            var accountExist = _accountService.AccountExist(dto.AccountId);
            if (!accountExist)
                return ValidationReslut.NotValidResult($"Error on validating transaction. {ErrorMessages.AccountDoesNotExist(dto.AccountId)}");

            if (dto.Amount <= 0)
            {
                var negativeAmountError = $"Provided amount to deposit or withdraw should be positive.";
                return ValidationReslut.NotValidResult(negativeAmountError);
            }

            // ToDo check decimal scale of 2
            // IF dto.Amount == 10.111111 => is NOT valid

            switch (dto.Type)
            {
                case Data.Enums.TransactionType.Deposit:
                    // Nothing more to check.
                    break;

                case Data.Enums.TransactionType.Withdrawal:
                    var balance = _accountService.GetBalance(dto.AccountId);
                    var isBalanceEnoughToWithdraw = (balance - dto.Amount) >= 0;
                    if (isBalanceEnoughToWithdraw)
                    {
                        var insufficientFundsError = $"Insufficient funds to perform withdraw. Balance: {balance}. Withdrawal: {dto.Amount}, Account Id:{dto.AccountId}.";
                        return ValidationReslut.NotValidResult(insufficientFundsError);
                    }
                    break;

                default:
                    var transactionTypeError = $"Unknown transaction type used. Transaction type: {dto.Type}, Account Id:{dto.AccountId}";
                    return ValidationReslut.NotValidResult(transactionTypeError);
            }

            return ValidationReslut.ValidResult();
        }
    }
}
