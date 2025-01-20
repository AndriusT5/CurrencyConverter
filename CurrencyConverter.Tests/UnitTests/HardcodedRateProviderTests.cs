using CurrencyConverter.Domain.Exceptions;
using CurrencyConverter.Infrastructure.Providers;

namespace CurrencyConverter.Tests.UnitTests
{
    [TestFixture]
    public class HardcodedRateProviderTests
    {
        private HardcodedRateProvider _rateProvider;

        [SetUp]
        public void Setup()
        {
            _rateProvider = new HardcodedRateProvider();
        }

        [Test]
        public void GetExchangeRate_ValidCurrencies_ReturnsCorrectRate()
        {
            // Arrange
            string sourceCurrency = "EUR";
            string targetCurrency = "DKK";
            decimal expectedRate = 7.4394m;

            // Act
            var rate = _rateProvider.GetExchangeRate(sourceCurrency, targetCurrency);

            // Assert
            Assert.AreEqual(expectedRate, rate);
        }

        [Test]
        public void GetExchangeRate_InvalidCurrencies_ThrowsCurrencyNotFoundException()
        {
            // Arrange
            string sourceCurrency = "XYZ";
            string targetCurrency = "ABC";

            // Act & Assert
            var ex = Assert.Throws<CurrencyNotFoundException>(() =>
                _rateProvider.GetExchangeRate(sourceCurrency, targetCurrency));

            Assert.That(ex.Message, Does.Contain("Exchange rate from XYZ to ABC not found."));
        }
    }
}