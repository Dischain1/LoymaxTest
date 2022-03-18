using AutoMapper;
using LoymaxTest.Helpers;
using LoymaxTest.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Services.Accounts.Interfaces;
using Services.Accounts.Models;
using Services.Common;
using System;
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
        public async Task<AddAccountResult> Account(AddAccountModel model)
        {
            if (!ModelState.IsValid)
            {
                var error = $"Error occurred on registering new account. {ModelState.JoinErrors()}";
                _logger.LogError(error);
                return AddAccountResult.FailedResult(error);
            }

            var addAccountDto = _mapper.Map<AddAccountDto>(model);

            var validationResult = _accountValidator.Validate(addAccountDto);
            if (!validationResult.Valid)
            {
                var error = $"Error occurred on registering new account. {validationResult.Errors}";
                _logger.LogError(error);
                return AddAccountResult.FailedResult(error);
            }

            try
            {
                var newAccountId = await _accountService.AddAccount(addAccountDto);
                return AddAccountResult.SucceededResult(newAccountId);
            }
            catch (Exception exception)
            {
                _logger.LogError(exception.Message);
                return AddAccountResult.FailedResult(ErrorMessages.UnexpectedErrorOccurred);
            }
        }

        [HttpGet]
        [Route("balance/{accountId:int}")]
        public async Task<GetBalanceResult> Balance(int accountId)
        {
            var accountExist = await _accountService.AccountExist(accountId);
            if (!accountExist)
            {
                var error = $"Error occurred on getting Balance. {ErrorMessages.AccountDoesNotExist(accountId)}";
                _logger.LogError(error);
                return GetBalanceResult.FailedResult(error);
            }

            try
            {
                var balance = await _accountService.CalculateBalance(accountId);
                return GetBalanceResult.SucceededResult(balance);
            }
            catch (Exception exception)
            {
                _logger.LogError(exception.Message);
                return GetBalanceResult.FailedResult(ErrorMessages.UnexpectedErrorOccurred);
            }
        }
    }
}
