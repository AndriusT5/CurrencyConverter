using CurrencyConverter.ConsoleApp.Helpers;
using CurrencyConverter.Domain.Exceptions;
using CurrencyConverter.Domain.Interfaces;
using CurrencyConverter.Infrastructure;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using NLog.Extensions.Logging;

namespace CurrencyConverter.ConsoleApp
{
    internal class Program
    {
        static void Main(string[] args)
        {
            var serviceProvider = new ServiceCollection()
                 .AddLogging(builder =>
                 {
                     builder.ClearProviders();
                     builder.SetMinimumLevel(LogLevel.Trace);
                     builder.AddNLog();
                 })
                .AddCurrencyConverterServices()
                .BuildServiceProvider();

            var currencyConverter = serviceProvider.GetRequiredService<ICurrencyConverter>();
            var logger = serviceProvider.GetRequiredService<ILogger<Program>>();

            Console.WriteLine("Currency Calculator started...");
            logger.LogInformation("Starting application.");

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
                    logger.LogError(ex, "An error occurred while converting currency.");
                    Console.ReadLine();
                    Console.Clear();
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Unexpected error: {ex.Message}");
                    logger.LogCritical(ex, "Unexpected error occurred.");
                    Console.ReadLine();
                    Console.Clear();
                }
            }
        }
    }
}
