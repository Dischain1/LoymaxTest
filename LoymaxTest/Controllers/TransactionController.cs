using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Services.Transactions;
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

        public TransactionController(ILogger<TransactionController> logger,
            ITransactionService transactionService)
        {
            _logger = logger;
            _transactionService = transactionService;
        }

        [HttpPost]
        [Route("Deposit")]
        public decimal Deposit()
        {
            throw new NotImplementedException();
        }

        [HttpPost]
        [Route("Withdrawal")]
        public decimal Withdrawal()
        {
            throw new NotImplementedException();
        }
    }
}
