using AppServices.Services;
using AppServices.Services.Interfaces;
using DomainServices.Interfaces;
using DomainServices.Services;
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