using Persistance;
using Application;
using Application.Abstractions.Services;
using Application.Repositories.ExerciseRepositories;
using Application.Repositories.WorkoutRepositories;
using Persistance.Concretes.Repositories.ExerciseRepositories;
using Persistance.Concretes.Repositories.WorkoutRepositories;
using Persistance.Concretes.Services;
using Persistance.Context.MongoDbContext;
using System.Reflection;
using Application.Profiles;
using AutoMapper;
using Consul;
using API.Extensions;
using API.Models.Consul;
using API.Models;
using System.Diagnostics;
using OpenTracing;
using Jaeger.Reporters;
using Jaeger;
using Jaeger.Samplers;
using OpenTracing.Contrib.NetCore.Configuration;
using Jaeger.Senders.Thrift;

var builder = WebApplication.CreateBuilder(args);


builder.Services.AddApplicationServices();
builder.Services.AddPersistanceServices(builder.Configuration);

builder.Services.AddSingleton<IConsulClient>(consul => new ConsulClient(consulConfig => ////////////
{
    consulConfig.Address = new Uri(builder.Configuration["Consul:Host"]);
}, null, handlerOverride =>
{
    //disable proxy of httpclienthandler  
    handlerOverride.Proxy = null;
    handlerOverride.UseProxy = false;
}));


#region OpenTelemetry

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
#endregion

builder.Services.AddHttpClient();

builder.Services.Configure<MenuConfiguration>(builder.Configuration.GetSection("Menu")); ///////////////
builder.Services.AddSingleton<IHostedService, ConsulRegisterService>(); /////////////////////

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();
app.Run();