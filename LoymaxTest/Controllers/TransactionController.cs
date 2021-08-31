using AutoMapper;
using LoymaxTest.Helpers;
using LoymaxTest.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Services.Transactions.Interfaces;
using Services.Transactions.Models;
using System.Threading.Tasks;

namespace LoymaxTest.Controllers
{
    [ApiController]
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/[controller]")]
    public class TransactionController : ControllerBase
    {
        private readonly ITransactionService _transactionService;
        private readonly ILogger<TransactionController> _logger;
        private readonly IMapper _mapper;

        public TransactionController(ILogger<TransactionController> logger,
            ITransactionService transactionService,
            IMapper mapper)
        {
            _logger = logger;
            _transactionService = transactionService;
            _mapper = mapper;
        }

        [HttpPost]
        [Route("Deposit")]
        public async Task<AddTransactionResult> Deposit(DepositTransactionModel deposit)
        {
            if (!ModelState.IsValid)
            {
                var errorsText = ModelState.JoinErrors();
                _logger.LogError($"Error occurred on posting Deposit. {errorsText}");
                return AddTransactionResult.FailedResult(errorsText);
            }

            var newTransactionDto = _mapper.Map<AddTransactionDto>(deposit);

            var validationResult = await _transactionService.SaveTransaction(newTransactionDto);
            if (!validationResult.Succeeded)
                _logger.LogError($"Error occurred on posting Deposit. {validationResult.Errors}");

            return validationResult;
        }

        [HttpPost]
        [Route("Withdrawal")]
        public async Task<AddTransactionResult> Withdrawal(WithdrawalTransactionModel withdrawal)
        {
            if (!ModelState.IsValid)
            {
                var errorsText = ModelState.JoinErrors();
                _logger.LogError($"Error occurred on posting Withdrawal. {errorsText}");
                return AddTransactionResult.FailedResult(errorsText);
            }

            var newTransactionDto = _mapper.Map<AddTransactionDto>(withdrawal);

            var validationResult = await _transactionService.SaveTransaction(newTransactionDto);
            if (!validationResult.Succeeded)
                _logger.LogError($"Error occurred on posting Withdrawal. {validationResult.Errors}");

            return validationResult;
        }
    }
}
