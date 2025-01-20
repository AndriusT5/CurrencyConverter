namespace CurrencyConverter.ConsoleApp.Helpers
{
    public static class OutputFormatter
    {
        public static void ShowResult(decimal amount, string currency)
        {
            Console.WriteLine($"Converted Amount: {amount:0.0000} {currency}");
            Console.ReadLine();
        }
    }
}
