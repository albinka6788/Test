using BHIC.DML.WC.DTO;
using BHIC.Domain.LP;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BHIC.DML.WC.DataContract
{
    public interface ILandingPageDataService :  ILineOfBusinessProvider
    {
        long AddEditLandingPage(LandingPageTransaction landingPageTransaction, long userId);
        LandingPageTransaction GetLandingPage(string AdId);
        List<LandingPageTransaction> GetLandingPages(string filter);
        LandingPageTransaction GetLandingPage(long Id);
        void UpdateTransactionCounter(string tokenId);
        bool InsertOrUpdateCTAMsgs(List<CTAMessage> ctaMessage, string tokenId, long userId);
        bool InsertOrUpdateTemplateLogo(string template, string logo);
        long Authentication(string username, string password);
        DataSet GetDefaultMasterData();
        bool DeleteLandingPages(string ids, long userId);
    }
}
