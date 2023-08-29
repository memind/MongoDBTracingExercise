using APITest.Consul.Configs;
using APITest.Consul.Services;
using Consul;
using Jaeger;
using Jaeger.Reporters;
using Jaeger.Samplers;
using Jaeger.Senders.Thrift;
using Microsoft.Extensions.DependencyInjection;
using OpenTracing;
using OpenTracing.Contrib.NetCore.Configuration;
using OpenTracing.Util;
using System.Diagnostics;
using System.Net.Sockets;
using System.Reflection;
using System.Runtime.Intrinsics.Arm;
using System.Xml.Linq;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();


#region Consul
builder.Services.AddSingleton<IConsulClient>(consul => new ConsulClient(cfg =>
{
    cfg.Address = new Uri(builder.Configuration["Consul:Host"]);
}, null, handlerOverride =>
{
    handlerOverride.Proxy = null;
    handlerOverride.UseProxy = false;
}));

builder.Services.Configure<TestServiceConfiguration>(builder.Configuration.GetSection("TestService"));
builder.Services.Configure<Test2ServiceConfiguration>(builder.Configuration.GetSection("Test2Service"));
builder.Services.AddSingleton<IHostedService, ConsulRegisterServices>();
#endregion



#region OpenTracing

// Docker'da calistir:
// docker run -d -p 6831:6831 udp -p 6832:6832 udp -p 14268:14268 -p 14250:14250 -p 16686:16686 -p 5778:5778 --name jaeger jaegertracing/all-in-one:1.22

builder.Services.AddOpenTracing();
// Adds the Jaeger Tracer.
builder.Services.AddSingleton<ITracer>(sp =>
{
    var serviceName = sp.GetRequiredService<IWebHostEnvironment>().ApplicationName;
    var loggerFactory = sp.GetRequiredService<ILoggerFactory>();
    var reporter = new RemoteReporter.Builder().WithLoggerFactory(loggerFactory).WithSender(new UdpSender())
        .Build();
    var tracer = new Tracer.Builder(serviceName)
        // The constant sampler reports every span.
        .WithSampler(new ConstSampler(true))
        // LoggingReporter prints every reported span to the logging framework.
        .WithReporter(reporter)
        .Build();
    return tracer;
});

builder.Services.Configure<HttpHandlerDiagnosticOptions>(options =>
    options.OperationNameResolver =
        request => $"{request.Method.Method}: {request?.RequestUri?.AbsoluteUri}");

builder.Services.AddHttpClient();
#endregion

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
