using OpenTelemetry.Logs;
using OpenTelemetry.Metrics;
using OpenTelemetry.Resources;
using OpenTelemetry.Trace;
using Serilog;
using Serilog.Events;

namespace MShop.API.Customer.Configuration
{
    public static class ConfigurationLog
    {
        public static WebApplicationBuilder AddConfigurationLogs(this WebApplicationBuilder builder, IConfiguration configuration)
        {
            var url = configuration["OpenTelemetry:Otlp:Endpoint"];
            var ServiceName = configuration["OpenTelemetry:ServiceName"];
            var ServiceVersion = configuration["OpenTelemetry:ServiceVersion"];

            builder.Host.UseSerilog((context, services, loggerConfig) => 
            {
                loggerConfig
                     .MinimumLevel.Information()
                     .MinimumLevel.Override("Microsoft", LogEventLevel.Warning)
                     .MinimumLevel.Override("Microsoft.AspNetCore", LogEventLevel.Warning)
                     .Enrich.FromLogContext()
                     .Enrich.WithEnvironmentName()
                     .Enrich.WithMachineName()
                     .Enrich.WithProperty("Service", ServiceName)
                     .WriteTo.Console()
                     .WriteTo.OpenTelemetry(opt =>
                     {
                         opt.Endpoint = url;
                         opt.Protocol = Serilog.Sinks.OpenTelemetry.OtlpProtocol.Grpc;
                     });
                     
            });

            builder.Logging.ClearProviders();
            builder.Logging.AddOpenTelemetry(opt => { 

                opt.IncludeScopes = true;
                opt.IncludeFormattedMessage = true;
                opt.ParseStateValues = true;

                opt.AddOtlpExporter(opt => {
                    opt.Endpoint = new Uri(url);
                });
            });


            builder.Services.AddOpenTelemetry()
            .ConfigureResource(resource =>
            {
                resource.AddService(
                    serviceName: ServiceName,
                    serviceVersion: ServiceName);
            })
            .WithTracing(tracing =>
            {
                tracing
                    .AddAspNetCoreInstrumentation(o =>
                    {
                        o.RecordException = true;
                    })
                    .AddHttpClientInstrumentation()
                    .AddOtlpExporter(otlp =>
                    {
                        otlp.Endpoint = new Uri(url);
                    });

            })
            .WithMetrics(metrics =>
            {
                metrics
                    .AddAspNetCoreInstrumentation()
                    .AddHttpClientInstrumentation()
                    .AddRuntimeInstrumentation()
                    .AddOtlpExporter(otlp =>
                    {
                        otlp.Endpoint = new Uri(url);
                    });
            });

            return builder;
        }
    }
}
