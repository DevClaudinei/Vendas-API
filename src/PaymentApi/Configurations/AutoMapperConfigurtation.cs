using Microsoft.Extensions.DependencyInjection;
using System;
using System.Reflection;

namespace PaymentApi.Configurations;

/// <summary>
/// Configura automapper para ser utilizado na camada de aplicação
/// </summary>
public static class AutoMapperConfiguration
{
    /// <summary>
    /// Declara o serviço do automapper e informa em qual camada será utilizado
    /// </summary>
    /// <param name="services"></param>
    /// <exception cref="ArgumentNullException"></exception>
    public static void AddAutoMapperConfiguration(this IServiceCollection services)
    {
        if (services == null)
        {
            throw new ArgumentNullException(nameof(services));
        }

        var assembly = Assembly.Load("AppServices");

        services
            .AddAutoMapper((serviceProvider, mapperConfiguration) => mapperConfiguration.AddMaps(assembly), assembly);
    }
}