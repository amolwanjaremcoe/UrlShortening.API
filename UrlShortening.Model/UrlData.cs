using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Text.Json.Serialization;

namespace UrlShortening.Model
{
    public class UrlData
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string InternalId { get; set; }
        public string ShortCode { get; set; }
        public string OriginalUrl { get; set; }
        public DateTime CreationDate { get; set; }
        public DateTime ExpirationDate { get; set; }
    }
}
