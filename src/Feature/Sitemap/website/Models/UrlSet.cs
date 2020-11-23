using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Xml.Serialization;

namespace Helixbase.Feature.Sitemap.Models
{
    ///<summary>
    /// Class to generate urlset for sitemap
    /// </summary>

    [XmlRoot("urlset")]
    public class Urlset
    {
        ///<summary>
        /// Constructor to initialize Url Object
        /// </summary>

        public Urlset() { Url = new List<Url>(); }

        ///<summary>
        /// Urls collection
        /// </summary>

        [XmlElement("url")]
        public List<Url> Url { get; set; }

    }

    ///<summary>
    /// Class to generate url with its parameters for sitemap
    /// </summary>

    public class Url
    {
        ///<summary>
        /// Location Parameter
        /// </summary>

        [XmlElement("loc")]
        public string Loc { get; set; }

        ///<summary>
        /// Last modified on
        /// </summary>

        [XmlElement("lastmod")]
        public string Lastmod { get; set; }

        //Add required properties here like changefreq, priority
    }
}