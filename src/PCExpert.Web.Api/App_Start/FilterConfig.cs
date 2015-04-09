using System.Diagnostics.Contracts;
using System.Web;
using System.Web.Mvc;
using PCExpert.Web.Api.Filters;

namespace PCExpert.Web.Api
{
    public class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
			Contract.Requires(filters != null);

            filters.Add(new HandleErrorAttribute());
#if !DEBUG
			filters.Add(new PCExpertExceptionFilter());
#endif
        }
    }
}
