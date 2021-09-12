﻿using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace UrlShortening.Service
{
    public class UrlValidationService : IUrlValidationService
    {
        private readonly IHttpClientFactory _httpClientFactory;

        public UrlValidationService(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        public async Task<bool> IsUrlValid(string url)
        {
            Uri uriResult;
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
                catch (Exception)
                {
                    return false;
                }
            }
            return result;
        }
    }
}