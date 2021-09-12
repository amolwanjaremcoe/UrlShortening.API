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
    [Route("[controller]")]
    public class UrlController : Controller
    {
        private readonly IUrlValidationService _urlValidationService;

        private readonly MongoDBConfig _mongoDBConfig;

        public UrlController(IUrlValidationService urlValidationService, IConfiguration configuration)
        {
            _urlValidationService = urlValidationService;
            _mongoDBConfig = configuration.GetSection("MongoDBConfig").Get<MongoDBConfig>();
        }

        // GET: UrlController
        [HttpGet("{key}")]
        public ActionResult Index(string key)
        {
            String urlString = @"http://google.com";//new ShortUrl.Logic.UrlManager().GetUrl(key);
            if (!String.IsNullOrWhiteSpace(urlString))
            {
                return Redirect(urlString);
            }
            return null;
        }

        [HttpPost]
        public async Task<string> CreateUrl(string originalUrl)
        {
            if (await _urlValidationService.IsUrlValid(originalUrl))
            {
                return $"this is short url for {originalUrl}";
            }
            else
            {
                throw new HttpRequestException("Invalid Original Url value was provided.");
            }
        }

        
    }
}
