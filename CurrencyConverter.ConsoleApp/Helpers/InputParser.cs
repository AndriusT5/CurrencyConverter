namespace CurrencyConverter.ConsoleApp.Helpers
{
    public static class InputParser
    {
        public static (string fromCurrency, string toCurrency, string amount) CollectInput()
        {
            Console.WriteLine("Enter base currency (e.g., EUR):");
            var fromCurrency = Console.ReadLine();

            Console.WriteLine("Enter target currency (e.g., DKK):");
            var toCurrency = Console.ReadLine();

            Console.WriteLine("Enter amount:");
            var amountEntered = Console.ReadLine();

            return (fromCurrency, toCurrency, amountEntered);
        }

        public static (string fromCurrency, string toCurrency, decimal amount) ParseInput(string inputFrom, string inputTo, string inputAmount)
        {
            if (string.IsNullOrWhiteSpace(inputFrom) || string.IsNullOrWhiteSpace(inputTo))
            {
                throw new FormatException("Currency cannot be empty. Please enter a valid currency.");
            }

            if (inputFrom.Trim().Length != 3 || inputTo.Trim().Length != 3)
            {
                throw new FormatException("Invalid currency format. Please use the format: 'ABC'");
            }

            var fromCurrency = inputFrom.ToUpper();
            var toCurrency = inputTo.ToUpper();

            if (!decimal.TryParse(inputAmount, out decimal amount) || amount <= 0)
            {
                throw new FormatException("Invalid amount. Please enter a positive number.");
            }

            return (fromCurrency, toCurrency, amount);
        }
    }
}
