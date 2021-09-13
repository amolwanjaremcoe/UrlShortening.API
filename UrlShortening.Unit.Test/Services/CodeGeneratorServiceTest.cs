using Microsoft.Extensions.Logging;
using Moq;
using NUnit.Framework;
using UrlShortening.Service;
using UrlShortening.Service.Implementation;

namespace UrlShortening.Unit.Test.Services
{
    [TestFixture]
    public class CodeGeneratorServiceTest
    {
        private IShortCodeGeneratorService _shortCodeGeneratorService;
        private Mock<ILogger> _mockLogger;

        /// <summary>
        /// Setup
        /// </summary>
        [SetUp]
        public void SetUp()
        {
            _mockLogger = new Mock<ILogger>();
            _shortCodeGeneratorService = new ShortCodeGeneratorService(_mockLogger.Object);
        }

        [Test]
        public void Service_Should_Generate_Six_Digit_ShortCode()
        {
            var url = @"https://Google.com";
            var shortCode = _shortCodeGeneratorService.GenerateShortCode(url);
            Assert.AreEqual(6,shortCode.Length);
        }

        [Test]
        public void Service_Should_Generate_Different_ShortCode_For_Multiple_Requests_for_Same_Url()
        {
            var url = @"https://Google.com";
            var shortCode1 = _shortCodeGeneratorService.GenerateShortCode(url);
            var shortCode2 = _shortCodeGeneratorService.GenerateShortCode(url);
            Assert.AreNotEqual(shortCode1, shortCode2);           
        }
    }
}
