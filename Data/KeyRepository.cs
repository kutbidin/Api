using System.Threading.Tasks;
using UrlMaker.Models;
using System.Linq;
using System;
using Microsoft.EntityFrameworkCore;

namespace UrlMaker.Data
{
    public class KeyRepository : IKeyRepository
    {
        private readonly AppDataContext _dataContext;
        public KeyRepository(AppDataContext dataContext)
        {
            _dataContext = dataContext;
        }

        public async Task<string> AddKey(KeyModel key)
        {
            if (_dataContext.Keys.FirstOrDefault(k => k.Key == key.Key) != null)
            {
                throw new Exception("SAME KEY EXISTS");
            }
            await _dataContext.AddAsync(key);
            await _dataContext.SaveChangesAsync();
            return key.Key;
        }

        public async Task<bool> DeleteKey(string key)
        {
            KeyModel keyModel=await _dataContext.Keys.FirstOrDefaultAsync(k=>k.Key == key);
            if (keyModel != null)
            {
                _dataContext.Remove(keyModel);
                return true;
            }
            return false;
        }

        public async void GenerateNewKey(int count)
        {
            KeyModel[] keys=new KeyModel[count];
            for(int i = 0; i < count; i++)
            {
                keys[i] = new KeyModel();
                keys[i].Key=Guid.NewGuid().ToString().Substring(0,6);
            }
            await _dataContext.AddRangeAsync(keys);
            await _dataContext.SaveChangesAsync();
        }

        //return the first key from collection,and deletes it from keys table afterwards
        public async Task<string> GetNewKey()
        {
            KeyModel key=_dataContext.Keys.First();
            _dataContext.Remove(key);
            await _dataContext.SaveChangesAsync();
            return key.Key;
            
        }
    }
}
