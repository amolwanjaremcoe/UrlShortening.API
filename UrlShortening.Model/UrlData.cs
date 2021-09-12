using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;

namespace UrlShortening.Model
{
    public class UrlData
    {
        [BsonId]
        public ObjectId InternalId { get; set; }
        public string ShortCode { get; set; }
        public string OriginalUrl { get; set; }
        public DateTime CreationDate { get; set; }
        public DateTime ExpirationDate { get; set; }
    }
}
