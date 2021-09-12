using Microsoft.Extensions.Logging;
using System;
using System.Net.Http;
using System.Threading.Tasks;
using UrlShortening.DataAccess;

namespace UrlShortening.Service.Implementation
{
    public class UrlValidationService : IUrlValidationService
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly ILogger _logger;

        public UrlValidationService(IHttpClientFactory httpClientFactory, ILogger logger)
        {
            _httpClientFactory = httpClientFactory;
            _logger = logger;
        }

        /// <summary>
        /// Check the URL by matching the schema and structure
        /// Verify if the URL is true URL by comparing HTTP GET Response StatusCode value with HttpStatusCode.OK
        /// </summary>
        /// <param name="url"></param>
        /// <returns></returns>
        public async Task<bool> IsUrlValid(string url)
        {
            Uri uriResult;
            _logger.LogDebug($"Validating URL - {url}");
            bool result = Uri.TryCreate(url, UriKind.Absolute, out uriResult)
                && (uriResult.Scheme == Uri.UriSchemeHttp || uriResult.Scheme == Uri.UriSchemeHttps);
            if (result)
            {
                try
                {
                    var httpClient = _httpClientFactory.CreateClient();
                    var response = await httpClient.GetAsync(url);
                    return response.StatusCode == System.Net.HttpStatusCode.OK;
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex,$"Error occured while validating URL - {url}");
                    return false;
                }
            }
            return result;
        }
    }
}
