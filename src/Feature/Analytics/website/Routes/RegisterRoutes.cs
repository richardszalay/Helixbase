using System.Web;
using System.Web.Http;
using System.Web.Http.WebHost;
using System.Web.Mvc;
using System.Web.Routing;
using System.Web.SessionState;
using Sitecore.Pipelines;

namespace Helixbase.Feature.Analytics.Routes
{
    public class SessionRouteHandler : IRouteHandler
    {
        IHttpHandler IRouteHandler.GetHttpHandler(RequestContext requestContext)
        {
            return new SessionControllerHandler(requestContext.RouteData);
        }
    }

    public class SessionControllerHandler : HttpControllerHandler, IRequiresSessionState
    {
        public SessionControllerHandler(RouteData routeData)
            : base(routeData)
        {
        }
    }
    public class RegisterRoutesBase
    {

        protected static void MapRouteWithSession(HttpConfiguration configuration, string routeName, string routePath, string controller, string action)
        {
            var routes = configuration.Routes;
            routes.MapHttpRoute(routeName, routePath, new
            {
                controller = controller,
                action = action
            });

            var route = System.Web.Routing.RouteTable.Routes[routeName] as System.Web.Routing.Route;
            route.RouteHandler = new SessionRouteHandler();
        }
    }
    public class RegisterAnalyticsRoutes : RegisterRoutesBase
    {
        public void Process(PipelineArgs args)
        {

            GlobalConfiguration.Configure(this.Configure);
        }

        protected void Configure(HttpConfiguration configuration)
        {
            configuration.MapHttpAttributeRoutes();
           // MapRouteWithSession(configuration, "SF.Analytics", "api/sf/1.0/analytics/{action}", "Analytics", "index");
        }
    }
    public class RegisterRoutes
    {
 
        public void Process(PipelineArgs args)
        {
            RouteTable.Routes.MapRoute("Feature.Analytics", "api/sitecore/analytics/{action}", new
            {
                controller = "Analytics",

            });

        }
 

       
    }
}