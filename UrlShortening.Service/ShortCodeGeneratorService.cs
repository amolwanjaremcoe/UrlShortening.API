using System;
using System.Collections.Generic;
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
        public async Task<string> GenerateShortCode()
        {
            return string.Empty;
        }
    }
}
