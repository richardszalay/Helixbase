using System.Web.Mvc;
using Helixbase.Feature.Hero.Mediators;
using Helixbase.Feature.Hero.Services;
using Sitecore.Mvc.Controllers;

namespace Helixbase.Feature.Hero.Controllers
{
    public class HeroAPIController : SitecoreController
    {
        private readonly IHeroService _heroService;

        public HeroAPIController(IHeroService heroService )
        {
            _heroService = heroService;
 
 
        }
        /// <summary>
        /// Not used, here to demonstrate routes found in RegisterRoutes.cs
        /// </summary>
        /// <returns></returns>
        public ActionResult TestAction()
        {
          var items=  _heroService.GetHeroImagesSearch();
            return Content($"This is a test {items.Content}");
        }
    }
}