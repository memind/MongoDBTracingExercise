using APITest.Consul.Configs;
using APITest.Consul.Services;
using App.Metrics.AspNetCore;
using App.Metrics.Formatters.Prometheus;
using Consul;
using Hangfire;
using Hangfire.SqlServer;
using Jaeger;
using Jaeger.Reporters;
using Jaeger.Samplers;
using Jaeger.Senders.Thrift;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Server.Kestrel.Core;
using Microsoft.Extensions.DependencyInjection;
using OpenTracing;
using OpenTracing.Contrib.NetCore.Configuration;
using Prometheus;
using System.Diagnostics;
using System.Net.Sockets;
using System.Reflection;
using System.Runtime.Intrinsics.Arm;
using System.Xml.Linq;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddCors(opt => opt.AddDefaultPolicy(policy => policy.AllowAnyHeader().AllowAnyMethod().AllowAnyOrigin()));

#region Services
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

        builder.Services.AddHttpClient();
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

    #region HangFire
        builder.Services.AddHangfire(configuration => configuration
            .SetDataCompatibilityLevel(CompatibilityLevel.Version_180)
            .UseSimpleAssemblyNameTypeSerializer()
            .UseRecommendedSerializerSettings()
            .UseSqlServerStorage(builder.Configuration["Hangfire:MsSqlConnectionString"]));
        builder.Services.AddHangfireServer();
        builder.Services.AddMvc();
    #endregion
#endregion

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

#region Services
    #region Hangfire
        app.UseHangfireDashboard();
        app.MapHangfireDashboard();
    #endregion
    #region AppMetrics - Prometheus - Grafana
        app.UseRouting();
        app.UseHttpMetrics();
        app.MapMetrics();
    #endregion
#endregion

app.UseHttpsRedirection();

app.UseAuthorization();
app.UseCors();
app.MapControllers();

app.Run();