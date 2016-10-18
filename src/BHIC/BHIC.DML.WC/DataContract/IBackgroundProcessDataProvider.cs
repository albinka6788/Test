using BHIC.DML.WC.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BHIC.DML.WC.DataContract
{
    public interface IBackgroundProcessDataProvider
    {
        List<UserQuoteDTO> GetInactiveUserQuote();
        bool UpdateSaveForLaterMailStatus(int UserId);
    }
}
