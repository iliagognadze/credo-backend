using System.Reflection;
using Amazon;
using Amazon.CloudWatchLogs;
using Amazon.Internal;
using Amazon.Runtime;
using Credo.Application;
using Microsoft.AspNetCore.HttpLogging;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.OpenApi.Models;
using Repository;
using Repository.Contracts;
using Serilog;
using Serilog.Sinks.AwsCloudWatch;
using Shared.Options;

namespace Credo.API.Extensions;

public static class ServiceExtensions
{
    public static void ConfigureSwagger(this IServiceCollection services)
    {
        services.AddSwaggerGen(c =>
        {
            c.SwaggerDoc("v1", new OpenApiInfo { Title = "credo", Version = "v1" });
        });
    }

    public static void ConfigureSqlContext(this IServiceCollection services, IConfiguration configuration) =>
        services.AddDbContext<RepositoryContext>(opts =>
            opts.UseNpgsql(configuration.GetConnectionString("DefaultConnection")!, options =>
            {
                options.MigrationsHistoryTable(HistoryRepository.DefaultTableName, "public");
            }));

    public static void ConfigureAutoMapper(this IServiceCollection services) =>
        services.AddAutoMapper(Assembly.GetEntryAssembly());
    
    public static void ConfigureRepository(this IServiceCollection services) =>
        services.AddScoped<IRepositoryManager, RepositoryManager>();

    public static void ConfigureOptions(this IServiceCollection services, IConfiguration configuration)
    {
        services.Configure<JwtSettings>(configuration.GetSection("JwtSettings"));
    }

    public static void ConfigureMediator(this IServiceCollection services) => 
        services.AddMediatR(configuration =>
            configuration.RegisterServicesFromAssembly(typeof(ApplicationAssembly).Assembly));
    
    public static void ConfigureLogger(this IHostBuilder host, WebApplicationBuilder builder) =>
        host.ConfigureLogging((_, logging) =>
        {
            logging.ClearProviders();

            var client = new AmazonCloudWatchLogsClient(new BasicAWSCredentials(
                    builder.Configuration["Logger:Credentials:AccessKey"],
                    builder.Configuration["Logger:Credentials:SecretKey"]), 
                RegionEndpoint.GetBySystemName(builder.Configuration["Logger:Region"]));
            
            var logger = new LoggerConfiguration()
                .WriteTo.Console()
                .WriteTo.AmazonCloudWatch(
                    logGroup: $"{builder.Environment.EnvironmentName}/{builder.Environment.ApplicationName}",
                    logStreamPrefix: DateTime.UtcNow.ToString("yyyyMMddHHmmssfff"),
                    cloudWatchClient: client
                )
                .CreateLogger();

            logging.AddSerilog(logger);
            builder.Services.AddSingleton(logger);
        });

    public static void ConfigureHttpLogging(this IServiceCollection services)
    {
        services.AddHttpLogging(logging =>
        {
            logging.LoggingFields = HttpLoggingFields.All;
            logging.RequestHeaders.Add("sec-ch-ua");
            logging.ResponseHeaders.Add("MyResponseHeader");
            logging.MediaTypeOptions.AddText("application/javascript");
            logging.RequestBodyLogLimit = 4096;
            logging.ResponseBodyLogLimit = 4096;
        });
    }
}