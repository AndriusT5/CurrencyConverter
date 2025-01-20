using CurrencyCalculator.Application.Services;
using CurrencyCalculator.Domain.Interfaces;
using CurrencyCalculator.Infrastructure.Providers;
using Microsoft.Extensions.DependencyInjection;

namespace CurrencyCalculator.Infrastructure
{
    public static class DependencyInjectionConfig
    {
        public static IServiceCollection AddCurrencyCalculatorServices(this IServiceCollection services)
        {
            services.AddScoped<ICurrencyConverter, CurrencyConverterService>();
            services.AddSingleton<IRateProvider, HardcodedRateProvider>();
            return services;
        }
    }
}
