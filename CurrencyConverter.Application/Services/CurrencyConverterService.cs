using CurrencyConverter.Domain.Interfaces;
using Microsoft.Extensions.Logging;

namespace CurrencyConverter.Application.Services
{
    public class CurrencyConverterService : ICurrencyConverter
    {
        private readonly IRateProvider _rateProvider;
        private readonly ILogger<CurrencyConverterService> _logger;

        public CurrencyConverterService(IRateProvider rateProvider, ILogger<CurrencyConverterService> logger)
        {
            _rateProvider = rateProvider ?? throw new ArgumentNullException(nameof(rateProvider));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public decimal Convert(string sourceCurrency, string targetCurrency, decimal amount)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(sourceCurrency))
                    throw new ArgumentException("Source currency cannot be null or empty.", nameof(sourceCurrency));

                if (string.IsNullOrWhiteSpace(targetCurrency))
                    throw new ArgumentException("Target currency cannot be null or empty.", nameof(targetCurrency));

                if (amount <= 0)
                    throw new ArgumentException("Amount must be greater than zero.", nameof(amount));

                sourceCurrency = sourceCurrency.ToUpper();
                targetCurrency = targetCurrency.ToUpper();

                decimal rate = _rateProvider.GetExchangeRate(sourceCurrency, targetCurrency);

                return amount * rate;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred during currency conversion.");
                throw;
            }
        }

    }
}
