using Data.Enums;
using Data.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Services.Transactions;
using Services.Transactions.Models;
using System;

namespace LoymaxTest.Controllers
{
    [ApiController]
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/[controller]")]
    public class TransactionController : ControllerBase
    {
        private readonly ITransactionService _transactionService;
        private readonly ILogger<TransactionController> _logger;
        private readonly ITransactionValidator _transactionValidator; // ??? here or in ITransactionService ???

        public TransactionController(ILogger<TransactionController> logger,
            ITransactionService transactionService,
            ITransactionValidator transactionValidator)
        {
            _logger = logger;
            _transactionService = transactionService;
            _transactionValidator = transactionValidator;
        }

        // ToDo automapper
        [HttpPost]
        [Route("Deposit")]
        public TransactionResult Deposit(DepositTransactionModel deposit)
        {
            if (!ModelState.IsValid)
            {
                var errorsText = ModelState.JoinErrors();
                _logger.LogError(errorsText);
                return TransactionResult.CreateFailedResult(errorsText);
            }

            var addTransactionDto = new AddTransactionDto(
                accountId: deposit.AccountId,
                amount: deposit.Deposit,
                type: TransactionType.Deposit);

            var validationResult = _transactionService.AddTransaction(addTransactionDto);

            return validationResult;
        }

        [HttpPost]
        [Route("Withdrawal")]
        public TransactionResult Withdrawal(WithdrawalTransactionModel withdrawal)
        {
            if (!ModelState.IsValid)
            {
                var errorsText = ModelState.JoinErrors();
                _logger.LogError(errorsText);
                throw new ArgumentException(errorsText, nameof(withdrawal));
            }

            var addTransactionDto = new AddTransactionDto(
               accountId: withdrawal.AccountId,
               amount: withdrawal.Withdrawal,
               type: TransactionType.Withdrawal);

            var validationResult = _transactionService.AddTransaction(addTransactionDto);
            if (!validationResult.Succeded)
                _logger.LogError(validationResult.Errors);

            return validationResult;
        }
    }
}
