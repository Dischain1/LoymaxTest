using System.Threading.Tasks;
using Data.Enums;
using Services.Accounts.Interfaces;
using Services.Common;
using Services.Transactions.Interfaces;
using Services.Transactions.Models;

namespace Services.Transactions
{
    public class TransactionValidator : ITransactionValidator
    {
        private readonly IAccountService _accountService;

        public TransactionValidator(IAccountService accountService)
        {
            _accountService = accountService;
        }

        public async Task<ValidationResult> Validate(AddTransactionDto transactionAddDto, bool isUsedInsideTransaction)
        {
            var accountExist = await _accountService.AccountExist(transactionAddDto.AccountId);
            if (!accountExist)
                return ValidationResult.NotValidResult($"Error on validating transaction. {ErrorMessages.AccountDoesNotExist(transactionAddDto.AccountId)}");

            if (transactionAddDto.Amount <= 0)
            {
                var negativeAmountError = $"Provided amount to deposit or withdraw should be positive. Provided value is {transactionAddDto.Amount}";
                return ValidationResult.NotValidResult(negativeAmountError);
            }

            // ToDo check decimal scale of 2
            // IF dto.Amount == 10.111111 => It is NOT valid


            switch (transactionAddDto.Type)
            {
                case TransactionType.Deposit:
                    if (transactionAddDto.Amount > CommonConstants.DepositLimit)
                    {
                        var depositLimitExceeded = $"Deposit limit exceeded. Deposit: {transactionAddDto.Amount}, Deposit limit: {CommonConstants.DepositLimit}.";
                        return ValidationResult.NotValidResult(depositLimitExceeded);
                    }
                    break;

                case TransactionType.Withdrawal:
                    if (transactionAddDto.Amount > CommonConstants.WithdrawalLimit)
                    {
                        var depositLimitExceeded = $"Withdrawal limit exceeded. Withdrawal: {transactionAddDto.Amount}, Withdrawal limit: {CommonConstants.WithdrawalLimit}.";
                        return ValidationResult.NotValidResult(depositLimitExceeded);
                    }
                    var currentBalance = await _accountService.CalculateBalance(transactionAddDto.AccountId, isUsedInsideTransaction: isUsedInsideTransaction);
                    var insufficientFunds = currentBalance - transactionAddDto.Amount < 0;
                    if (insufficientFunds)
                    {
                        var insufficientFundsError = $"Insufficient funds to withdraw. Balance: {currentBalance}. Withdrawal: {transactionAddDto.Amount}, Account Id:{transactionAddDto.AccountId}.";
                        return ValidationResult.NotValidResult(insufficientFundsError);
                    }
                    break;

                default:
                    var transactionTypeError = $"Unknown transaction type used. Transaction type: {transactionAddDto.Type}, Account Id:{transactionAddDto.AccountId}";
                    return ValidationResult.NotValidResult(transactionTypeError);
            }

            return ValidationResult.ValidResult();
        }
    }
}
