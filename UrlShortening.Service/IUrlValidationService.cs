using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace UrlShortening.Service
{
    public interface IUrlValidationService
    {
        public Task<bool> IsUrlValid(string url);
    }
}
