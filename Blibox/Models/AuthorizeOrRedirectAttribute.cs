using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace Blibox
{
    /// <summary>
    /// Authorize or redirect to an unauthorized MVC action if the user does not have the required roles
    /// (an unauthenticated user will be redirected to the defualt sign in action)
    /// <para>Decorate an action or a controller like this [AuthorizeAndRedirect(Roles = "RoleName")]</para>
    /// </summary>
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, Inherited = true, AllowMultiple = false)]
    public class AuthorizeOrRedirectAttribute : System.Web.Mvc.AuthorizeAttribute
    {
        protected override void HandleUnauthorizedRequest(AuthorizationContext filterContext)
        {
            base.HandleUnauthorizedRequest(filterContext);

            if (filterContext.RequestContext.HttpContext.User.Identity.IsAuthenticated)
            {
                var routeData = new RouteData();
                routeData.Values.Add("controller", "Error");
                routeData.Values.Add("action", "Unauthorized");
                filterContext.Result = new RedirectToRouteResult(routeData.Values);
            }
        }
    }
}