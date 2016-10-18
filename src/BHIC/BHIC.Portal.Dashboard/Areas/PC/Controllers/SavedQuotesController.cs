using System.Web.Mvc;
using BHIC.Core.PolicyCentre;
using BHIC.Domain.PolicyCentre;
using System.Collections.Generic;
using System.Data;
using BHIC.ViewDomain;
using Newtonsoft.Json;
using System;
using System.Linq;
using BHIC.Contract.Policy;
using BHIC.Core.Policy;
using BHIC.Domain.Policy;
using BHIC.Common;
using BHIC.Portal.Dashboard.App_Start;
using BHIC.Common.XmlHelper;
using System.Reflection;
using BHIC.Common.Quote;

namespace BHIC.Portal.Dashboard.Areas.PC.Controllers
{
    [CustomAuthorize]
    [CustomAntiForgeryToken]
    public class SavedQuotesController : BaseController
    {

        // GET: DashBoard/SavedQuote
        public ActionResult SavedQuotes()
        {
            return PartialView("SavedQuotes");
        }

        [HttpGet]
        public JsonResult GetQuotes()
        {
            List<UserQuote> lstUserQuotes = new List<UserQuote>();
            try
            {
                int userId = UserSession().Id;
                GetUserQuotes getUserQuotes = new GetUserQuotes();
                lstUserQuotes = GetList(getUserQuotes.UserQuotes(userId));
            }
            catch (Exception ex)
            {
                loggingService.Fatal(string.Format("Method {0} executed with error message : {1}", MethodBase.GetCurrentMethod().Name, ex.ToString()));
            }
            return Json(new { success = true, errorMessage = "", quotes = lstUserQuotes }, JsonRequestBehavior.AllowGet);
        }

        private List<UserQuote> GetList(DataSet dsQuotes)
        {
            List<UserQuote> lstUserQuotes = new List<UserQuote>();

            //Comment : Here get logged user-id
            var loggedUserId = UserSession().Id;

            //output list creation
            if (dsQuotes.Tables[0].Rows.Count > 0)
            {
                for (int counter = 0; counter < dsQuotes.Tables[0].Rows.Count; counter++)
                {
                    var dataRow = dsQuotes.Tables[0].Rows[counter];
                    var customSession = GetSessionData(Convert.ToString(dataRow["SessionData"]));
                    lstUserQuotes.Add(new UserQuote
                    {
                        ID = Convert.ToInt32(dataRow["Id"]),
                        QuoteID = Convert.ToString(dataRow["QuoteNumber"]),
                        LineOfBusiness = Convert.ToString(dataRow["LineOfBusinessName"]),
                        RetrieveQuoteURL = Convert.ToString(dataRow["RetrieveQuoteURL"]),
                        LineOfBusinessId = Convert.ToInt32(dataRow["LineOfBusinessId"]),
                        ZipCode = customSession != null ? customSession.ZipCode : "-",
                        ClassDescriptionKeywordId = customSession == null ? null : customSession.QuoteVM == null ? null : customSession.QuoteVM.ClassDescriptionKeywordId,



                        //Quoted Date will be Created Date from DB if Guard does not provide QuotedDate from API
                        QuotedDate = Convert.ToDateTime(dataRow["CreatedDate"]),

                        //New implementation by Prem on 01.12.2015 (to link PC quote with direct PurchasePath quote-summary page)
                        //New implementation by Prem on 14.01.2016 (to send user info along with exisiting quote-id info)
                        EncryptedQuoteID = Server.UrlEncode(Encryption.EncryptText(string.Format("{0};{1}", dataRow["QuoteNumber"].ToString(), loggedUserId)))
                    });
                }

                //input for api call              
                lstUserQuotes = GetViewSavedQuotes(lstUserQuotes);
            }

            return lstUserQuotes;
        }

        private List<UserQuote> GetViewSavedQuotes(List<UserQuote> lstUserQuotes)
        {
            List<string> lstQuote = lstUserQuotes.Select(r => r.QuoteID).ToList();
            if (lstQuote.Count > 0)
            {
                string quotes = String.Join(",", lstQuote);
                IQuoteService quoteService = new QuoteService(guardServiceProvider);
                List<UserQuote> lstQuotes = quoteService.ViewSavedQuoteList(new QuoteRequestParms
                {
                    QuoteIdList = quotes,
                    IncludeRelatedPolicyData = true,
                    IncludeRelatedExposuresGraph = true,
                    IncludeRelatedRatingData = true,
                    IncludeRelatedInsuredNames = false,
                    IncludeRelatedOfficers = false,
                    IncludeRelatedLocations = false,
                    IncludeRelatedContactsGraph = false,
                    IncludeRelatedQuestions = false,
                    IncludeRelatedQuoteStatus = false,
                    IncludeRelatedPaymentTerms = false

                }, lstUserQuotes);

                //To get the zip code & Business Class for BOP Policy
                if (lstUserQuotes.Any(t => t.LineOfBusinessId == 2))
                {
                    List<string> lstBOPQuote = lstUserQuotes.Where(t => t.LineOfBusinessId == 2).Select(r => r.QuoteID).ToList();
                    string bopQuotes = String.Join(",", lstBOPQuote);
                    List<UserQuote> lstBOPQuotes = quoteService.ViewSavedBOPQuoteList(new PCQuoteInformationRequestParms
                    {
                        QuoteIdList = bopQuotes,
                    }, lstUserQuotes);

                }

                // Get Quotes based on QuoteExpiryDate
                int quoteExpiryDays = -ConfigCommonKeyReader.QuoteExpiryDays;
                for (int i = 0; i < lstUserQuotes.Count; i++)
                {
                    if (lstUserQuotes[i].QuotedDate.HasValue)
                    {
                        try
                        {
                            DateTime quoteDate = lstUserQuotes[i].QuotedDate.Value.Date;
                            DateTime quoteExpiryDate = DateTime.Now.AddDays(quoteExpiryDays).Date;

                            if (quoteDate <= quoteExpiryDate)
                            {
                                lstUserQuotes.RemoveAt(i);
                                i--;
                            }
                        }
                        catch (Exception)
                        {
                            // ignored
                        }
                    }
                }
            }
            else
            {
                lstUserQuotes = null;
            }

            return lstUserQuotes;
        }

        [HttpGet]
        public JsonResult CheckUserStatus()
        {
            return Json(new { success = (UserSession() != null) ? true : false, resultText = (UserSession() != null) ? "user logged-in" : "user logged-out" }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult DeleteQuote(UserQuote postData)
        {
            try
            {
                string quoteId = Encryption.DecryptText(Server.UrlDecode(postData.EncryptedQuoteID)).Split(';')[0];
                GetUserQuotes getUserQuotes = new GetUserQuotes();
                int userId = UserSession().Id;
                if (getUserQuotes.DeleteQuote(quoteId, userId))
                {
                    DeleteCookie();
                    return Json(new { success = true });
                }
                return Json(new { success = false, errorMessage = "quote not deleted" });
            }
            catch (Exception ex)
            {

                return Json(new { success = false, errorMessage = ex.Message });
            }
        }

        public JsonResult DeleteCookie()
        {
            //Comment : Cookies has been removed from PurchasePath (Handled Through Session Iteself)
            //if (QuoteCookieHelper.Cookie_GetQuoteId(this.ControllerContext.HttpContext) > 0)
            //{
            //    QuoteCookieHelper.Cookie_DeleteQuoteId(this.ControllerContext.HttpContext);
            //}
            return Json(new { success = true }, JsonRequestBehavior.AllowGet);
        }

        private CustomSession GetSessionData(string sessionData)
        {
            try
            {
                CustomSession customSession = JsonConvert.DeserializeObject<CustomSession>(sessionData);
                return customSession;
            }
            catch
            {
                return null;
            }
        }
    }

}
