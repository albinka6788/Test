using BHIC.Common.CommonUtilities;
using BHIC.Common.Trace;
using System.Web.Mvc;

namespace BHIC.Portal.LP.App_Start
{
    public class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            //Comment : Here Application custom action filters
            filters.Add(new TransactionLogAttribute());
            filters.Add(new PageDisableCacheAttribute());
        }
    }
}