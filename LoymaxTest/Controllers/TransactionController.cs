using Data.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Services.Transactions;
using System;
using System.Linq;

namespace LoymaxTest.Controllers
{
    [ApiController]
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/[controller]")]
    public class TransactionController : ControllerBase
    {
        private readonly ITransactionService _transactionService;
        private readonly ILogger<TransactionController> _logger;

        public TransactionController(ILogger<TransactionController> logger,
            ITransactionService transactionService)
        {
            _logger = logger;
            _transactionService = transactionService;
        }

        [HttpPost]
        [Route("Deposit")]
        public void Deposit(TransactionBaseModel model)
        {
            if (!ModelState.IsValid)
            {
                var errorsText = ModelState.JoinErrors();
                _logger.LogError(errorsText);
                throw new ArgumentException(errorsText, nameof(model));
            }

            // ToDo model & validation & logging
            _transactionService.Deposit();
        }

        [HttpPost]
        [Route("Withdrawal")]
        public void Withdrawal(TransactionBaseModel model)
        {
            if (!ModelState.IsValid)
            {
                var errorsText = ModelState.JoinErrors();
                _logger.LogError(errorsText);
                throw new ArgumentException(errorsText, nameof(model));
            }

            // ToDo model & validation & logging
            _transactionService.Withdraw();
        }
    }
}
