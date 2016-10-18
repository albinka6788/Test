using BHIC.DML.WC.DTO;
using BHIC.Domain.LP;
using System.Collections.Generic;
using System.Data;

namespace BHIC.Contract.LP
{
    public interface ILandingPageService
    {
        List<LineOfBusiness> GetAllLineOfBusiness();
        long CreateLandingPage(LandingPageTransaction landingPageTransaction, long userId);
        LandingPageTransaction GetLandingPage(string AdId);
        List<LandingPageTransaction> GetLandingPages(string filter);
        LandingPageTransaction GetLandingPage(long Id);
        void UpdateTransactionCounter(string tokenId);
        bool InsertOrUpdateCTAMsgs(List<CTAMessage> lIstCTAMessage, string TokenId, long userId);
        bool InsertOrUpdateTemplateLogo(string template, string logo);
        long Authentication(string username, string password);
        DataSet GetDefaultMasterData();
        bool DeleteLandingPages(string ids, long userId);
    }
}
