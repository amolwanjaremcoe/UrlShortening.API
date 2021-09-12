using Microsoft.Extensions.Caching.Distributed;
using MongoDB.Bson;
using MongoDB.Driver;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using UrlShortening.Model;

namespace UrlShortening.DataAccess
{
    public class UrlDataRepository : IUrlDataRepository
    {
        private readonly IUrlDataContext _context;
        private readonly IDistributedCache _cache;

        public UrlDataRepository(IUrlDataContext context, IDistributedCache cache)
        {
            _context = context;
            _cache = cache;
        }

        // api/[GET]
        public async Task<IEnumerable<UrlData>> GetAllUrlDatasAsync()
        {
            return await _context
                                .UrlDatas
                                .Find(_ => true)
                                .ToListAsync();
        }

        public async Task CreateAsync(UrlData urlData)
        {
            urlData.CreationDate = DateTime.UtcNow;           
            await _context.UrlDatas.InsertOneAsync(urlData);
        }

        public async Task<bool> DeleteAsync(string shortCode)
        {
            await _cache.RemoveAsync(shortCode);

            FilterDefinition<UrlData> filter = Builders<UrlData>.Filter.Eq(m => m.ShortCode, shortCode);
            DeleteResult deleteResult = await _context
                                                .UrlDatas
                                              .DeleteOneAsync(filter);
            return deleteResult.IsAcknowledged
                && deleteResult.DeletedCount > 0;
        }

        /// <summary>
        /// Check if value is present in the cache if yes then return
        /// Else 
        /// </summary>
        /// <param name="shortCode"></param>
        /// <returns></returns>
        public async Task<UrlData> GetUrlDataAsync(string shortCode)
        {
            var urlByteData = await _cache.GetAsync(shortCode);
            if (urlByteData != null)
            {
                var jsonData = Encoding.ASCII.GetString(urlByteData);
                return JsonConvert.DeserializeObject<UrlData>(jsonData);                
            }
            else
            {
                FilterDefinition<UrlData> filter = Builders<UrlData>.Filter.Eq(m => m.ShortCode, shortCode);
                var urlData = await _context
                        .UrlDatas
                        .Find(filter)
                        .FirstOrDefaultAsync();

                if (urlData != null)
                {
                    //Save in the cache
                    var jsonString = JsonConvert.SerializeObject(urlData);
                    var byteData = Encoding.ASCII.GetBytes(jsonString);
                    await _cache.SetAsync(urlData.ShortCode, byteData);
                }
                return urlData;
            }
        }
    }
}
