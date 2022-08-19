using System.Threading.Tasks;
using UrlMaker.Models;

namespace UrlMaker.Data
{
    public interface IKeyRepository
    {
        Task<string> GetNewKey();
        Task<string> AddKey(KeyModel key);
        Task<bool> DeleteKey(string key);
        void GenerateNewKey(int count);
    }
}
