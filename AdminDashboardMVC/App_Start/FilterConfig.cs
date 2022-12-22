using AdminDashboardMVC.App_Start;
using System.Web;
using System.Web.Mvc;

namespace AdminDashboardMVC
{
    public class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleErrorAttribute());
            filters.Add(new HandleAntiforgeryTokenErrorAttribute() { ExceptionType = typeof(HttpAntiForgeryException) } );
        }
    }
}
