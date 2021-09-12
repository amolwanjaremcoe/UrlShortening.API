using MongoDB.Bson;
using MongoDB.Driver;
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
        public UrlDataRepository(IUrlDataContext context)
        {
            _context = context;
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
            await _context.UrlDatas.InsertOneAsync(urlData);
        }

        public async Task<bool> DeleteAsync(string shortCode)
        {
            FilterDefinition<UrlData> filter = Builders<UrlData>.Filter.Eq(m => m.ShortCode, shortCode);
            DeleteResult deleteResult = await _context
                                                .UrlDatas
                                              .DeleteOneAsync(filter);
            return deleteResult.IsAcknowledged
                && deleteResult.DeletedCount > 0;
        }
        public async Task<UrlData> GetUrlDataAsync(string shortCode)
        {
            FilterDefinition<UrlData> filter = Builders<UrlData>.Filter.Eq(m => m.ShortCode, shortCode);
            return await _context
                    .UrlDatas
                    .Find(filter)
                    .FirstOrDefaultAsync();
        }
    }
}
