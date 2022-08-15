using System.Threading.Tasks;

namespace UrlApi.Data
{
    public interface IKeyRepository
    {
       Task<string> Add(KeyModel key);
       void Delete(KeyModel key);
       Task<bool> SaveChangesAsync();
       Task<string> GetAsync(string key);
       Task<string[]> GetNextGroupOfKeys(int count);
       
  }
}