using System.Web;
using System.Web.Mvc;

namespace ToDoList
{
    public class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleErrorAttribute());

            // this requires the user to login as a default in order to access every page. Relevant pages can then be whitelisted. 
            filters.Add(new AuthorizeAttribute());
        }
    }
}
