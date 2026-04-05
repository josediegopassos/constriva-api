using FluentValidation;
using MassTransit;
using MediatR;
using Microsoft.AspNetCore.RateLimiting;
using Microsoft.OpenApi.Models;
using MongoDB.Driver;
using Serilog;
using Constriva.Application.Common.Behaviors;
using Constriva.Application.Features.Lens.Interfaces;
using Constriva.API.Consumers;
using Constriva.API.Hubs;
using Constriva.API.Services;
using Constriva.Infrastructure.DependencyInjection;
using Constriva.Messaging.Contracts.Lens.Events;
using System.Globalization;
using System.Reflection;
using System.Text.Json.Serialization;
using System.Threading.RateLimiting;

var ptBr = new CultureInfo("pt-BR");
CultureInfo.DefaultThreadCurrentCulture   = ptBr;
CultureInfo.DefaultThreadCurrentUICulture = ptBr;

var builder = WebApplication.CreateBuilder(args);

Log.Logger = new LoggerConfiguration()
    .ReadFrom.Configuration(builder.Configuration)
    .Enrich.FromLogContext()
    .WriteTo.Console()
    .WriteTo.File("logs/obras-.log", rollingInterval: RollingInterval.Day)
    .CreateLogger();

builder.Host.UseSerilog();
builder.Services.AddInfrastructure(builder.Configuration);

builder.Services.AddMediatR(cfg =>
{
    cfg.RegisterServicesFromAssembly(Assembly.Load("Constriva.Application"));
    cfg.AddBehavior(typeof(IPipelineBehavior<,>), typeof(LoggingBehavior<,>));
    cfg.AddBehavior(typeof(IPipelineBehavior<,>), typeof(ValidationBehavior<,>));
});

builder.Services.AddValidatorsFromAssembly(Assembly.Load("Constriva.Application"));
builder.Services.AddAutoMapper(Assembly.Load("Constriva.Application"));
builder.Services.AddControllers()
    .AddJsonOptions(opt =>
    {
        opt.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
        opt.JsonSerializerOptions.DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull;
    });

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", p => p
        .SetIsOriginAllowed(_ => true)
        .AllowAnyMethod()
        .AllowAnyHeader()
        .AllowCredentials());
    options.AddPolicy("Production", p =>
        p.WithOrigins(builder.Configuration.GetSection("AllowedOrigins").Get<string[]>() ?? Array.Empty<string>())
         .AllowAnyMethod().AllowAnyHeader().AllowCredentials());
});

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "Constriva API", Version = "v1", Description = "API multi-tenant para gestão de obras e construção civil" });
    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Description = "JWT Authorization. Formato: Bearer {token}",
        Name = "Authorization", In = ParameterLocation.Header,
        Type = SecuritySchemeType.ApiKey, Scheme = "Bearer"
    });
    c.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        { new OpenApiSecurityScheme { Reference = new OpenApiReference { Type = ReferenceType.SecurityScheme, Id = "Bearer" } }, Array.Empty<string>() }
    });
    c.CustomSchemaIds(type => type.FullName?.Replace("+", ".") ?? type.Name);
    c.ResolveConflictingActions(apiDescriptions => apiDescriptions.First());
});

builder.Services.AddSignalR();

// ── Lens: Serviços ───────────────────────────────────────────────────────
builder.Services.AddScoped<ILensFileStorageService, LensFileStorageService>();
builder.Services.AddScoped<ILensNotificationService, LensNotificationService>();

// ── Lens: MassTransit + RabbitMQ ─────────────────────────────────────────
var rabbitHost = builder.Configuration["RabbitMq:Host"] ?? "localhost";
var rabbitPort = ushort.Parse(builder.Configuration["RabbitMq:Porta"] ?? "5672");
var rabbitUser = builder.Configuration["RabbitMq:Usuario"] ?? "guest";
var rabbitPass = builder.Configuration["RabbitMq:Senha"] ?? "guest";
var rabbitVHost = builder.Configuration["RabbitMq:VirtualHost"] ?? "/";

builder.Services.AddMassTransit(cfg =>
{
    cfg.AddConsumer<DocumentoLensConcluidoConsumer>();
    cfg.AddConsumer<DocumentoLensErroConsumer>();

    cfg.UsingRabbitMq((context, rabbitCfg) =>
    {
        rabbitCfg.Host(rabbitHost, rabbitPort, rabbitVHost, h =>
        {
            h.Username(rabbitUser);
            h.Password(rabbitPass);
        });

        rabbitCfg.ReceiveEndpoint("constriva-api-lens-eventos", e =>
        {
            e.ConfigureConsumer<DocumentoLensConcluidoConsumer>(context);
            e.ConfigureConsumer<DocumentoLensErroConsumer>(context);
        });
    });
});

// ── Lens: MongoDB (leitura para analytics) ───────────────────────────────
var mongoConnStr = builder.Configuration["MongoDb:ConnectionString"];
if (!string.IsNullOrEmpty(mongoConnStr))
{
    var mongoDbName = builder.Configuration["MongoDb:NomeBanco"] ?? "constriva_lens_logs";
    builder.Services.AddSingleton<IMongoClient>(new MongoClient(mongoConnStr));
    builder.Services.AddSingleton(sp => sp.GetRequiredService<IMongoClient>().GetDatabase(mongoDbName));
}

// ── MediatR: registrar handlers do projeto API (Lens consumers usam MediatR)
builder.Services.AddMediatR(cfg =>
{
    cfg.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly());
});

builder.Services.AddRateLimiter(opt =>
{
    opt.AddFixedWindowLimiter("api", o => { o.PermitLimit = 200; o.Window = TimeSpan.FromMinutes(1); });
    opt.AddFixedWindowLimiter("auth", o => { o.PermitLimit = 10; o.Window = TimeSpan.FromMinutes(15); });
});
builder.Services.AddHealthChecks()
    .AddNpgSql(builder.Configuration.GetConnectionString("DefaultConnection")!);

var app = builder.Build();

app.UseSerilogRequestLogging();
if (app.Environment.IsDevelopment()) { app.UseSwagger(); app.UseSwaggerUI(); }
app.UseRequestLocalization(new RequestLocalizationOptions
{
    DefaultRequestCulture = new Microsoft.AspNetCore.Localization.RequestCulture("pt-BR"),
    SupportedCultures   = [ptBr],
    SupportedUICultures = [ptBr]
});
app.UseHttpsRedirection();
app.UseCors(app.Environment.IsDevelopment() ? "AllowAll" : "Production");
app.UseRateLimiter();
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();
app.MapHealthChecks("/health");
app.MapHub<Constriva.API.Hubs.NotificationHub>("/hubs/notifications");
app.MapHub<LensHub>("/hubs/lens");

// Migrate + Seed
await Constriva.Infrastructure.Persistence.DbSeeder.SeedAsync(app.Services);

app.Run();
