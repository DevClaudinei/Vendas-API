using Microsoft.Extensions.DependencyInjection;
using System;
using System.Reflection;

namespace PaymentApi.Configurations;

/// <summary>
/// Configura automapper para ser utilizado na camada de aplica��o
/// </summary>
public static class AutoMapperConfiguration
{
    /// <summary>
    /// Declara o servi�o do automapper e informa em qual camada ser� utilizado
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