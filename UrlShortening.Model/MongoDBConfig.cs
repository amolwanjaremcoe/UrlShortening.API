using System;
using System.Collections.Generic;
using System.Text;

namespace UrlShortening.Model
{
    public class MongoDBConfig
    {
        public string Database { get; set; }
        public string Host { get; set; }
        public int Port { get; set; }
        //public string ConnectionString
        //{
        //    get
        //    {
        //        return $@"mongodb://{Host}:{Port}";
        //    }
        //}
    }
}
