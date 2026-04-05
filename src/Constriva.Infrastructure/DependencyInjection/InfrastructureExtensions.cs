using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Constriva.Application.Common.Interfaces;
using Constriva.Application.Features.Auth;
using Constriva.Application.Features.Auth.Interfaces;
using Constriva.Domain.Interfaces.Repositories;
using Constriva.Infrastructure.Identity;
using Constriva.Infrastructure.Persistence;
using Constriva.Infrastructure.Persistence.Repositories;
using Constriva.Infrastructure.Persistence.Repositories.Clientes;
using Constriva.Infrastructure.Persistence.Repositories.Orcamento;
using Constriva.Infrastructure.Services;
using System.Text;

namespace Constriva.Infrastructure.DependencyInjection;

public static class InfrastructureServiceExtensions
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        // Npgsql: aceitar DateTime com Kind=Unspecified tratando como UTC
        AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);

        // Npgsql: habilitar serialização dinâmica de JSON (List<string> → jsonb)
        var dataSourceBuilder = new Npgsql.NpgsqlDataSourceBuilder(configuration.GetConnectionString("DefaultConnection"));
        dataSourceBuilder.EnableDynamicJson();
        var dataSource = dataSourceBuilder.Build();

        // DbContext
        services.AddDbContext<AppDbContext>(options =>
            options.UseNpgsql(dataSource,
                npg => npg.MigrationsAssembly(typeof(AppDbContext).Assembly.FullName)));

        // Repositories - Base
        services.AddScoped<IUnitOfWork, UnitOfWork>();
        services.AddScoped(typeof(Domain.Interfaces.Repositories.IRepository<>), typeof(Repository<>));
        services.AddScoped(typeof(Domain.Interfaces.Repositories.ITenantRepository<>), typeof(TenantRepository<>));

        // Repositories - Tenant / Auth
        services.AddScoped<IEmpresaRepository, EmpresaRepository>();
        services.AddScoped<IUsuarioRepository, UsuarioRepository>();

        // Repositories - Clientes
        services.AddScoped<IClienteRepository, ClienteRepository>();

        // Repositories - Obras
        services.AddScoped<IObraRepository, ObraRepository>();

        // Repositories - Estoque
        services.AddScoped<IMaterialRepository, MaterialRepository>();
        services.AddScoped<IEstoqueRepository, EstoqueRepository>();

        // Repositories - Financeiro
        services.AddScoped<ILancamentoFinanceiroRepository, LancamentoFinanceiroRepository>();
        services.AddScoped<ICentroCustoRepository, CentroCustoRepository>();
        services.AddScoped<IContaBancariaRepository, ContaBancariaRepository>();

        // Repositories - Compras
        services.AddScoped<IFornecedorRepository, FornecedorRepository>();
        services.AddScoped<IComprasRepository, ComprasRepository>();

        // Repositories - Cronograma
        services.AddScoped<ICronogramaRepository, CronogramaRepository>();

        // Repositories - SST
        services.AddScoped<ISSTRepository, SSTRepository>();

        // Repositories - GED
        services.AddScoped<IGEDRepository, GEDRepository>();

        // Repositories - Qualidade
        services.AddScoped<IQualidadeRepository, QualidadeRepository>();

        // Repositories - RH
        services.AddScoped<IRHRepository, RHRepository>();

        // Repositories - Contratos
        services.AddScoped<IContratoRepository, ContratoRepository>();
        services.AddScoped<IFaseObraRepository, FaseObraRepository>();

        // Repositories - Orçamento
        services.AddScoped<IOrcamentoRepository, OrcamentoRepository>();
        services.AddScoped<IGrupoOrcamentoRepository, GrupoOrcamentoRepository>();
        services.AddScoped<IItemOrcamentoRepository, ItemOrcamentoRepository>();
        services.AddScoped<IComposicaoOrcamentoRepository, ComposicaoOrcamentoRepository>();

        // Repositories - Relatórios
        services.AddScoped<IRelatoriosRepository, RelatoriosRepository>();

        // Repositories - Lens
        services.AddScoped<IDocumentoLensRepository, DocumentoLensRepository>();

        // Repositories - Agente
        services.AddScoped<IAgenteRepository, AgenteRepository>();

        // Services - Agente
        services.AddScoped<Constriva.Application.Features.Agente.Services.IAgentService, AgentService>();
        services.AddScoped<Constriva.Application.Features.Agente.Services.IConstrivaToolsService, ConstrivaToolsService>();
        services.AddScoped<Constriva.Application.Features.Agente.Services.IAgenteTokenService, AgenteTokenService>();

        // OpenAI
        services.Configure<Constriva.Application.Features.Agente.Settings.OpenAISettings>(
            configuration.GetSection("OpenAI"));
        services.AddHttpClient("OpenAI", client =>
        {
            var baseUrl = configuration["OpenAI:BaseUrl"] ?? "https://api.openai.com";
            client.BaseAddress = new Uri(baseUrl);
            var apiKey = configuration["OpenAI:ApiKey"];
            if (!string.IsNullOrEmpty(apiKey))
                client.DefaultRequestHeaders.Add("Authorization", $"Bearer {apiKey}");
        });

        // Services - Lens
        services.AddScoped<Constriva.Application.Features.Lens.Interfaces.ILensLogService, LensLogService>();

        // Services
        services.AddScoped<IJwtService, JwtService>();
        services.AddScoped<ICurrentUser, CurrentUserService>();
        services.AddScoped<IEmailService, EmailService>();
        services.AddScoped<IFileStorageService, S3StorageService>();
        services.AddScoped<INotificationService, NotificationService>();

        // HttpContext
        services.AddHttpContextAccessor();

        // JWT Authentication
        var jwtKey = configuration["Jwt:Key"] ?? throw new InvalidOperationException("JWT Key not configured");
        services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            .AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtKey)),
                    ValidateIssuer = true,
                    ValidIssuer = configuration["Jwt:Issuer"],
                    ValidateAudience = true,
                    ValidAudience = configuration["Jwt:Audience"],
                    ValidateLifetime = true,
                    ClockSkew = TimeSpan.Zero
                };

                // Support WebSocket tokens
                options.Events = new JwtBearerEvents
                {
                    OnMessageReceived = context =>
                    {
                        var accessToken = context.Request.Query["access_token"];
                        var path = context.HttpContext.Request.Path;
                        if (!string.IsNullOrEmpty(accessToken) && path.StartsWithSegments("/hubs"))
                            context.Token = accessToken;
                        return Task.CompletedTask;
                    }
                };
            });

        // Redis Cache
        var redisConn = configuration.GetConnectionString("Redis");
        if (!string.IsNullOrEmpty(redisConn))
        {
            services.AddStackExchangeRedisCache(options => options.Configuration = redisConn);
            services.AddScoped<ICacheService, RedisCacheService>();
        }
        else
        {
            services.AddDistributedMemoryCache();
            services.AddScoped<ICacheService, MemoryCacheService>();
        }

        return services;
    }
}
