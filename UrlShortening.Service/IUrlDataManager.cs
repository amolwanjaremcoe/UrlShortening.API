using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace UrlShortening.Service
{
    public interface IUrlDataManager
    {
        Task<string> GetUrlAsync(string shortCode);

        /// <summary>
        /// 1. Generate the ShortCode by calling ShortCodeGeneratorService
        /// 2. Verify if the code is not already assigned or else regenerate the code
        /// 3. Store the data in the Mongo DB
        /// 4. Return the ShortCode to caller
        /// </summary>
        /// <param name="originalUrl">Original URL</param>
        /// <returns></returns>
        public Task<string> GenerateShortUrlCode(string originalUrl);
    }
}
