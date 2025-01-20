namespace CurrencyCalculator.Domain.Interfaces
{
    public interface IRateProvider
    {
        decimal GetExchangeRate(string sourceCurrency, string targetCurrency);
    }
}
