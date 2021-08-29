using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Services.Accounts;
using System;

namespace LoymaxTest.Controllers
{
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
        public int Account()
        {
            throw new NotImplementedException();
        }

        [HttpGet]
        public decimal Balance()
        {
            throw new NotImplementedException();
        }
    }
}
