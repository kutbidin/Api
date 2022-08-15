
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace UrlApi.Data
{
  public class UrlDbModel
  {
        /// <summary>
        /// the shortedned url
        /// </summary>
        [Key]
        public long Id { get; set; }
        
        public string Key { get; set; }
        /// <summary>
        /// URL protocol part http ,https etc
        /// </summary>
        public string Protocol { get; set; }
        /// <summary>
        /// host name
        /// </summary>
        public string Host { get; set; }
        /// <summary>
        /// the absolute path for the site
        /// </summary>
        public string Path { get; set; }

        /// <summary>
        /// for access counts statistics
        /// </summary>
        //public long Counter  { get; set; }
        public string Url
        {
            get { return $"{Protocol}{Host}{Path}"; }
        }

    }
}