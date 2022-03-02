using System.Web;
using System.Web.Mvc;

namespace Cuong_WebsiteTimPhongTro
{
    public class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleErrorAttribute());
        }
    }
}
