using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using UrlShortening.Model;
using UrlShortening.Service;

namespace UrlShortening.API.Controllers
{
   // [Route("[controller]")]
    public class UrlController : Controller
    {
        private readonly IUrlValidationService _urlValidationService;

        private readonly IUrlDataManager _urlDataManager;

        public UrlController(IUrlValidationService urlValidationService, IUrlDataManager urlDataManager)
        {
            _urlValidationService = urlValidationService;
            _urlDataManager = urlDataManager;
        }

        // GET: UrlController
        [HttpGet("{shortCode}")]
        public async Task<ActionResult> Index(string shortCode)
        {
            if (!string.IsNullOrEmpty(shortCode))
            {
                string urlString = await _urlDataManager.GetUrlAsync(shortCode);
                if (!String.IsNullOrWhiteSpace(urlString))
                {
                    return Redirect(urlString);
                }              
            }
            return NotFound();
        }

        [HttpPost("short")]
        public async Task<string> CreateUrl([FromBody] UrlViewModel urlViewModel)
        {
            if (urlViewModel != null && await _urlValidationService.IsUrlValid(urlViewModel.OriginalUrl))
            {
                var shortCode = await _urlDataManager.GenerateShortUrlCode(urlViewModel.OriginalUrl);
                return $"{Request.Scheme}://{Request.Host}/{shortCode}";               
            }
            else
            {
                throw new HttpRequestException("Invalid Original Url value was provided.");
            }
        }

        
    }
}
