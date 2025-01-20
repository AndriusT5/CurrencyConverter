using CurrencyConverter.Domain.Interfaces;

namespace CurrencyConverter.Application.Services
{
    public class CurrencyConverterService : ICurrencyConverter
    {
        private readonly IRateProvider _rateProvider;

        public CurrencyConverterService(IRateProvider rateProvider)
        {
            _rateProvider = rateProvider ?? throw new ArgumentNullException(nameof(rateProvider));
        }

        public decimal Convert(string sourceCurrency, string targetCurrency, decimal amount)
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

    }
}
