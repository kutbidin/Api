using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace UrlApi.Data
{
  public class KeyRepository : IKeyRepository
  {
    private readonly KeyContext _context;

    public KeyRepository(KeyContext context)
        {
            _context = context;
        }

        public async Task<string> Add(KeyModel data) 
    {
       IQueryable<KeyModel> query = _context.Keys.Include(c => c.Key);
       KeyModel  testKey=query.Where(c => c.Key == data.Key).FirstOrDefault();
       if (testKey == null)
       {
          await _context.AddAsync(data);
                _context.SaveChanges();
          return data.Key;
       }
       //failed to add new url key
       return null;
    }

    public void Delete(KeyModel data)
    {
      _context.Remove(data);
    }

    public async Task<bool> SaveChangesAsync()
    {
       return (await _context.SaveChangesAsync()) > 0;
    }

    // get the original url from shote url key
    public async Task<string> GetAsync(string key)
    {
      IQueryable<KeyModel> query = _context.Keys
          .Include(c => c.Key);
      KeyModel model = await query.Where(c => c.Key == key).FirstOrDefaultAsync();
      Delete(model);  // remove the assigned keys from key pool
      return model.Key;
    }

        public Task<string[]> GetNextGroupOfKeys(int count)
        {
            throw new NotImplementedException();
        }

    }
}
