using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;
using UrlMaker.Models;

namespace UrlMaker.Data
{
    public class UrlRepository : IUrlRepository
    {
        private  readonly AppDataContext _dataContext;
        public UrlRepository(AppDataContext urlContext)
        {
            _dataContext = urlContext;     
        }
        public async Task<int> DeleteUrl(UrlModel item)
        {
            _dataContext.Urls.Remove(item);
           return  await _dataContext.SaveChangesAsync();
        }
        public async Task<string> GetShortUrl(string longUrl, string customShortUrl = null)
        {
            //if url exists , delete it
            try
            {
                if (customShortUrl != null)
                {
                    if (customShortUrl.Length != 6)
                    {
                       throw new Exception("SHORT URL MUST BE 6 CHARACTERS LENGTH");                        
                    }
                    // case insensitive
                    UrlModel existingUrlKey = _dataContext.Urls.FirstOrDefault(u => u.Key == customShortUrl);
                    // case sensitive
                    if(existingUrlKey != null && existingUrlKey.Key==customShortUrl)
                    {
                       throw new Exception("SHORT URL NOT AVAILABLE");
                    }
                    UrlModel m = ParseUrl(longUrl);
                    existingUrlKey = _dataContext.Urls.FirstOrDefault(u => u.Host == m.Host && u.Path==m.Path && u.Protocol==m.Protocol);
                    if (existingUrlKey!=null)
                    {
                         existingUrlKey.Key = customShortUrl;
                        _dataContext.Urls.Update(existingUrlKey);
                        await _dataContext.SaveChangesAsync();
                        return existingUrlKey.ShortUrl;
                    }
                    else
                    {
                       UrlModel customModel = ParseUrl(longUrl);                        
                       customModel.Key = customShortUrl;
                       await _dataContext.Urls.AddAsync(customModel);
                       await _dataContext.SaveChangesAsync();
                        return customModel.ShortUrl;
                    }
                }

                UrlModel model = ParseUrl(longUrl);                        
                string newKey =await GetNewKey();
                //to avoid key duplication
                while (_dataContext.Urls.FirstOrDefault(m => m.Key == newKey) != null)
                {
                    newKey=await GetNewKey();
                }
                model.Key = newKey;
                UrlModel oldUrl = _dataContext.Urls.FirstOrDefault(u => u.Protocol == model.Protocol && u.Host == model.Host && u.Path == model.Path);
                if (oldUrl != null)
                {
                    oldUrl.Key = newKey;
                    _dataContext.Urls.Update(oldUrl);
                    await _dataContext.SaveChangesAsync();

                }
                else
                {
                    await _dataContext.Urls.AddAsync(model);
                    await _dataContext.SaveChangesAsync();
                }
                return model.ShortUrl;
            }catch(Exception ex)
            {
                throw ex;
            }
            
        }
        //helper methods
        public UrlModel ParseUrl(string longUrl,bool isShort=false)
        {
            try
            {
               UrlModel urlModel = new UrlModel();
               int len=longUrl.IndexOf("://")+3;
               
               urlModel.Protocol = longUrl.Substring(0, len);
               string url=longUrl.Substring(len);
                if (url.Length < 5)
                {
                    throw new Exception("NOT A VALID URL");
                }
                len = url.IndexOf('/')+1;
               urlModel.Host = url.Substring(0,len);
               urlModel.Path=url.Substring(len);
               if(isShort==false && urlModel.Path.Length < 7)
               {
                   throw new Exception("GIVEN URL IS IN SHORT FORMAT");
               }
               return urlModel;
            }catch(Exception ex)
            {
                throw new Exception("INVALID URL FORMAT");
            }
        }

        public async Task<string> GetNewKey()
        {
            KeyModel key =await _dataContext.Keys.FirstAsync<KeyModel>();
            _dataContext.Keys.Remove(key);
            _dataContext.SaveChanges();
            return key.Key;
        }


        public async Task<string> GetLongUrl(string shortUrl)
        {
            //UrlModel model = ParseUrl(shortUrl,true);
            //UrlModel urlModel = await _dataContext.Urls.FirstOrDefaultAsync(m=>m.ShortUrl==shortUrl);
            UrlModel model = ParseUrl(shortUrl,true);
            UrlModel urlModel = await _dataContext.Urls.FirstOrDefaultAsync(m=>m.Key==model.Path && m.Host==model.Host && m.Protocol==model.Protocol);
            if (urlModel != null)
            {
                return urlModel.LongUrl;
            }
            return "NOT FOUND";
        }
    }
}
