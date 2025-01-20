using CurrencyConverter.ConsoleApp.Helpers;
using CurrencyConverter.Domain.Exceptions;
using CurrencyConverter.Domain.Interfaces;
using CurrencyConverter.Infrastructure;
using Microsoft.Extensions.DependencyInjection;

namespace CurrencyConverter.ConsoleApp
{
    internal class Program
    {
        static void Main(string[] args)
        {
            var serviceProvider = new ServiceCollection()
                .AddCurrencyConverterServices()
                .BuildServiceProvider();

            var currencyConverter = serviceProvider.GetRequiredService<ICurrencyConverter>();

            Console.WriteLine("Currency Calculator started...");

            Console.WriteLine("Enter base currency (e.g., EUR):");
            var fromCurrency = Console.ReadLine();

            Console.WriteLine("Enter target currency (e.g., DKK):");
            var toCurrency = Console.ReadLine();

            Console.WriteLine("Enter amount:");
            var amountEntered = Console.ReadLine();

            var (sourceCurrency, targetCurrency, amount) = InputParser.ParseInput(fromCurrency, toCurrency, amountEntered);

            try
            {
                var result = currencyConverter.Convert(sourceCurrency, targetCurrency, amount);
                Console.WriteLine($"Converted Amount: {result:0.0000} {targetCurrency}");
            }
            catch (CurrencyNotFoundException ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Unexpected error: {ex.Message}");
            }
        }
    }
}
