using Serilog;
using Constriva.Messaging.Extensions;

Log.Logger = new LoggerConfiguration()
    .WriteTo.Console()
    .CreateBootstrapLogger();

try
{
    Log.Information("Iniciando Constriva.Messaging Worker Service...");

    var builder = Host.CreateApplicationBuilder(args);

    builder.Services.AddSerilog((services, cfg) => cfg
        .ReadFrom.Configuration(builder.Configuration)
        .ReadFrom.Services(services)
        .Enrich.FromLogContext()
        .WriteTo.Console()
        .WriteTo.File("logs/messaging-.log", rollingInterval: RollingInterval.Day));

    builder.Services.AdicionarConstrivaMensageria(builder.Configuration);

    var host = builder.Build();

    Log.Information("Constriva.Messaging Worker Service iniciado com sucesso.");

    await host.RunAsync();
}
catch (Exception ex)
{
    Log.Fatal(ex, "Constriva.Messaging Worker Service encerrado com erro fatal.");
}
finally
{
    Log.CloseAndFlush();
}
