using System.Net;
using System.Net.Http;
using System.Web.Http.Controllers;
using System.Web.Http.Filters;
using Sitecore;


namespace Helixbase.Feature.Analytics.Filter
{
    //[AuthorizedUser(@"sitecore\ServicesAPI")]
   
    public class AuthorizedUser : AuthorizationFilterAttribute
    {
        private readonly string _user;

        public AuthorizedUser(string user)
        {
            _user = user;
        }
        public override void OnAuthorization(HttpActionContext actionContext)
        {
            base.OnAuthorization(actionContext);
            var context = Context.User;

            if ((context.IsAuthenticated && context.Name.Equals(_user)))
                return;

            actionContext.Response =
                actionContext.ControllerContext.Request.CreateErrorResponse(HttpStatusCode.Unauthorized,
                    "Unauthorized Access; User is " + Context.User.LocalName);
        }
    }
}
