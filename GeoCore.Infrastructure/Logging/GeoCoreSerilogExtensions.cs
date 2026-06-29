using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Serilog;
using Serilog.Events;

namespace GeoCore.Infrastructure.Logging;

public static class GeoCoreSerilogExtensions
{
    public static IHostBuilder UseGeoCoreSerilog(this IHostBuilder hostBuilder)
    {
        return hostBuilder.UseSerilog((context, _, loggerConfiguration) =>
        {
            Configure(loggerConfiguration, context.Configuration);
        });
    }

    public static LoggerConfiguration ConfigureGeoCoreSerilog(this LoggerConfiguration loggerConfiguration, IConfiguration configuration)
    {
        return Configure(loggerConfiguration, configuration);
    }

    private static LoggerConfiguration Configure(LoggerConfiguration loggerConfiguration, IConfiguration configuration)
    {
        var section = configuration.GetSection("GeoCore:Logging");
        var minimumLevel = Enum.TryParse<LogEventLevel>(section["MinimumLevel"], true, out var level)
            ? level
            : LogEventLevel.Information;

        loggerConfiguration
            .MinimumLevel.Is(minimumLevel)
            .Enrich.FromLogContext();

        if (section.GetValue("Console:Enabled", true))
        {
            loggerConfiguration.WriteTo.Console();
        }

        var filePath = section["File:Path"];
        if (!string.IsNullOrWhiteSpace(filePath))
        {
            loggerConfiguration.WriteTo.File(
                filePath,
                rollingInterval: RollingInterval.Day,
                retainedFileCountLimit: section.GetValue<int?>("File:RetainedFileCountLimit"));
        }

        return loggerConfiguration;
    }
}
