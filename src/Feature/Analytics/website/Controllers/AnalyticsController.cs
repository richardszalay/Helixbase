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
using Sitecore.Diagnostics;
using Microsoft.AspNetCore.Mvc;

namespace Helixbase.Feature.Analytics.Controllers
{
    public class AnalyticsController : SitecoreController
    {
        private readonly IAnalyticsService _analyticsService;

        public AnalyticsController(IAnalyticsService analyticsService)
        {

            _analyticsService = analyticsService;


        }

    
      
        public string SayHello()
        {
            



            return $"This is a test";
        }
 
        [System.Web.Http.HttpPost]
        public async Task<string> IdentifyContact(ContactDetails data)
        {
            Assert.IsNotNull(data, "ContactDetails is null");

           var result  = await _analyticsService.IdentifyContact(data);
            return result==null ?null : result.Identifier;
        }





            public string EndSession()
        {

            System.Web.HttpContext.Current.Session.Abandon();
            return $"EndSession";
        }

 
    }
}
