using Moq;
using CurrencyCalculator.Application.Services;
using CurrencyCalculator.Domain.Interfaces;
using CurrencyCalculator.Domain.Exceptions;

namespace CurrencyCalculator.Tests.UnitTests
{
    [TestFixture]
    public class CurrencyConverterServiceTests
    {
        private Mock<IRateProvider> _mockRateProvider;
        private CurrencyConverterService _currencyConverterService;

        [SetUp]
        public void Setup()
        {
            _mockRateProvider = new Mock<IRateProvider>();
            _currencyConverterService = new CurrencyConverterService(_mockRateProvider.Object);
        }

        [Test]
        public void Convert_ValidInputs_ReturnsConvertedAmount()
        {
            // Arrange
            string sourceCurrency = "USD";
            string targetCurrency = "EUR";
            decimal amount = 100m;
            decimal expectedRate = 0.85m;
            decimal expectedConvertedAmount = 85m;

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
            string sourceCurrency = "USD";
            string targetCurrency = "EUR";
            decimal amount = -10m;

            // Act & Assert
            var ex = Assert.Throws<ArgumentException>(() =>
                _currencyConverterService.Convert(sourceCurrency, targetCurrency, amount));

            Assert.That(ex.Message, Does.Contain("Amount must be greater than zero."));
            _mockRateProvider.VerifyNoOtherCalls();
        }
    }
}