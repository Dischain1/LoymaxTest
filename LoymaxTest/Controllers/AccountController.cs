using LoymaxTest.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Services.Accounts;
using Services.Accounts.Models;

namespace LoymaxTest.Controllers
{
    [ApiController]
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/[controller]")]
    public class AccountController : ControllerBase
    {
        private readonly ILogger<AccountController> _logger;
        private readonly IAccountService _accountService;
        private IAccountValidator _accountValidator;

        const int AccountCreationFaildId = -1;
        const decimal BalanceOfNotFoundAccount = decimal.MinValue;

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
                return AccountCreationFaildId;
            }
           
            //ToDo automapper maybe?
            var addAccountDto = new AddAccountDto 
            {
                FirstName = model.FirstName,
                LastName = model.LastName,
                Panronimic = model.Panronimic,
                DateOfBirth = model.DateOfBirth,
            };
            var validationResult = _accountValidator.Validate(addAccountDto);
            if (!validationResult.Valid)
            {
                _logger.LogError(validationResult.Errors);
                return AccountCreationFaildId;
            }
          
            return _accountService.AddAccount(addAccountDto);
        }

        [HttpGet]
        [Route("balance/{accountId:int}")]
        public decimal Balance(int accountId)
        {
            if (!_accountService.AccountExist(accountId))
            {
                _logger.LogError($"Balance of non-existent account is requested. Account Id: {accountId}");
                return BalanceOfNotFoundAccount;
            }

            return _accountService.GetBalance(accountId);
        }
    }
}
