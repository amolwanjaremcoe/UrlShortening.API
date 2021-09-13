using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;
using UrlShortening.DataAccess;
using UrlShortening.Model;
using UrlShortening.Model.Exception;

namespace UrlShortening.Service.Implementation
{
    public class UrlDataManager : IUrlDataManager
    {
        private readonly IUrlDataRepository _urlDataRepository;
        private readonly IShortCodeGeneratorService _shortCodeGeneratorService;
        private readonly int _codeGenerationMaxAttempts;
        private readonly int _defaultExpirationYear;
        private readonly ILogger _logger;        

        public UrlDataManager(IUrlDataRepository urlDataRepository, IShortCodeGeneratorService shortCodeGeneratorService, ILogger logger, ServerConfig serverConfig)
        {
            _urlDataRepository = urlDataRepository;
            _shortCodeGeneratorService = shortCodeGeneratorService;
            _logger = logger;
            _codeGenerationMaxAttempts = serverConfig.CodeGenerationMaxAttempts;
            _defaultExpirationYear = serverConfig.ShortcodeExpirationYear;
        }

        public async Task<string> GetUrlAsync(string shortCode)
        {
            _logger.LogDebug($"Get URL with code - {shortCode}");
            var urlData = await _urlDataRepository.GetUrlDataAsync(shortCode);
            if (urlData != null)
            {
                if (DateTime.Now <= urlData.ExpirationDate)
                {
                    return urlData.OriginalUrl;
                }
                else
                {
                    _logger.LogInformation($"Deleting expired short URL with code - {shortCode}");
                    await _urlDataRepository.DeleteAsync(shortCode);
                }
            }
            else
            {
                _logger.LogInformation($"Short URL with code {shortCode} not found");
            }
            return string.Empty;
        }

        /// <summary>
        /// 1. Generate the ShortCode by calling ShortCodeGeneratorService
        /// 2. Verify if the code is not already assigned or else regenerate the code
        /// 3. Store the data in the Mongo DB
        /// 4. Return the ShortCode to caller
        /// </summary>
        /// <param name="originalUrl">Original URL</param>
        /// <returns></returns>
        public async Task<string> GenerateShortUrlCode(string originalUrl)
        {
            _logger.LogDebug($"Generating Short code for URL - {originalUrl}");
            var shortCode = await GenerateUniqueShortCode(originalUrl);
            await _urlDataRepository.CreateAsync(new UrlData
            {
                ExpirationDate = DateTime.UtcNow.AddYears(_defaultExpirationYear),
                OriginalUrl = originalUrl,
                ShortCode = shortCode
            });
            _logger.LogDebug($"Generated Short code {shortCode} for URL - {originalUrl}");
            return shortCode;
        }

        /// <summary>
        /// Function to generate Unique short code for the URL
        /// It will call the Code generation service to generate a short code
        /// If the short code value is already used, it will re attempt to generate the code for up to the value of codeGenerationMaxAttempts
        /// </summary>
        /// <param name="originalUrl"></param>
        /// <returns></returns>
        private async Task<string> GenerateUniqueShortCode(string originalUrl)
        {
            int attempts = 0;
            while (true)
            {
                _logger.LogDebug($"Attemp #{attempts+1} to generate short code for URL - {originalUrl}");
                var shortCode = _shortCodeGeneratorService.GenerateShortCode(originalUrl);
                if (await _urlDataRepository.GetUrlDataAsync(shortCode) != null)
                {
                    attempts++;
                    if (attempts >= _codeGenerationMaxAttempts)
                        throw new KeyGenerationFailedException();
                }
                else
                {
                    _logger.LogDebug($"Generated short code {shortCode} in Attemp #{attempts} for URL - {originalUrl}");
                    return shortCode;
                }
            }
        }
    }
}
