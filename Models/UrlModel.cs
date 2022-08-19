namespace UrlMaker.Models
{
    public class UrlModel
    {       
        public long Id { get;set; }
        //URL FORMAT 
        //  https://   site.com/   action=ex?id=2&name=xx
        //  protocol     Host         Path
        public string LongUrl 
        {
           get {
              return $"{Protocol}{Host}{Path}";
           }
        }
        public string ShortUrl
        {
            get
            {
                return $"{Protocol}{Host}{Key}";
            }
        }
        public string Protocol { get;set; }
        public string Host { get;set; }
        public string Path { get;set; }
        /// <summary>
        /// the short URL path
        /// </summary>
        public string Key { get; set; }

    }
}
