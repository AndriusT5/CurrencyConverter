using CurrencyCalculator.Domain.Exceptions;
using CurrencyCalculator.Domain.Interfaces;

namespace CurrencyCalculator.Infrastructure.Providers
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
            var key = (sourceCurrency.ToUpper(), targetCurrency.ToUpper());

            if (!_exchangeRates.TryGetValue(key, out var rate))
            {
                throw new CurrencyNotFoundException($"Exchange rate from {sourceCurrency} to {targetCurrency} not found.");
            }

            return rate;
        }
    }
}
