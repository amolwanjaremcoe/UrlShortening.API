using Base62;
using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using UrlShortening.DataAccess;

namespace UrlShortening.Service
{
    public class ShortCodeGeneratorService : IShortCodeGeneratorService
    {
        /// <summary>
        /// Generate and return the SIX digit unique short code
        /// </summary>
        /// <returns></returns>
        public async Task<string> GenerateShortCode(string url)
        {
            url = $"{url}{DateTime.Now.Ticks}"; //Add the current datetime ticks to generate different value for each time
            var byteData = Encoding.ASCII.GetBytes(url);
            var md5Hash = new MD5CryptoServiceProvider().ComputeHash(byteData); //this will be 128 bits data => 32 characters
            var base62Value = md5Hash.ToBase62(); //this will be data with length > 21
            //Select first characters
            return base62Value.Substring(0, 6);
        }
    }
}
