using EntityFrameworkCore.UnitOfWork.Extensions;
using Infraestructure.Data.Context;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace PaymentApi.Configurations;

/// <summary>
/// Realiza configurações referentes ao uso de banco de dados MySql
/// </summary>
public static class DbConfiguration
{
    /// <summary>
    /// Realiza a configuração do serviço de banco de dados e uso do pacote unit of work
    /// </summary>
    /// <param name="services"></param>
    /// <param name="configuration"></param>
    public static void AddDbConfiguration(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContext<ApplicationDbContext>(options =>
        {
            options.UseMySql(configuration.GetConnectionString("DefaultConnection"),
                            ServerVersion.Parse("8.0.29-mysql"),
                            b => b.MigrationsAssembly("Infrastructure.Data"));
        }, contextLifetime: ServiceLifetime.Transient);
        services.AddUnitOfWork<ApplicationDbContext>(ServiceLifetime.Transient);
    }
}