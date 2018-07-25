using System.Web;
using System.Web.Mvc;

namespace CreditSuisse.QIS.VendingMachine
{
    public class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleErrorAttribute());
        }
    }
}
