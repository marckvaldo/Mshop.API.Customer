using Elastic.CommonSchema;
using Elastic.Serilog.Sinks;
using Elastic.Transport;
using Serilog;

namespace MShop.API.Customer.Configuration
{
    public static class ConfigurationLog
    {
        public static WebApplicationBuilder AddConfigurationLogs(this WebApplicationBuilder builder, IConfiguration configuration)
        {
            builder.Host.UseSerilog((context, services, loggerConfig) => 
            {
                loggerConfig
                     .MinimumLevel.Information()
                     .Enrich.FromLogContext()
                     .Enrich.WithEnvironmentName()
                     .Enrich.WithMachineName()
                     .Enrich.WithProperty("Service", "Catalog.API")
                     .WriteTo.Console()
                     .WriteTo.Elasticsearch([new Uri("http://elasticsearch:9200")], Opt => {

                         Opt.DataStream = new Elastic.Ingest.Elasticsearch.DataStreams.DataStreamName(
                             type: "logs", 
                             dataSet: "aplication",
                             @namespace: context.HostingEnvironment.EnvironmentName.ToLower());

                     }, transport =>
                     {
                         transport.Authentication(new BasicAuthentication("", ""));
                     });
                     
            });


            return builder;
        }
    }
}
