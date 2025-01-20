namespace CurrencyCalculator.ConsoleApp.Helpers
{
    public static class InputParser
    {
        public static (string FromCurrency, string ToCurrency, decimal Amount) ParseInput(string inputFrom, string inputTo, string inputAmount)
        {
            if (string.IsNullOrWhiteSpace(inputFrom) || string.IsNullOrWhiteSpace(inputTo))
            {
                throw new FormatException("Currency cannot be empty. Please enter a valid currency.");
            }

            if (inputFrom.Trim().Length != 3 || inputFrom.Trim().Length != 3)
            {
                throw new FormatException("Invalid currency format. Please use the format: '<SourceCurrency> <TargetCurrency> <Amount>'");
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
