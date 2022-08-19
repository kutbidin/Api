using System.Threading.Tasks;
using UrlMaker.Models;

namespace UrlMaker.Data
{
    public interface IUrlRepository
    {
        Task<string> GetShortUrl(string longUrl, string customShortUrl = null);
        Task<string> GetLongUrl(string shortUrl);
        Task<int> DeleteUrl(UrlModel item);
    }
}
