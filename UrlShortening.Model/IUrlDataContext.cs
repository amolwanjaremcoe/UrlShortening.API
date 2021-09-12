using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Text;
using UrlShortening.Model;

namespace UrlShortening.Model
{
    public interface IUrlDataContext
    {
        IMongoCollection<UrlData> UrlDatas { get; }
    }
}
