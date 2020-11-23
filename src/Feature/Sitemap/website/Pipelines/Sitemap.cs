#region Namespaces
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Xml.Serialization;
using Helixbase.Feature.Sitemap.Models;
using Sitecore;
using Sitecore.Data.Items;
using Sitecore.Diagnostics;
using Sitecore.Links;
using Sitecore.Pipelines.HttpRequest;
 
#endregion

#region Main Code
namespace Helixbase.Feature.Sitemap.Pipelines.HTTPRequestBegin
{
    public class Sitemap : HttpRequestProcessor
    {
        public override void Process(HttpRequestArgs args)
        {
            //This check will verify if the physical path of the request exists or not.
            if (!System.IO.File.Exists(args.HttpContext.Request.PhysicalPath) &&
                !System.IO.Directory.Exists(args.HttpContext.Request.PhysicalPath))
            {
                Assert.ArgumentNotNull(args, "args");
                //Check if the request is of sitemap.xml then only allow the request to serve sitemap.xml
                if (args.Url == null || !args.Url.FilePath.ToLower().EndsWith("sitemap.xml")) return;
                try
                {
                    // Homepage of the Website.
                    // Start path will give homepage including Multisite.
                    var homepage = Context.Database.GetItem(args.StartPath);
                    var ser = new XmlSerializer(typeof(Urlset));

                    var urlSet = new Urlset();

                    //Create node of Homepage in Sitemap.
                    var tmpurlset = new List<Url>();
                    var config = AppendLanguage();

                    if (!ExcludeItemFromSitemap(homepage))
                    {
                        tmpurlset.Add(new Url
                        {
                            Loc = GetAbsoluteLink(LinkManager.GetItemUrl(homepage, new UrlOptions() { LanguageEmbedding = (config == 2 ? LanguageEmbedding.Always : (config == 1 ? LanguageEmbedding.AsNeeded : LanguageEmbedding.Never)) })),
                            Lastmod = homepage.Statistics.Updated.ToString("yyyy-MM-dd hh:mm:ss")
                        });
                    }

                    // Get all decendants of Homepage to create full Sitemap.
                    var childrens = homepage.Axes.GetDescendants();
                    //Remove the items whose templateid is in exclude list
                    var finalcollection = childrens.Where(x => !ExcludeItemFromSitemap(x)).ToList();

                    tmpurlset.AddRange(finalcollection.Select(childItem => new Url
                    {
                        Loc = GetAbsoluteLink(LinkManager.GetItemUrl(childItem, new UrlOptions() { LanguageEmbedding = (config == 2 ? LanguageEmbedding.Always : (config == 1 ? LanguageEmbedding.AsNeeded : LanguageEmbedding.Never)) })),
                        Lastmod = childItem.Statistics.Updated.ToString("yyyy-MM-dd hh:mm:ss")
                    }));

                    // Populate created collection to right object
                    urlSet.Url = tmpurlset;

                    //Write XML Response for Sitemap.
                    var response = HttpContext.Current.Response;
                    response.AddHeader("Content-Type", "text/xml");
                    ser.Serialize(response.OutputStream, urlSet);
                    HttpContext.Current.Response.End();
                    //Response Ends Here
                }
                catch (Exception ex)
                {
                    Log.Error("Error - Sitemap.xml.", ex, this);
                }
            }
        }

        /// <summary>
        /// Crete Absolute url as per the site
        /// </summary>
        /// <param name="relativeUrl"></param>
        /// <returns></returns>
        private static string GetAbsoluteLink(string relativeUrl)
        {
            return HttpContext.Current.Request.Url.Scheme + "://" + HttpContext.Current.Request.Url.Host + relativeUrl;
        }

        /// <summary>
        /// Append language or not in URL to return language specific sitemap.xml
        /// </summary>
        /// <returns></returns>
        private static int AppendLanguage()
        {
            return string.IsNullOrEmpty(Sitecore.Configuration.Settings.GetSetting("LanguageEmbedForSitemap")) ? 0 : System.Convert.ToInt32((Sitecore.Configuration.Settings.GetSetting("LanguageEmbedForSitemap")));
        }

        /// <summary>
        /// This method will get a list of excluding template ids and will check if the passed item is in
        /// </summary>
        /// <param name="objItem"></param>
        /// <returns></returns>
        private static bool ExcludeItemFromSitemap(Item objItem)
        {
            //Check if the item is having any version
            if (objItem.Versions.Count > 0)
            {
                var excludeItems = Sitecore.Configuration.Settings.GetSetting("ExcludeSitecoreItemsByTemplatesInSitemap");
                var collection = excludeItems.Split(',').ToList();
                return collection.Contains(objItem.TemplateID.ToString());
            }
            return true;
        }
    }
}
#endregion