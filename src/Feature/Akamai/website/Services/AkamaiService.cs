using Helixbase.Feature.Akamai.Akamai.Utilities;
using Sitecore.Data.Items;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using SC = Sitecore.Configuration;
namespace Helixbase.Feature.Akamai.Services
{
    public class AkamaiService : IAkamaiService
    {
        private readonly static AkamaiCCUWrapper wrapper = new AkamaiCCUWrapper();
        private readonly static string _rootPath = SC.Settings.GetSetting("Redirect.RootPath");
        private readonly static string _hostname = SC.Settings.GetSetting("Akamai.Hostname");
        private readonly static string _network = SC.Settings.GetSetting("Akamai.NetWork");

        public void AkamaiInvalidate(Item sourceItem)
        {
            var childPaths = GetChildPaths(sourceItem).Distinct().ToList();
            foreach (var childPath in childPaths)
            {

                InvalidateByUrl(childPath.Replace(_rootPath, ""));
            }
        }
        private   List<string> GetChildPaths(Item sourceItem)
        {
            var result = new List<string>();
            if (sourceItem != null)
            {
                result.Add(sourceItem.Paths.Path);
                var itemChildren = sourceItem.GetChildren().ToList();
                if (itemChildren.Count > 0)
                {
                    foreach (var itemNode in itemChildren)
                    {
                        if (itemNode != null)
                        {
                            var itemNodeChildren = itemNode.GetChildren().ToList();
                            if (itemNodeChildren.Count > 0)
                            {
                                result.AddRange(GetChildPaths(itemNode));
                            }
                            else
                            {
                                result.Add(itemNode.Paths.Path);
                            }
                        }
                    }
                }
            }
            return result;
        }
        public   void InvalidateByUrl(string url)
        {


            var request = new AkamaiCCUUrlRequest()
            {
                network = (AkamaiNetwork)System.Enum.Parse(typeof(AkamaiNetwork), _network),
                objects = new string[] { url },
                hostname = _hostname
            };

            var response = wrapper.InvalidateByUrl(request);



        }
    }
}