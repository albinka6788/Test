using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BHIC.Contract.LP;
using BHIC.Domain.LP;
using BHIC.DML.WC.DataContract;
using BHIC.DML.WC.DataService;
using BHIC.DML.WC.DTO;
namespace BHIC.Core.LP
{
    public class LandingPageService : ILandingPageService
    {
        ILandingPageDataService landingPageDataService = new LandingPageDataService();
        ILineOfBusinessProvider lineOfBusinessProvider = new LineOfBusinessProvider();
        public long CreateLandingPage(LandingPageTransaction landingPageTransaction, long userId)
        {
            return landingPageDataService.AddEditLandingPage(landingPageTransaction, userId);
        }


        public LandingPageTransaction GetLandingPage(string AdId)
        {
            return landingPageDataService.GetLandingPage(AdId);
        }

        public List<LandingPageTransaction> GetLandingPages(string filter)
        {
            return landingPageDataService.GetLandingPages(filter);
        }


        public LandingPageTransaction GetLandingPage(long Id)
        {
            return landingPageDataService.GetLandingPage(Id);
        }

        public void UpdateTransactionCounter(string tokenId)
        {
            landingPageDataService.UpdateTransactionCounter(tokenId);
        }

        public bool InsertOrUpdateCTAMsgs(List<CTAMessage> ctaMessage, string tokenId, long userId)
        {
            return landingPageDataService.InsertOrUpdateCTAMsgs(ctaMessage, tokenId, userId);
        }


        public bool InsertOrUpdateTemplateLogo(string template, string logo)
        {
            return landingPageDataService.InsertOrUpdateTemplateLogo(template, logo);
        }


        public long Authentication(string username, string password)
        {
            return landingPageDataService.Authentication(username, password);
        }

        public System.Data.DataSet GetDefaultMasterData()
        {
            return landingPageDataService.GetDefaultMasterData();
        }

        public List<LineOfBusiness> GetAllLineOfBusiness()
        {
            return lineOfBusinessProvider.GetAllLineOfBusiness();

        }

        public bool DeleteLandingPages(string ids, long userId)
        {
            return landingPageDataService.DeleteLandingPages(ids, userId);
        }
    }
}
