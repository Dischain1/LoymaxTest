using LoymaxTest.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.Extensions.Logging;
using Services.Accounts;
using Services.Accounts.Models;
using System;
using System.Linq;

namespace LoymaxTest.Controllers
{
    // ToDo logging

    [ApiController]
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/[controller]")]
    public class AccountController : ControllerBase
    {
        private readonly ILogger<AccountController> _logger;
        readonly IAccountService _accountService;

        public AccountController(ILogger<AccountController> logger,
            IAccountService accountService)
        {
            _logger = logger;
            _accountService = accountService;
        }

        [HttpPost]
        public int Account(AddAccountModel model)
        {
            if (!ModelState.IsValid)
            {
                var errorsText = ModelState.JoinErrors();
                _logger.LogError(errorsText);
                throw new ArgumentException(errorsText, nameof(model));
            }

            //ToDo automapper maybe?
            var addAccountDto = new AddAccountDto();

            return _accountService.AddAccount(addAccountDto);
        }

        [HttpGet]
        public decimal Balance(int accountId)
        {


            return _accountService.GetBalance(accountId);
        }
    }
}
