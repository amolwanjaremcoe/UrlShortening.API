using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;
using UrlShortening.Model;
using UrlShortening.Model.Exception;
using UrlShortening.Service;

namespace UrlShortening.API.Controllers
{
    public class UrlController : Controller
    {
        private readonly IUrlValidationService _urlValidationService;
        private readonly ILogger _logger;
        private readonly IUrlDataManager _urlDataManager;

        public UrlController(IUrlValidationService urlValidationService, IUrlDataManager urlDataManager, ILogger logger)
        {
            _urlValidationService = urlValidationService;
            _urlDataManager = urlDataManager;
            _logger = logger;
        }

        // GET: UrlController
        [HttpGet("{shortCode}")]
        public async Task<ActionResult> Index(string shortCode)
        {
            _logger.LogDebug($"Received Redirect request for code {shortCode}");
            if (!string.IsNullOrEmpty(shortCode))
            {
                string urlString = await _urlDataManager.GetUrlAsync(shortCode);
                if (!string.IsNullOrWhiteSpace(urlString))
                {
                    return Redirect(urlString);
                }
                else
                {
                    _logger.LogError($"Error: Unable to retrieve Original URL for code {shortCode}");
                }
            }
            else
            {
                _logger.LogError("Error: Received EMPTY URL short code"); 
            }
            return NotFound();
        }

        [HttpPost("short")]
        public async Task<string> CreateShortUrl([FromBody] UrlViewModel urlViewModel)
        {
            _logger.LogDebug($"Received URL Shortening request for {urlViewModel?.OriginalUrl}");
            if (urlViewModel != null && await _urlValidationService.IsUrlValid(urlViewModel.OriginalUrl))
            {
                _logger.LogDebug($"Processing Shortening request for {urlViewModel?.OriginalUrl}");
                var shortCode = await _urlDataManager.GenerateShortUrlCode(urlViewModel.OriginalUrl);
                return $"{Request.Scheme}://{Request.Host}/{shortCode}";               
            }
            else
            {
                throw new InvalidUrlException(urlViewModel?.OriginalUrl);
            }
        }        
    }
}
