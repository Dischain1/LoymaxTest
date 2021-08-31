using AutoMapper;
using LoymaxTest.Helpers;
using LoymaxTest.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Services.Accounts.Interfaces;
using Services.Accounts.Models;
using Services.Common;
using System.Threading.Tasks;

namespace LoymaxTest.Controllers
{
    [ApiController]
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/[controller]")]
    public class AccountController : ControllerBase
    {
        private readonly ILogger<AccountController> _logger;
        private readonly IAccountService _accountService;
        private readonly IAccountValidator _accountValidator;
        private readonly IMapper _mapper;

        private const int AccountCreationFailedId = -1;
        private const decimal BalanceOfNotFoundAccount = decimal.MinValue;

        public AccountController(ILogger<AccountController> logger,
            IAccountService accountService,
            IAccountValidator accountValidator,
            IMapper mapper)
        {
            _logger = logger;
            _accountService = accountService;
            _accountValidator = accountValidator;
            _mapper = mapper;
        }

        [HttpPost]
        public async Task<int> Account(AddAccountModel model)
        {
            if (!ModelState.IsValid)
            {
                _logger.LogError($"Error occurred on registering new account. {ModelState.JoinErrors()}");
                return AccountCreationFailedId;
            }

            var addAccountDto = _mapper.Map<AddAccountDto>(model);

            var validationResult = _accountValidator.Validate(addAccountDto);
            if (!validationResult.Valid)
            {
                _logger.LogError($"Error occurred on registering new account. {validationResult.Errors}");
                return AccountCreationFailedId;
            }

            return await _accountService.AddAccount(addAccountDto);
        }

        [HttpGet]
        [Route("balance/{accountId:int}")]
        public async Task<decimal> Balance(int accountId)
        {
            var accountExist = await _accountService.AccountExist(accountId);
            if (!accountExist)
            {
                _logger.LogError($"Error occurred on getting Balance. {ErrorMessages.AccountDoesNotExist(accountId)}");
                return BalanceOfNotFoundAccount;
            }

            return await _accountService.CalculateBalance(accountId);
        }
    }
}
