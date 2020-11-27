using System.Web.Http;
using Sitecore;
using Helixbase.Feature.Analytics.Models;
using Helixbase.Feature.Analytics.Services;
using Sitecore.Services.Core;
using Sitecore.Services.Infrastructure.Web.Http;
using System.Net.Http;
using System.Threading.Tasks;
using System.Text;
using Sitecore.Mvc.Controllers;
using System.Web.Mvc;
using System.Net;

namespace Helixbase.Feature.Analytics.Controllers
{
    public class AnalyticsController : SitecoreController
    {
        private readonly IAnalyticsService _analyticsService;

        public AnalyticsController(IAnalyticsService analyticsService)
        {

            _analyticsService = analyticsService;


        }

    
      
        public ActionResult SayHello()
        {
            



            return Content($"This is a test");
        }


        
        

 
        public ActionResult EndSession()
        {
           

            System.Web.HttpContext.Current.Session.Abandon();



            return Content($"EndSession");
        }

 
    }
}
