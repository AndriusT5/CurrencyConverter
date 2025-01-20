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
            var serviceProvider = ConfigureServices();

            var currencyConverter = serviceProvider.GetRequiredService<ICurrencyConverter>();
            var logger = serviceProvider.GetRequiredService<ILogger<Program>>();

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
                    HandleKnownError(ex, logger);
                }
                catch (Exception ex)
                {
                    HandleUnknownError(ex, logger);
                }
            }
        }

        private static ServiceProvider ConfigureServices()
        {
            return new ServiceCollection()
                .AddLogging(builder =>
                {
                    builder.ClearProviders();
                    builder.SetMinimumLevel(LogLevel.Trace);
                    builder.AddNLog();
                })
                .AddCurrencyConverterServices()
                .BuildServiceProvider();
        }

        private static void HandleKnownError(CurrencyNotFoundException ex, ILogger logger)
        {
            Console.WriteLine($"Error: {ex.Message}");
            logger.LogError(ex, "A known error occurred while converting currency.");
            WaitAndClear();
        }

        private static void HandleUnknownError(Exception ex, ILogger logger)
        {
            Console.WriteLine($"Unexpected error: {ex.Message}");
            logger.LogCritical(ex, "An unexpected error occurred.");
            WaitAndClear();
        }

        private static void WaitAndClear()
        {
            Console.ReadLine();
            Console.Clear();
        }
    }
}
