using Base62;
using Microsoft.Extensions.Logging;
using System;
using System.Security.Cryptography;
using System.Text;

namespace UrlShortening.Service.Implementation
{
    public class ShortCodeGeneratorService : IShortCodeGeneratorService
    {
        private readonly ILogger _logger;

        public ShortCodeGeneratorService(ILogger logger)
        {
            _logger = logger;
        }

        /// <summary>
        /// Generate and return the SIX digit unique short code
        /// This function us using MD5 hash algorithm producing a 128-bit (16 bytes) hash value
        /// In order to generate different hash code for every request, we are appending the date time ticks before hashing the URL value
        /// The hash can then be encoded using the base62([A-Z, a-z, 0-9]) of this 16 byte value
        /// Using base62 encoding, a 6 letters long key would result in 62^6 = ~56.8 billion possible strings
        /// </summary>
        /// <returns></returns>
        public string GenerateShortCode(string url)
        {
            _logger.LogDebug($"Generating short code for url - {url} using MD5 and base62");
            url = $"{url}{DateTime.Now.Ticks}"; //Add the current datetime ticks to generate different value for each time
            var byteData = Encoding.ASCII.GetBytes(url);
            var md5Hash = new MD5CryptoServiceProvider().ComputeHash(byteData); //this will be 128 bits data => 32 characters
            var base62Value = md5Hash.ToBase62(); //this will be data with length > 21
            //Select first SIX characters from the base62 value
            var shortCode = base62Value.Substring(0, 6);
            _logger.LogDebug($"Generated short code - {shortCode} for url - {url} using MD5 and base62");
            return shortCode;
        }
    }
}
