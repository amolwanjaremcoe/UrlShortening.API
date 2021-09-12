using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace UrlShortening.Service
{
    public interface IShortCodeGeneratorService
    {
        public Task<string> GenerateShortCode(string url);
    }
}
