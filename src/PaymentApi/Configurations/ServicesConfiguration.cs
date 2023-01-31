using AppServices.Services;
using AppServices.Services.Interfaces;
using DomainServices.Interfaces;
using DomainServices.Services;
using Microsoft.Extensions.DependencyInjection;

namespace PaymentApi.Configurations;

/// <summary>
/// Classe de configuração dos serviços
/// </summary>
public static class ServicesConfiguration
{
    /// <summary>
    /// Realiza injeção de dependencia para as camadas de aplicação e de domínio
    /// </summary>
    /// <param name="services"></param>
    public static void AddServicesConfiguration(this IServiceCollection services)
    {
        services.AddTransient<IVendaAppService, VendaAppService>();

        services.AddTransient<IVendaService, VendaService>();
    }
}