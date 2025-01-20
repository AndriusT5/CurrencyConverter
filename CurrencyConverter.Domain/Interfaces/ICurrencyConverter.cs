namespace CurrencyConverter.Domain.Interfaces
{
    public interface ICurrencyConverter
    {
        decimal Convert(string sourceCurrency, string targetCurrency, decimal amount);
    }
}
