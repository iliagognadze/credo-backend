using System.Reflection;
using Credo.Application;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.OpenApi.Models;
using Repository;
using Repository.Contracts;
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
}