using LoymaxTest.Helpers;
using LoymaxTest.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Services.Accounts;
using Services.Accounts.Interfaces;
using Services.Accounts.Models;
using Services.Common;

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

        private const int AccountCreationFailedId = -1;
        private const decimal BalanceOfNotFoundAccount = decimal.MinValue;

        public AccountController(ILogger<AccountController> logger,
            IAccountService accountService,
            IAccountValidator accountValidator)
        {
            _logger = logger;
            _accountService = accountService;
            _accountValidator = accountValidator;
        }

        [HttpPost]
        public int Account(AddAccountModel model)
        {
            if (!ModelState.IsValid)
            {
                _logger.LogError(ModelState.JoinErrors());
                return AccountCreationFailedId;
            }

            //ToDo automapper maybe?
            var addAccountDto = new AddAccountDto
            {
                FirstName = model.FirstName,
                LastName = model.LastName,
                Patronimic = model.Patronimic,
                DateOfBirth = model.DateOfBirth,
            };
            var validationResult = _accountValidator.Validate(addAccountDto);
            if (!validationResult.Valid)
            {
                _logger.LogError(validationResult.Errors);
                return AccountCreationFailedId;
            }

            return _accountService.AddAccount(addAccountDto);
        }

        [HttpGet]
        [Route("balance/{accountId:int}")]
        public decimal Balance(int accountId)
        {
            if (!_accountService.AccountExist(accountId))
            {
                _logger.LogError($"Error occurred on getting Balance. {ErrorMessages.AccountDoesNotExist(accountId)}");
                return BalanceOfNotFoundAccount;
            }

            return _accountService.CalculateBalance(accountId);
        }
    }
}
