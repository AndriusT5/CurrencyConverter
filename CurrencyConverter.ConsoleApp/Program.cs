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

            while (true)
            {
                try
                {
                    var (fromCurrency, toCurrency, amountEntered) = InputParser.CollectInput();
                    var (sourceCurrency, targetCurrency, amount) = InputParser.ParseInput(fromCurrency, toCurrency, amountEntered);

                    var result = currencyConverter.Convert(sourceCurrency, targetCurrency, amount);
                    OutputFormatter.ShowResult(result, targetCurrency);
                    break;
                }
                catch (CurrencyNotFoundException ex)
                {
                    Console.WriteLine($"Error: {ex.Message}");
                    Console.ReadLine();
                    Console.Clear();
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Unexpected error: {ex.Message}");
                    Console.ReadLine();
                    Console.Clear();
                }
            }
        }
    }
}
