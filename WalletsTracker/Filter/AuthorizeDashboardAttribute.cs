using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Routing;

namespace WalletsTracker.Filter
{
    public class AuthorizeDashboardAttribute: ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            base.OnActionExecuting(context);
            if (string.IsNullOrEmpty(context.HttpContext.Session.GetString("UserIdKeyWalletApp")))
            {
                context.Result = new RedirectToRouteResult(
                    new RouteValueDictionary {
                        { "controller", "Home" },
                        { "action", "Login" }
                    });
            }
        }
    }
}
