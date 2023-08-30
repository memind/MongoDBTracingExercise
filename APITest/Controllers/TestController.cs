using Hangfire;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using OpenTracing;
using System;
using System.Reflection.Metadata.Ecma335;

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
            var id = BackgroundJob.Enqueue(() => Console.WriteLine("Background Job! Triggered when clicked."));
            BackgroundJob.ContinueJobWith(id, () => Console.WriteLine("Continuations Job! Triggered after background job!"));
            RecurringJob.AddOrUpdate(() => Console.WriteLine("Recurring Job! Triggered minutely."), Cron.Minutely());
            BackgroundJob.Schedule(() => Console.WriteLine("Scheduled/Delayed Job! Triggered after 5 seconds from click!"), TimeSpan.FromSeconds(5));

            return "deneme";
        }
    }
}
