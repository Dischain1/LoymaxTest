using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;

namespace LoymaxTest.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class AccountController : ControllerBase
    {
        private readonly ILogger<AccountController> _logger;

        public AccountController(ILogger<AccountController> logger)
        {
            _logger = logger;
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
