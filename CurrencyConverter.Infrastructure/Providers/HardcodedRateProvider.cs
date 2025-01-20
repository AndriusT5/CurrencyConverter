using CurrencyConverter.Domain.Exceptions;
using CurrencyConverter.Domain.Interfaces;
using Microsoft.Extensions.Logging;

namespace CurrencyConverter.Infrastructure.Providers
{
    public class HardcodedRateProvider : IRateProvider
    {
        private readonly Dictionary<(string Source, string Target), decimal> _exchangeRates;
        private readonly ILogger<HardcodedRateProvider> _logger;

        public HardcodedRateProvider(ILogger<HardcodedRateProvider> logger)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));

            _exchangeRates = new Dictionary<(string, string), decimal>
            {
                { ("EUR", "DKK"), 743.94m / 100 },
                { ("USD", "DKK"), 663.11m / 100 },
                { ("GBP", "DKK"), 852.85m / 100 },
                { ("SEK", "DKK"), 76.10m / 100 },
                { ("NOK", "DKK"), 78.40m / 100 },
                { ("CHF", "DKK"), 683.58m / 100 },
                { ("JPY", "DKK"), 5.9740m / 100 },
            };
        }

        public decimal GetExchangeRate(string sourceCurrency, string targetCurrency)
        {
            try
            {
                var directKey = (sourceCurrency.ToUpper(), targetCurrency.ToUpper());
                var inverseKey = (targetCurrency.ToUpper(), sourceCurrency.ToUpper());

                if (_exchangeRates.TryGetValue(directKey, out var directRate))
                    return directRate;
                
                else if (_exchangeRates.TryGetValue(inverseKey, out var inverseRate))
                    return 1 / inverseRate;
                
                else
                    throw new CurrencyNotFoundException($"Exchange rate from {sourceCurrency} to {targetCurrency} not found.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error while fetching exchange rate.");
                throw;
            }
        }
    }
}
