using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Text;

namespace UrlShortening.Model
{
    public class UrlDataContext : IUrlDataContext
    {
        private readonly IMongoDatabase _db;
        public UrlDataContext(MongoDBConfig config)
        {
            var client = new MongoClient(config.ConnectionString);
            _db = client.GetDatabase(config.Database);
        }
        public IMongoCollection<UrlData> UrlDatas => _db.GetCollection<UrlData>("url");
    }
}
