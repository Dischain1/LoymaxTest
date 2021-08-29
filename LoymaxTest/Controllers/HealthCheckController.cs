using Microsoft.AspNetCore.Mvc;

namespace LoymaxTest.Controllers
{
    [Route("[controller]")]
    public class HealthCheckController : Controller
    {
        [HttpGet]
        public JsonResult Get()
        {
            return new JsonResult("Ok");
        }
    }
}
