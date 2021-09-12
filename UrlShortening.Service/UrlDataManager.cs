using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using UrlShortening.DataAccess;
using UrlShortening.Model;

namespace UrlShortening.Service
{
    public class UrlDataManager : IUrlDataManager
    {
        private readonly IUrlDataRepository _urlDataRepository;
        private readonly IShortCodeGeneratorService _shortCodeGeneratorService;
        private readonly int _codeGenerationMaxAttempts = 5;
        private readonly int _defaultExpirationYear = 5;
        public UrlDataManager(IUrlDataRepository urlDataRepository, IShortCodeGeneratorService shortCodeGeneratorService)
        {
            _urlDataRepository = urlDataRepository;
            _shortCodeGeneratorService = shortCodeGeneratorService;
        }

        public async Task<string> GetUrlAsync(string shortCode)
        {
            var urlData = await _urlDataRepository.GetUrlDataAsync(shortCode);
            if (urlData != null)
            {
                if (DateTime.Now <= urlData.ExpirationDate)
                {
                    return urlData.OriginalUrl;
                }
                else
                {
                    await _urlDataRepository.DeleteAsync(shortCode);
                }
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
            var shortCode = await GenerateUniqueShortCode(originalUrl);
            await _urlDataRepository.CreateAsync(new UrlData
            {
                ExpirationDate = DateTime.UtcNow.AddYears(_defaultExpirationYear),
                OriginalUrl = originalUrl,
                ShortCode = shortCode
            });
            return shortCode;
        }

        private async Task<string> GenerateUniqueShortCode(string originalUrl)
        {
            int attempts = 0;
            while (true)
            {
                var shortCode = await _shortCodeGeneratorService.GenerateShortCode(originalUrl);
                if (await _urlDataRepository.GetUrlDataAsync(shortCode) != null)
                {
                    attempts++;
                    if (attempts >= _codeGenerationMaxAttempts)
                        throw new Exception("Short key generation failed");
                }
                else
                {
                    return shortCode;
                }
            }
        }
    }
}
