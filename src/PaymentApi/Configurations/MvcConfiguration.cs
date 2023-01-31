using System.Reflection;
using FluentValidation.AspNetCore;
using Microsoft.Extensions.DependencyInjection;

namespace PaymentApi.Configurations;

/// <summary>
/// Realiza configurações referentes MVC
/// </summary>
public static class MvcConfiguration
{
    /// <summary>
    /// Adiciona configurações para controllers, fluentvalidation e informa em que camada estará as validations
    /// </summary>
    /// <param name="services"></param>
    public static void AddMvcConfiguration(this IServiceCollection services)
    {
        services
            .AddControllers()
            .AddFluentValidation(options => options
            .RegisterValidatorsFromAssembly(Assembly.Load("AppServices")));
    }
}