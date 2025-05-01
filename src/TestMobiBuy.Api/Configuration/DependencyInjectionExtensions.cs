using System.Diagnostics.CodeAnalysis;
using System.IO.Compression;
using System.Text.Json;
using System.Text.Json.Serialization;
using MassTransit;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.ResponseCompression;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using TestMobiBuy.Application.Externals;
using TestMobiBuy.Application.Interfaces;
using TestMobiBuy.Application.Services;
using TestMobiBuy.Domain.Interfaces;
using TestMobiBuy.Domain.Settings;
using TestMobiBuy.Infrastructure.Contexts;
using TestMobiBuy.Infrastructure.Repositories;
using TestMobiBuy.Share.Converters;

namespace TestMobiBuy.Api.Configuration;

[ExcludeFromCodeCoverage]
public static class DependencyInjectionExtensions
{
    public static void AddDependencyInjection(this IServiceCollection services, IConfiguration config)
    {

        if (services is null) throw new ArgumentNullException(nameof(services));

        services.AddSettings(config);
        services.AddSwaggerDocs(config);
        services.AddHttpClient();
        services.AddHttpContextAccessor();
        services.AddResponseCaching();
        services.AddConfigureOptions(config);
        services.AddServices();
        services.AddRepositories();
        services.AddExternals();
        services.AddRabbitMQ(config);
        services.AddCompression();
        services.AddJsonSerializer();
        services.AddControllers();
    }

    private static IServiceCollection AddSettings(this IServiceCollection services, IConfiguration config)
    {
        if (services is null) throw new ArgumentNullException(nameof(services));
        if (config is null) throw new ArgumentNullException(nameof(config));

        services.Configure<SwaggerSettings>(config.GetSection(SwaggerSettings.SectionName));
        services.Configure<ConnectionSettings>(config.GetSection(ConnectionSettings.SectionName));
        services.Configure<ExternalServicesSettings>(config.GetSection(ExternalServicesSettings.SectionName));

        return services;
    }

    public static void AddRabbitMQ(this IServiceCollection services, IConfiguration configuration)
    {
        var rabbitMqSettings = configuration.GetSection("RabbitMq").Get<RabbitMqSettings>();
        
        services.AddMassTransit(x =>
        {
            x.UsingRabbitMq((context, cfg) =>
            {
                cfg.Host(rabbitMqSettings.Host, rabbitMqSettings.VirtualHost, h =>
                {
                    h.Username(rabbitMqSettings.Username);
                    h.Password(rabbitMqSettings.Password);
                });
            });
        });

        services.AddScoped<IMessageBusService, MessageBusService>();
    }
    
    private static IServiceCollection AddSwaggerDocs(this IServiceCollection services, IConfiguration config)
    {
        if (services is null) throw new ArgumentNullException(nameof(services));

        var swaggerSettings = config.GetSection(SwaggerSettings.SectionName).Get<SwaggerSettings>();

        services.AddSwaggerGen(options =>
        {
            options.SwaggerDoc("v1", new OpenApiInfo { Title = swaggerSettings?.Title ?? "API", Version = swaggerSettings?.Version ?? "v1" });
        });

        return services;
    }

    private static IServiceCollection AddCompression(this IServiceCollection services)
    {
        services.AddResponseCompression(options =>
        {
            options.MimeTypes = ResponseCompressionDefaults.MimeTypes.Concat(new[] { "application/json" }); // Adicionar tipos MIME adicionais, se necess�rio
            options.Providers.Add<GzipCompressionProvider>();
            options.Providers.Add<BrotliCompressionProvider>();
            options.EnableForHttps = true;
        });

        services.Configure<GzipCompressionProviderOptions>(options => options.Level = CompressionLevel.SmallestSize);
        services.Configure<BrotliCompressionProviderOptions>(options => options.Level = CompressionLevel.SmallestSize);

        return services;
    }

    private static IServiceCollection AddJsonSerializer(this IServiceCollection services)
    {
        services.Configure<JsonOptions>(options =>
        {
            options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
            options.JsonSerializerOptions.Converters.Add(new JsonIsoDateTimeConverter());
            options.JsonSerializerOptions.Converters.Add(new JsonIsoDateTimeOffSetConverter());
            options.JsonSerializerOptions.PropertyNamingPolicy = JsonNamingPolicy.CamelCase;
            options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
            options.JsonSerializerOptions.MaxDepth = 128;
            options.JsonSerializerOptions.DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull;
        });

        return services;
    }


    private static void AddConfigureOptions(this IServiceCollection services, IConfiguration config)
    {
        services.AddDbContext<DataContext>(options => options.UseNpgsql(config.GetConnectionString("DefaultConnection")));

        // Executar migrações ao iniciar a aplicação
        using (var serviceProvider = services.BuildServiceProvider())
        {
            using (var scope = serviceProvider.CreateScope())
            {
                var dbContext = scope.ServiceProvider.GetRequiredService<DataContext>();
                dbContext.Database.Migrate();
            }
        }
    }

    private static void AddServices(this IServiceCollection services)
    {
        services.AddScoped<ICustomerService, CustomerService>();
    }

    private static void AddRepositories(this IServiceCollection services)
    {
        services.AddScoped<ICustomerRepository, CustomerRepository>();
        services.AddScoped<ICustomerAddressRepository, CustomerAddressRepository>();
    }

    private static void AddExternals(this IServiceCollection services)
    {
        services.AddScoped<IViaCepApiExternal, ViaCepApiExternal>();
    }
}
