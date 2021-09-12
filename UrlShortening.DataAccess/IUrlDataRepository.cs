using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using UrlShortening.Model;

namespace UrlShortening.DataAccess
{
    public interface IUrlDataRepository
    {
        // api/[GET]
        Task<IEnumerable<UrlData>> GetAllUrlDatasAsync();

        // api/[POST]
        Task CreateAsync(UrlData urlData);

        // api/abc/[DELETE]
        Task<bool> DeleteAsync(string shortCode);

        Task<UrlData> GetUrlDataAsync(string shortCode);
    }
}
