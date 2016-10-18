using BHIC.Domain.CommercialAuto;
using BHIC.ViewDomain;
using BHIC.ViewDomain.Landing;
using System.Collections.Generic;
using System.Web.Mvc;

namespace BHIC.Portal.WC.Areas.PurchasePath.Controllers
{
    public class NavigationController : BaseController
    {
        public List<NavigationModel> GetProgressBarLinks(int flag)
        {
            List<NavigationModel> links = new List<NavigationModel>();
            switch (flag)
            {
                case 1:
                    {
                        links.Add(new NavigationModel { ClassName = "step1 complete", DisplayText = "Business Info", NavigationLink = "/GetExposureDetails", Href = "javascript:void(0)" });
                        links.Add(new NavigationModel { ClassName = "step2 ", DisplayText = "Business Details", NavigationLink = "#", Href = string.Empty });
                        links.Add(new NavigationModel { ClassName = "step3 ", DisplayText = "Your Quote", NavigationLink = "#", Href = string.Empty });
                        links.Add(new NavigationModel { ClassName = "step4 ", DisplayText = "Contact Info", NavigationLink = "#", Href = string.Empty });
                        links.Add(new NavigationModel { ClassName = "step5 ", DisplayText = "Buy", NavigationLink = "#", Href = string.Empty });
                        break;
                    }
                case 2:
                    {
                        links.Add(new NavigationModel { ClassName = "step1 complete", DisplayText = "Business Info", NavigationLink = "/GetExposureDetails", Href = "javascript:void(0)" });
                        links.Add(new NavigationModel { ClassName = "step2 complete", DisplayText = "Business Details", NavigationLink = "/GetQuestions", Href = "javascript:void(0)" });
                        links.Add(new NavigationModel { ClassName = "step3 ", DisplayText = "Your Quote", NavigationLink = "#", Href = string.Empty });
                        links.Add(new NavigationModel { ClassName = "step4 ", DisplayText = "Contact Info", NavigationLink = "#", Href = string.Empty });
                        links.Add(new NavigationModel { ClassName = "step5 ", DisplayText = "Buy", NavigationLink = "#", Href = string.Empty });
                        break;
                    }
                case 3:
                    {
                        links.Add(new NavigationModel { ClassName = "step1 complete", DisplayText = "Business Info", NavigationLink = "/GetExposureDetails", Href = "javascript:void(0)" });
                        links.Add(new NavigationModel { ClassName = "step2 complete", DisplayText = "Business Details", NavigationLink = "/GetQuestions", Href = "javascript:void(0)" });
                        links.Add(new NavigationModel { ClassName = "step3 complete", DisplayText = "Your Quote", NavigationLink = "/QuoteSummary", Href = "javascript:void(0)" });
                        links.Add(new NavigationModel { ClassName = "step4 ", DisplayText = "Contact Info", NavigationLink = "#", Href = string.Empty });
                        links.Add(new NavigationModel { ClassName = "step5 ", DisplayText = "Buy", NavigationLink = "#", Href = string.Empty });
                        break;
                    }
                case 4:
                    {
                        links.Add(new NavigationModel { ClassName = "step1 complete", DisplayText = "Business Info", NavigationLink = "/GetExposureDetails", Href = "javascript:void(0)" });
                        links.Add(new NavigationModel { ClassName = "step2 complete", DisplayText = "Business Details", NavigationLink = "/GetQuestions", Href = "javascript:void(0)" });
                        links.Add(new NavigationModel { ClassName = "step3 complete", DisplayText = "Your Quote", NavigationLink = "/QuoteSummary", Href = "javascript:void(0)" });
                        links.Add(new NavigationModel { ClassName = "step4 complete", DisplayText = "Contact Info", NavigationLink = "/PurchaseQuote", Href = "javascript:void(0)" });
                        links.Add(new NavigationModel { ClassName = "step5 ", DisplayText = "Buy", NavigationLink = "#", Href = string.Empty });
                        break;
                    }
                case 5:
                    {
                        links.Add(new NavigationModel { ClassName = "step1 complete", DisplayText = "Business Info", NavigationLink = "/GetExposureDetails", Href = "javascript:void(0)" });
                        links.Add(new NavigationModel { ClassName = "step2 complete", DisplayText = "Business Details", NavigationLink = "/GetQuestions", Href = "javascript:void(0)" });
                        links.Add(new NavigationModel { ClassName = "step3 complete", DisplayText = "Your Quote", NavigationLink = "/QuoteSummary", Href = "javascript:void(0)" });
                        links.Add(new NavigationModel { ClassName = "step4 complete", DisplayText = "Contact Info", NavigationLink = "/PurchaseQuote", Href = "javascript:void(0)" });
                        links.Add(new NavigationModel { ClassName = "step5 complete", DisplayText = "Buy", NavigationLink = "/BuyPolicy", Href = "javascript:void(0)" });
                        break;
                    }
                default:
                    {
                        links.Add(new NavigationModel { ClassName = "step1 complete", DisplayText = "Business Info", NavigationLink = "/GetExposureDetails", Href = "javascript:void(0)" });
                        links.Add(new NavigationModel { ClassName = "step2 ", DisplayText = "Business Details", NavigationLink = "#", Href = string.Empty });
                        links.Add(new NavigationModel { ClassName = "step3 ", DisplayText = "Your Quote", NavigationLink = "#", Href = string.Empty });
                        links.Add(new NavigationModel { ClassName = "step4 ", DisplayText = "Contact Info", NavigationLink = "#", Href = string.Empty });
                        links.Add(new NavigationModel { ClassName = "step5 ", DisplayText = "Buy", NavigationLink = "#", Href = string.Empty });
                        break;
                    }
            }

            return links;
        }
    }
}
