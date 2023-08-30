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
using Microsoft.AspNetCore.Server.Kestrel.Core;
using App.Metrics.AspNetCore;
using Microsoft.AspNetCore.Mvc;
using App.Metrics.Formatters.Prometheus;
using Prometheus;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddCors(opt => opt.AddDefaultPolicy(policy => policy.AllowAnyHeader().AllowAnyMethod().AllowAnyOrigin()));

builder.Services.AddApplicationServices();
builder.Services.AddPersistanceServices(builder.Configuration);

#region Consul
builder.Services.AddSingleton<IConsulClient>(consul => new ConsulClient(consulConfig =>
{
    consulConfig.Address = new Uri(builder.Configuration["Consul:Host"]);
}, null, handlerOverride =>
{
    handlerOverride.Proxy = null;
    handlerOverride.UseProxy = false;
}));
builder.Services.Configure<WorkoutConfiguration>(builder.Configuration.GetSection("Workout"));
builder.Services.AddSingleton<IHostedService, ConsulRegisterService>();
#endregion

#region OpenTelemetry
builder.Services.AddOpenTracing();
builder.Services.AddSingleton<ITracer>(sp =>
{
    var serviceName = sp.GetRequiredService<IWebHostEnvironment>().ApplicationName;
    var loggerFactory = sp.GetRequiredService<ILoggerFactory>();
    var reporter = new RemoteReporter.Builder().WithLoggerFactory(loggerFactory).WithSender(new UdpSender())
        .Build();
    var tracer = new Tracer.Builder(serviceName)
        .WithSampler(new ConstSampler(true))
        .WithReporter(reporter)
        .Build();
    return tracer;
});

builder.Services.Configure<HttpHandlerDiagnosticOptions>(options =>
    options.OperationNameResolver =
        request => $"{request.Method.Method}: {request?.RequestUri?.AbsoluteUri}");
#endregion

#region Appmetrics - Prometheus - Grafana
builder.Services.Configure<KestrelServerOptions>(options =>
{
    options.AllowSynchronousIO = true;
});

builder.Services.Configure<IISServerOptions>(options =>
{
    options.AllowSynchronousIO = true;
});

builder.Services.AddMetrics();

builder.Host.UseMetricsWebTracking()
                .UseMetrics(options =>
                {
                    options.EndpointOptions = endpointsOptions =>
                    {
                        endpointsOptions.MetricsTextEndpointOutputFormatter = new MetricsPrometheusTextOutputFormatter();
                        endpointsOptions.MetricsEndpointOutputFormatter = new MetricsPrometheusProtobufOutputFormatter();
                        endpointsOptions.EnvironmentInfoEndpointEnabled = false;
                    };
                });

builder.Services.AddMvcCore().AddMetricsCore();
#endregion

builder.Services.AddHttpClient();

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

#region AppMetrics - Prometheus - Grafana
app.UseRouting();
app.UseHttpMetrics();
app.MapMetrics();
#endregion

app.UseCors();
app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();
app.Run();