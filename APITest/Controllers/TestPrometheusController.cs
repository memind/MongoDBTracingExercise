using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace APITest.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TestPrometheusController : ControllerBase
    {
        [HttpGet]
        public string Get()
        {
            return "deneme";
        }
    }
}
