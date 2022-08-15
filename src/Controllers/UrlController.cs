using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using UrlApi.Data;

namespace UrlApi.Controllers
{
  [Route("api/[action]")]
  [ApiController]
  public class UrlController:ControllerBase
  {
      private readonly IUrlRepository _repo;
      public UrlController(IUrlRepository repo)
      {
          _repo = repo;
      }

      public async Task<string> GetShortUrl(string longUrl)
      {
         return  await _repo.Add(longUrl,null);
      }    
      public async Task<string> GetUrl(string shortUrl)
      {
         return  await _repo.GetAsync(shortUrl);
      }
      public async Task<string> GetCustomShortUrl(string longUrl, string customUrl)
      {
          return await _repo.Add(longUrl, customUrl);
      }
    }
}
