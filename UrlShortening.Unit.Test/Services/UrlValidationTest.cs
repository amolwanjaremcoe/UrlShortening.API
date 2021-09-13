using Microsoft.Extensions.Logging;
using Moq;
using NUnit.Framework;
using System.Net.Http;
using System.Threading.Tasks;
using UrlShortening.Service;
using UrlShortening.Service.Implementation;

namespace UrlShortening.Test.Services
{
    [TestFixture]
    public class UrlValidationTest
    {
        private IUrlValidationService _urlValidationService;
        private Mock<IHttpClientFactory> _mockHttpClientFactory;
        private Mock<ILogger> _mockLogger;

        /// <summary>
        /// Setup
        /// </summary>
        [SetUp]
        public void SetUp()
        {
            _mockHttpClientFactory = new Mock<IHttpClientFactory>();
            _mockLogger = new Mock<ILogger>();
            _urlValidationService = new UrlValidationService(_mockHttpClientFactory.Object, _mockLogger.Object);            
        }

        [Test]
        public async Task Incorrect_Formatted_Url_Should_Return_False()
        {
            var incorrectUrl = @"https:/Incorrect.domain";
            var response = await _urlValidationService.IsUrlValid(incorrectUrl);
            Assert.IsFalse(response);
        }

        [Test]
        public async Task Invalid_Url_Should_Return_False()
        {
            var incorrectUrl = @"https://ThisDomainDoesNotExists.Com";
            _mockHttpClientFactory.Setup(f => f.CreateClient(string.Empty)).Returns(new HttpClient());
            var response = await _urlValidationService.IsUrlValid(incorrectUrl);
            Assert.IsFalse(response);
        }

        [Test]
        public async Task Valid_Url_Should_Return_True()
        {
            var correctUrl = @"https://Google.Com";
            _mockHttpClientFactory.Setup(f => f.CreateClient(string.Empty)).Returns(new HttpClient());
            var response = await _urlValidationService.IsUrlValid(correctUrl);
            Assert.IsTrue(response);
        }
    }
}
