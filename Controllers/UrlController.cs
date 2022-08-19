using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;
using UrlMaker.Data;

namespace UrlMaker.Controllers
{
    [ApiController]
    [Route("api/[controller]/[action]")]
    public class UrlController : ControllerBase
    {

        private readonly ILogger<UrlController> _logger;
        private readonly IUrlRepository _urlRepo;
        public UrlController(ILogger<UrlController> logger,IUrlRepository urlrepo)
        {
            _logger = logger;
            _urlRepo = urlrepo;
        }
        public async Task<string> GetShortUrl(string longUrl,string shortUrl)
        {
            _logger.LogInformation($"requested long url :{longUrl}");
            string res= (await _urlRepo.GetShortUrl(longUrl, shortUrl));
            _logger.LogInformation($"responsed short url :{shortUrl}");
            return res;
        } 
        public async Task<string> GetLongUrl(string shortUrl)
        {
            _logger.LogInformation($"requested short url :{shortUrl}");
             string res=(await _urlRepo.GetLongUrl(shortUrl));
            _logger.LogInformation($"redirect url :{shortUrl}");
            return res;
        }
    }
}
