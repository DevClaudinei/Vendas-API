using System.Reflection;
using FluentValidation.AspNetCore;
using Microsoft.Extensions.DependencyInjection;

namespace PaymentApi.Configurations;

/// <summary>
/// Realiza configura��es referentes MVC
/// </summary>
public static class MvcConfiguration
{
    /// <summary>
    /// Adiciona configura��es para controllers, fluentvalidation e informa em que camada estar� as validations
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