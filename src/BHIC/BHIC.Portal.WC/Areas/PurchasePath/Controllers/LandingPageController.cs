using System.Web;
using BHIC.Common.Config;
using BHIC.Common.Quote;
using BHIC.Common.Reattempt;
using BHIC.Contract.LP;
using BHIC.Core.LP;
using System;
using System.Web.Mvc;
using BHIC.Common;
using BHIC.Common.XmlHelper;
using BHIC.ViewDomain.LP;
using BHIC.Domain.Policy;
using BHIC.Portal.WC.App_Start;
using BHIC.Common.CommonUtilities;

namespace BHIC.Portal.WC.Areas.PurchasePath.Controllers
{
    //[CustomTransactionLogFilterAttribute]
    //[ValidateSession]
    public class LandingPageController : BaseController
    {
        //
        // GET: /PurchasePath/LandingPage/
        public ActionResult Index(string tokenId, string zipCode, string stateCode)
        {
            var addetail = new Domain.LP.LandingPageTransaction();
            try
            {
                // Authentication for landing page
                HttpContextBase context = this.ControllerContext.HttpContext;
                var validationKey = QuoteCookieHelper.Cookie_GetTokenId(context);
                QuoteCookieHelper.Cookie_DeleteTokenId(context);
                if (string.IsNullOrWhiteSpace(validationKey) || validationKey != string.Format(@"{0}{1}{2}", zipCode, stateCode, tokenId))
                {
                    throw new ApplicationException(string.Format("{0},{1} : {2} !", "Landing Page in Purchase Path", Constants.UnauthorizedLandingRequest, ""));
                }

                // validation for landing Page
                ILandingPageService landingPageService = new LandingPageService();
                addetail = landingPageService.GetLandingPage(tokenId);
                if ((addetail == null) || (!addetail.IsDeployed.Value))
                {
                    return RedirectToAction("Index", "Home", null);
                }

                //remove whitespaces from input parameters
                var state = stateCode.Trim();
                var lobId = addetail.lob.Trim();

                var loburlstatus = new LobUrlViewModel();

                #region old BOP code
                //if (lobId.Equals("BOP"))
                //{
                //    WalkThroughRequestParms walkThroughRequest = new WalkThroughRequestParms
                //    {
                //        ZipCode = zipCode,
                //        State = state
                //    };

                //    //append params to url
                //    var bopUrl = string.Concat(GetSchemeAndHostURLPart(), ConfigCommonKeyReader.BopUrl, UtilityFunctions.CreateQueryString(walkThroughRequest));
                //    loburlstatus.Lob = lobId;
                //    loburlstatus.Url = bopUrl;
                //    loburlstatus.Status = "OK";
                //}
                //else 
                #endregion

                if (lobId.Equals("WC") || lobId.Equals("BOP"))
                {
                    var customSession = GetCustomSession();

                    if (!string.IsNullOrEmpty(zipCode) && !string.IsNullOrEmpty(state) && !string.IsNullOrEmpty(lobId))
                    {
                        //re-generate quote id if user changes zip code 
                        if (customSession.QuoteID == 0)
                        {
                            customSession = GetNewCustomSession();
                        }
                        else
                        {
                            if ((!string.IsNullOrEmpty(customSession.ZipCode) && !customSession.ZipCode.Equals(zipCode, StringComparison.OrdinalIgnoreCase))
                                || (!string.IsNullOrEmpty(customSession.StateAbbr) && !customSession.StateAbbr.Equals(state, StringComparison.OrdinalIgnoreCase))
                                || (!string.IsNullOrEmpty(customSession.LobId) && !customSession.LobId.Equals(lobId, StringComparison.OrdinalIgnoreCase)))
                            {
                                customSession = GetNewCustomSession();
                            }
                        }

                        customSession.ZipCode = zipCode;
                        customSession.StateAbbr = state;
                        customSession.LobId = lobId;

                        //Adding valus to be used on business info
                        customSession.BusinessInfoVM.ZipCode = zipCode;
                        customSession.BusinessInfoVM.StateCode = state;
                        customSession.BusinessInfoVM.LobId = GetLobId(lobId);

                        customSession.IsLanding = true;
                        Session["googleAdId"] = tokenId;
                        SetCustomSession(customSession);

                        loburlstatus.Url = string.Concat(GetSchemeAndHostURLPart(), ConfigCommonKeyReader.PurchasePathAppBaseURL, "Quote/Index");
                        loburlstatus.Lob = lobId;

                        // Use to log landing Page Id with Policy purchase
                        return View(loburlstatus);

                    }
                }

                return RedirectToAction("Index", "Home", null);
            }
            catch (Exception ex)
            {
                loggingService.Trace("Error occured during landing page in purchase path: " + ex.Message);
                throw new ApplicationException(string.Format("{0},{1} : {2} !", "Landing Page in Purchase Path", Constants.UnauthorizedLandingRequest, ""));
            }
            finally
            {
                if (addetail != null && addetail.Id > 0)
                {
                    TransactionLogCustomSessions.CustomSessionForAdId(addetail.Id);
                }
            }

        }

        private string GetLobId(string lobAbbreviation)
        {
            lobAbbreviation = lobAbbreviation ?? string.Empty;
            return lobAbbreviation.Equals("WC", StringComparison.OrdinalIgnoreCase) ? "1" : "2";
        }

        private ViewDomain.CustomSession GetNewCustomSession()
        {
            ViewDomain.CustomSession customSession = new ViewDomain.CustomSession();
            customSession.BusinessInfoVM = new ViewDomain.Landing.BusinessInfoViewModel();
            customSession.QuoteVM = new ViewDomain.Landing.QuoteViewModel();
            customSession.QuoteVM.IsMultiClassApplicable = false;
            return customSession;
        }
    }
}
