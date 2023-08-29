using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using OpenTracing;

namespace APITest.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TestController : ControllerBase
    {
        private List<string> _list = new List<string>() { "Tracing ", "is ", "started ", "successfully" };
        private readonly ITracer _tracer;
        private readonly IHttpClientFactory _clientFactory;

        public TestController(ITracer tracer, IHttpClientFactory clientFactory)
        {
            _tracer = tracer;
            _clientFactory = clientFactory;
        }

        [HttpGet]
        public List<string> Get()
        {
            var actionName = ControllerContext.ActionDescriptor.DisplayName;
            using var scope = _tracer.BuildSpan(actionName).StartActive(true);
            var client = _clientFactory.CreateClient("logService");
            return _list;

        }

        [HttpGet("/getGet")]
        public string GetGet()
        {
            return "deneme";
        }
    }
}
