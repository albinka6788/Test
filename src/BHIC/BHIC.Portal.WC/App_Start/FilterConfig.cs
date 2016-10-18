using System.Web.Mvc;

using BHIC.Common.CommonUtilities;
using BHIC.Common.Trace;


namespace BHIC.Portal.WC
{
    public class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            //Comment : Here Application custom action filters
            //filters.Add(new CheckAppSessionCookieAttribute());
            filters.Add(new PageDisableCacheAttribute());
            //filters.Add(new CompressFilter());

            filters.Add(new HandleErrorAttribute());
            filters.Add(new TransactionLogAttribute());

        }
    }
}
