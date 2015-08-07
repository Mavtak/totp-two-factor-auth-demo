using System.Web.Mvc;
using System.Web.Routing;
using TotpAuthenticationExample.Data;

namespace TotpAuthenticationExample
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            RepositorySingletons.Initialize();
        }
    }
}
