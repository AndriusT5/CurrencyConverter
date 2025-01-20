using Moq;
using CurrencyConverter.Application.Services;
using CurrencyConverter.Domain.Interfaces;
using CurrencyConverter.Domain.Exceptions;
using Microsoft.Extensions.Logging;

namespace CurrencyConverter.Tests.UnitTests
{
    [TestFixture]
    public class CurrencyConverterServiceTests
    {
        private Mock<IRateProvider> _mockRateProvider;
        private Mock<ILogger<CurrencyConverterService>> _mockLogger;
        private CurrencyConverterService _currencyConverterService;

        [SetUp]
        public void Setup()
        {
            _mockLogger = new Mock<ILogger<CurrencyConverterService>>();
            _mockRateProvider = new Mock<IRateProvider>();
            _currencyConverterService = new CurrencyConverterService(_mockRateProvider.Object, _mockLogger.Object);
        }

        [Test]
        public void Convert_ValidInputs_ReturnsConvertedAmount()
        {
            // Arrange
            string sourceCurrency = "EUR";
            string targetCurrency = "DKK";
            decimal amount = 100m;
            decimal expectedRate = 7.4394m;
            decimal expectedConvertedAmount = 743.94m;

            _mockRateProvider.Setup(p => p.GetExchangeRate(sourceCurrency, targetCurrency))
                             .Returns(expectedRate);

            // Act
            var result = _currencyConverterService.Convert(sourceCurrency, targetCurrency, amount);

            // Assert
            Assert.AreEqual(expectedConvertedAmount, result);
            _mockRateProvider.Verify(p => p.GetExchangeRate(sourceCurrency, targetCurrency), Times.Once);
        }

        [Test]
        public void Convert_InvalidCurrency_ThrowsCurrencyNotFoundException()
        {
            // Arrange
            string sourceCurrency = "INVALID";
            string targetCurrency = "EUR";
            decimal amount = 100m;

            _mockRateProvider.Setup(p => p.GetExchangeRate(sourceCurrency, targetCurrency))
                             .Throws(new CurrencyNotFoundException("Currency not found."));

            // Act & Assert
            var ex = Assert.Throws<CurrencyNotFoundException>(() =>
                _currencyConverterService.Convert(sourceCurrency, targetCurrency, amount));

            Assert.That(ex.Message, Is.EqualTo("Currency not found."));
            _mockRateProvider.Verify(p => p.GetExchangeRate(sourceCurrency, targetCurrency), Times.Once);
        }

        [Test]
        public void Convert_NegativeAmount_ThrowsArgumentException()
        {
            // Arrange
            string sourceCurrency = "EUR";
            string targetCurrency = "DKK";
            decimal amount = -1m;

            // Act & Assert
            var ex = Assert.Throws<ArgumentException>(() =>
                _currencyConverterService.Convert(sourceCurrency, targetCurrency, amount));

            Assert.That(ex.Message, Does.Contain("Amount must be greater than zero."));
            _mockRateProvider.VerifyNoOtherCalls();
        }
    }
}