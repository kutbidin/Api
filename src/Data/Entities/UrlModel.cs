
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace UrlApi.Data
{
  public class UrlModel
  {
    [Key]
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
        /// the url for view
        /// </summary>
    public string Url
    {
        get { return $"{Protocol}{Host}{Path}"; }
    }


  }
}