using CurrencyConverter.Application.Services;
using CurrencyConverter.Domain.Interfaces;
using CurrencyConverter.Infrastructure.Providers;
using Microsoft.Extensions.DependencyInjection;

namespace CurrencyConverter.Infrastructure
{
    public static class DependencyInjectionConfig
    {
        public static IServiceCollection AddCurrencyConverterServices(this IServiceCollection services)
        {
            services.AddScoped<ICurrencyConverter, CurrencyConverterService>();
            services.AddSingleton<IRateProvider, HardcodedRateProvider>();
            return services;
        }
    }
}
