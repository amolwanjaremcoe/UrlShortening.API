using Microsoft.Extensions.Logging;
using Moq;
using NUnit.Framework;
using System.Threading.Tasks;
using UrlShortening.DataAccess;
using UrlShortening.Service;
using UrlShortening.Service.Implementation;

namespace UrlShortening.Unit.Test.Services
{
    [TestFixture]
    public class UrlDataManagerTest
    {
        private IUrlDataManager _urlDataManager;
        private Mock<IUrlDataRepository> _mockUrlDataRepository;
        private Mock<IShortCodeGeneratorService> _mockShortCodeGeneratorService;
        private Mock<ILogger> _mockLogger;

        /// <summary>
        /// Setup
        /// </summary>
        [SetUp]
        public void SetUp()
        {
            _mockLogger = new Mock<ILogger>();
            _mockUrlDataRepository = new Mock<IUrlDataRepository>();
            _mockShortCodeGeneratorService = new Mock<IShortCodeGeneratorService>();
            _urlDataManager = new UrlDataManager(_mockUrlDataRepository.Object, _mockShortCodeGeneratorService.Object, _mockLogger.Object);
        }

        [Test]
        public async Task Original_Url_Should_be_Returned_For_Valid_Shortcode()
        {
            var googleUrl = "https://Google.com";
            var stubUrlData = new Model.UrlData { ShortCode = "code", OriginalUrl = googleUrl, 
                ExpirationDate = System.DateTime.Now.AddYears(5), CreationDate = System.DateTime.Now };
            _mockUrlDataRepository.Setup(r => r.GetUrlDataAsync(It.IsAny<string>())).ReturnsAsync(stubUrlData);
            var response = await _urlDataManager.GetUrlAsync(stubUrlData.ShortCode);
            Assert.IsNotEmpty(response);
            Assert.AreEqual(response, googleUrl);
        }

        [Test]
        public async Task Expired_Url_Should_Return_Empty_Url()
        {
            var googleUrl = "https://Google.com";
            var stubUrlData = new Model.UrlData
            {
                ShortCode = "code",
                OriginalUrl = googleUrl,
                ExpirationDate = System.DateTime.Now.AddYears(-5),
                CreationDate = System.DateTime.Now
            };
            _mockUrlDataRepository.Setup(r => r.GetUrlDataAsync(It.IsAny<string>())).ReturnsAsync(stubUrlData);
            _mockUrlDataRepository.Setup(r => r.DeleteAsync(It.IsAny<string>())).ReturnsAsync(true);
            var response = await _urlDataManager.GetUrlAsync(stubUrlData.ShortCode);
            Assert.IsEmpty(response);            
        }
    }
}
