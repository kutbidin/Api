using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace UrlApi.Data
{
  public class UrlRepository : IUrlRepository
  {
     private readonly AppDataContext _dataContext;

    public UrlRepository(AppDataContext context)
    {
         _dataContext = context;
    }

    public async Task<string> Add(string longUrl,string custom=null) 
    {
       //IQueryable<UrlDbModel> query = _dataContext.Urls.Include(c => c.Key);
       UrlDbModel dbModel = ParseUrl(longUrl);
       dbModel.Key =await GetNewKey(custom);
        // delete old entries for the same url
       UrlDbModel oldModel = _dataContext.Urls.
           FirstOrDefault(c => c.Path == dbModel.Path 
                            && c.Host == dbModel.Host 
                            && c.Protocol == dbModel.Protocol);       
       if (oldModel != null)
       {
                _dataContext.Urls.Remove(oldModel);
                _dataContext.SaveChanges();

       }
       UrlDbModel model = _dataContext.Urls.FirstOrDefault(c => c.Key == dbModel.Key);
       if (model == null)
       {
          await _dataContext.Urls.AddAsync(dbModel);
                _dataContext.SaveChanges();
       }
          //return the shortened URL as well
       return $"{dbModel.Protocol}{dbModel.Host}{dbModel.Key}";

    }

    public void Delete(UrlDbModel data)
    {
      _dataContext.Urls.Remove(data);
    }

    public async Task<bool> SaveChangesAsync()
    {
       return (await _dataContext.SaveChangesAsync()) > 0;
    }

    // get the original url from shote url key
       public async Task<string> GetAsync(string shortUrl)
    {
       UrlDbModel model = ParseUrl(shortUrl,true);
       return (await _dataContext.Urls.FirstOrDefaultAsync(m=>m.Host==model.Host && m.Key==model.Path && m.Protocol==model.Protocol)).Url;
    }
        // helper methods
        public UrlDbModel ParseUrl(string longUrl,bool isShort=false)
        {
            try
            {
                longUrl = longUrl.ToLower();
                UrlDbModel model = new UrlDbModel();
                model.Protocol = longUrl.Substring(0, longUrl.IndexOf("://")+3);
                string tempUrl = longUrl.Substring(model.Protocol.Length);
                if (tempUrl.Length < 5)
                {
                    throw new Exception("NOT A VALID URL");
                }
                model.Host = tempUrl.Substring(0, tempUrl.IndexOf("/")+1);
                model.Path = tempUrl.Substring(model.Host.Length);
                if (model.Path.Length < 7 && isShort==false)
                {
                    throw new Exception("GIVEN URL IS IN SHORT FORMAT");
                }
                return model;
            }
            catch (Exception ex)
            {
                throw new Exception("INVALID URL FORMAT");
            }

        }
        public async Task<string> GetNewKey(string custom)
        {
            if (custom!=null)
            {
                if(custom.Length==6)
                {
                    if (await _dataContext.Keys.AnyAsync(c => c.Key == custom)) {
                        return "CUSTOM URL NOT AVAILABLE";
                    }
                    return custom;
                }
                return "CUSTOM URL MUST BE 6 CAHRACTERS LENGTH";
            }
            else
            {
                KeyModel keyModel = await _dataContext.Keys.FirstAsync<KeyModel>();
                _dataContext.Keys.Remove(keyModel);
                _dataContext.SaveChanges();
                return keyModel.Key;
            }
        }

    }
}
