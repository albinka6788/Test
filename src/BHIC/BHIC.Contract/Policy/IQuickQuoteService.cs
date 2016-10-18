using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using BHIC.Domain.Policy;

namespace BHIC.Contract.Policy
{
    public interface IQuickQuoteService
    {
        /// <summary>
        /// Return Premium Amt based on available-carrier and exposure-data using QuickQuote
        /// </summary>
        /// <param name="args"></param>
        /// <returns></returns>
        decimal GetQuickQuotePremium(QuickQuoteRequestParms args);
    }
}
