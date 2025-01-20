using CurrencyConverter.Application.Services;
using CurrencyConverter.Domain.Exceptions;
using CurrencyConverter.Infrastructure.Providers;
using Microsoft.Extensions.Logging;
using Moq;

namespace CurrencyConverter.Tests.UnitTests
{
    [TestFixture]
    public class HardcodedRateProviderTests
    {
        private HardcodedRateProvider _rateProvider;
        private Mock<ILogger<HardcodedRateProvider>> _mockLogger;

        [SetUp]
        public void Setup()
        {
            _mockLogger = new Mock<ILogger<HardcodedRateProvider>>();
            _rateProvider = new HardcodedRateProvider(_mockLogger.Object);
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
        public void GetExchangeRate_ValidInverseCurrencies_ReturnsCorrectInverseRate()
        {
            // Arrange
            string sourceCurrency = "DKK";
            string targetCurrency = "EUR";
            decimal expectedRate = 0.1344m;

            // Act
            var rate = _rateProvider.GetExchangeRate(sourceCurrency, targetCurrency);

            // Assert
            Assert.AreEqual(expectedRate, Math.Round(rate, 4));
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