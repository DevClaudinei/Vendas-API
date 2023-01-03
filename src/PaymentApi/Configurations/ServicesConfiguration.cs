using AppServices;
using DomainServices;
using DomainServices.Interfaces;
using Microsoft.Extensions.DependencyInjection;

namespace PaymentApi.Configurations;

public static class ServicesConfiguration
{
    public static void AddServicesConfiguration(this IServiceCollection services)
    {
        services.AddTransient<IVendaAppService, VendaAppService>();

        services.AddTransient<IVendaService, VendaService>();
    }
}