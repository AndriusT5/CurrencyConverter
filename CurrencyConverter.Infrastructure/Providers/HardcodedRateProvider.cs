using CurrencyConverter.Domain.Exceptions;
using CurrencyConverter.Domain.Interfaces;

namespace CurrencyConverter.Infrastructure.Providers
{
    public class HardcodedRateProvider : IRateProvider
    {
        private readonly Dictionary<(string Source, string Target), decimal> _exchangeRates;

        public HardcodedRateProvider()
        {
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
            var directKey = (sourceCurrency.ToUpper(), targetCurrency.ToUpper());
            var inverseKey = (targetCurrency.ToUpper(), sourceCurrency.ToUpper());

            if (_exchangeRates.TryGetValue(directKey, out var directRate))
            {
                return directRate;
            }
            else if (_exchangeRates.TryGetValue(inverseKey, out var inverseRate))
            {
                return 1 / inverseRate;
            }
            else
            {
                throw new CurrencyNotFoundException($"Exchange rate from {sourceCurrency} to {targetCurrency} not found.");
            }
        }
    }
}
