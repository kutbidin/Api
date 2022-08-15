using System.Threading.Tasks;

namespace UrlApi.Data
{
    public interface IUrlRepository
  {
    Task<string> Add(string data,string customData);
    void Delete(UrlDbModel data);
    Task<bool> SaveChangesAsync();
    Task<string> GetAsync(string key);

  }
}