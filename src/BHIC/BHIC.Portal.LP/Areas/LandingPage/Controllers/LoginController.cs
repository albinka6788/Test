using BHIC.Common.Logging;
using BHIC.Common.Quote;
using BHIC.Common.XmlHelper;
using BHIC.Contract.LP;
using BHIC.Contract.PurchasePath;
using BHIC.Core.LP;
using BHIC.Core.PurchasePath;
using BHIC.DML.WC.DTO;
using BHIC.Domain.LP;
using BHIC.Portal.LP.Areas.LandingPage.Models;
using BHIC.Portal.LP.Controllers;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace BHIC.Portal.LP.Areas.LandingPage.Controllers
{
    public class LoginController : BaseController
    {
        ILandingPageService landingPageService;
        public LoginController()
        {
            landingPageService = new LandingPageService();
        }

        private static ILoggingService logger = LoggingService.Instance;
        // GET: LandingPage/LandigPage
        public ActionResult Index()
        {
            return PartialView("Index");
        }

        // GET: LandingPage/Login
        public ActionResult Login()
        {
            return PartialView("Login");
        }

        // GET: LandingPage/List of Landing Pages
        public ActionResult LandingPages()
        {
            return PartialView("LandingPages");
        }

        public ActionResult AddTemplateBcakground()
        {
            return PartialView("AddTemplateBcakground");
        }

        // GET: LandingPage/AddNewLanding
        public ActionResult InsertOrUpdateLandingPage(string TokenId)
        {
            return PartialView("InsertOrUpdateLandingPage");
        }

        public ActionResult ViewYourLandingPage(string TokenId)
        {
            return PartialView("ViewYourLandingPage");
        }

        [HttpPost]
        public ActionResult CheckAuthentication(dynamic postData)
        {
            // User name & Passward containse postData
            try
            {
                Dictionary<string, string> values = JsonConvert.DeserializeObject<Dictionary<string, string>>(postData);
                long userId = landingPageService.Authentication(values["username"], values["password"]);
                if (userId > 0)
                {
                    Session["UserId"] = userId;
                    return Json(new { success = true });
                }
                return Json(new { success = false });
            }
            catch (Exception ex)
            {

                logger.Fatal(string.Format("CheckAuthentication with error message : {0}", ex.ToString()));
                return Json(new { success = false });
            }

        }

        [HttpPost]
        public ActionResult Logout()
        {
            // User name & Passward containse postData
            Session.RemoveAll();
            Session.Abandon();
            return Json(new { success = true });
        }



        [HttpGet]
        public ActionResult GetAllLandingPages()
        {
            try
            {
                var lptlist = landingPageService.GetLandingPages("");
                return Json(new { success = true, listOfLandingPages = lptlist }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                logger.Fatal(string.Format("GetAllLandingPages with error message : {0}", ex.ToString()));
                return Json(new { success = false }, JsonRequestBehavior.AllowGet);
            }
        }


        [HttpGet]
        public ActionResult GetDefaultLists()
        {

            try
            {

                MasterCollection masterCollection = new MasterCollection();

                var homeViewModel = new HomeViewModel();
                DataSet masterData = landingPageService.GetDefaultMasterData();
                DataTable ListOfLOB = masterData.Tables[0];
                DataTable ListOfStates = masterData.Tables[1];
                DataTable ListOfTemplates = masterData.Tables[2];

                foreach (DataRow item in ListOfLOB.Rows)
                {
                    masterCollection.Lob.Add(new SelectListItem { Value = item["Value"].ToString(), Text = item["Text"].ToString() });
                }

                foreach (DataRow item in ListOfStates.Rows)
                {
                    masterCollection.State.Add(new SelectListItem { Value = item["Value"].ToString(), Text = item["Text"].ToString() });
                }

                foreach (DataRow item in ListOfTemplates.Rows)
                {
                    masterCollection.Templates.Add(new SelectListItem { Value = item["Value"].ToString(), Text = item["Text"].ToString() });
                }


                if (Directory.Exists(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Images//Logo")))
                {
                    foreach (var item in Directory.GetFiles(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Images//Logo")).Select(Path.GetFileName).ToList())
                    {
                        masterCollection.Logo.Add(new SelectListItem { Value = "/LandingPage/Images/Logo/" + item, Text = item });
                    }

                }

                if (Directory.Exists(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Images//MainImage")))
                {
                    foreach (var item in Directory.GetFiles(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Images//MainImage")).Select(Path.GetFileName).ToList())
                    {
                        masterCollection.MainImage.Add(new SelectListItem { Value = "/LandingPage/Images/MainImage/" + item, Text = item });
                    }
                }


                return Json(new
                {
                    success = true,
                    listOfLOB = masterCollection.Lob,
                    listOfStates = masterCollection.State,
                    listOfLogos = masterCollection.Logo,
                    listOfBackgroundImages = masterCollection.MainImage,
                    listOfTemplates = masterCollection.Templates,
                }, JsonRequestBehavior.AllowGet);

            }
            catch (Exception ex)
            {
                logger.Fatal(string.Format("GetDefaultLists error message : {0}", ex.ToString()));
                return Json(new { success = false });
            }
        }

        [HttpGet]
        public ActionResult GetLandingPageDetailsByTokenId(string TokenId)
        {
            try
            {
                var homeViewModel = new HomeViewModel();
                homeViewModel.landingPageTransaction = landingPageService.GetLandingPage(TokenId);
                if (homeViewModel.landingPageTransaction.Id > 0)
                    return Json(new { success = true, landingPageDetails = homeViewModel.landingPageTransaction }, JsonRequestBehavior.AllowGet);
                else
                    return Json(new { success = false, RedirectUrl = GetSchemeAndHostURLPart() }, JsonRequestBehavior.AllowGet);

            }
            catch (Exception ex)
            {
                logger.Fatal(string.Format("GetLandingPageDetailsByTokenId with error message : {0}", ex.ToString()));
                return Json(new { success = false, RedirectUrl = GetSchemeAndHostURLPart() }, JsonRequestBehavior.AllowGet);
            }

        }


        [HttpPost]
        public ActionResult PostDataForLandingPage()
        {
            bool flag = false;
            bool isExists = false;
            string errorMsg = "";
            try
            {
                if (Request != null)
                {
                    var files = Enumerable.Range(0, Request.Files.Count).Select(i => Request.Files[i]);

                    foreach (HttpPostedFileBase file in files)
                    {
                        // Existing Images
                        MasterCollection masterCollection = new MasterCollection();
                        if (Directory.Exists(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Images//MainImage")))
                        {
                            foreach (var item in Directory.GetFiles(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Images//MainImage")).Select(Path.GetFileName).ToList())
                            {
                                if (file.FileName == item)
                                {
                                    isExists = true;
                                    errorMsg = "The Selected Background Image : " + item + " : already exists in the repository";
                                    break;
                                }
                            }
                        }
                    }
                }

                if (Request != null)
                {
                    var files = Enumerable.Range(0, Request.Files.Count).Select(i => Request.Files[i]);

                    foreach (HttpPostedFileBase file in files)
                    {

                        if (!isExists)
                        {
                            string fileName = file.FileName;
                            string fileContentType = file.ContentType;
                            var inputStream = file.InputStream;
                            var fname = Path.GetFileName(fileName);
                            var path = Path.Combine(Server.MapPath("~/Images/MainImage"), fileName);
                            flag = false;
                            using (var fileStream = System.IO.File.Create(path))
                            {
                                inputStream.CopyTo(fileStream);
                                flag = true;
                            }
                        }
                    }
                }

                if (flag)
                {
                    return Json(new { success = true }, JsonRequestBehavior.AllowGet);
                }
                {
                    return Json(new { success = false, msg = errorMsg }, JsonRequestBehavior.AllowGet);
                }

            }
            catch (Exception ex)
            {
                logger.Fatal(string.Format("PostDataForLandingPage with image uploade error message : {0}", ex.ToString()));
                return Json(new { success = false, msg = ex.ToString() }, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        public ActionResult AddOrUpdateLandingPage(LandingPageTransaction postData)
        {

            try
            {
                string tokenId = (postData.TokenId == null) ? Guid.NewGuid().ToString("N") : postData.TokenId;
                postData.TokenId = tokenId;
                long userId = Convert.ToInt64(Session["UserId"]);
                landingPageService.CreateLandingPage(postData, userId);
                landingPageService.InsertOrUpdateCTAMsgs(postData.CTAMsgList, tokenId, userId);
                return Json(new { success = true }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                logger.Fatal(string.Format("AddOrUpdateLandingPage with error message : {0}", ex.ToString()));
                return Json(new { success = false }, JsonRequestBehavior.AllowGet);
            }

        }

        [HttpPost]
        public ActionResult GetAd(dynamic postData)
        {
            try
            {
                Dictionary<string, string> values = JsonConvert.DeserializeObject<Dictionary<string, string>>(postData);

                var zipCode = values["ZipCode"].Trim();
                var stateCode = values["StateCode"].Trim();
                var tokenId = values["TokenId"].Trim();
                if (!string.IsNullOrEmpty(zipCode) && IsZipExists(zipCode))
                {
                    var county = GetAllStateByZip(zipCode);
                    var lobList = new List<LineOfBusiness>();
                    if (county != null && county.Any(t => t.StateCode == stateCode))
                    {
                        ILineOfBusinessService lobService = new LineOfBusinessService();
                        lobList = lobService.GetLineOfBusiness().Where(x => x.StateCode.Equals(stateCode)).ToList();
                    }

                    ILandingPageService landingPageService = new LandingPageService();
                    var addetail = landingPageService.GetLandingPage(tokenId);
                    if (addetail == null)
                        return Json(new { success = false, errorMsg = "Invalid Landing Page." }, JsonRequestBehavior.AllowGet);

                    if (lobList.First(t => t.Abbreviation == addetail.lob).Status != "Available")
                    {
                        return Json(new { success = false, errorMsg = lobList.First(t => t.Abbreviation == addetail.lob).Status }, JsonRequestBehavior.AllowGet);
                    }

                    string wcAppBaseUrl = ConfigCommonKeyReader.PurchasePathAppBaseURL;
                    wcAppBaseUrl = wcAppBaseUrl.EndsWith("/") ? wcAppBaseUrl.Substring(0, wcAppBaseUrl.Length - 1) : wcAppBaseUrl;

                    var targeturl = ConfigCommonKeyReader.LandingPageAppBaseURL + "Index?tokenId=" + values["TokenId"] + "&zipCode=" + values["ZipCode"] + "&stateCode=" + values["StateCode"];

                    targeturl = string.Format("{0}{1}{2}", GetSchemeAndHostURLPart(), wcAppBaseUrl, targeturl);

                    HttpContextBase context = this.ControllerContext.HttpContext;
                    QuoteCookieHelper.Cookie_SaveTokenId(context, string.Format(@"{0}{1}{2}", zipCode, stateCode, tokenId));
                    return Json(new { success = true, redirectUrl = targeturl }, JsonRequestBehavior.AllowGet);
                }
                return Json(new { success = false, errorMsg = "Please Enter Valid ZipCode." }, JsonRequestBehavior.AllowGet);

            }
            catch (Exception ex)
            {
                logger.Fatal(string.Format("GetAd with error message  : {0}", ex.ToString()));
                return Json(new { success = false, errorMsg = "Home", redirectUrl = string.Format("{0}", GetSchemeAndHostURLPart()) }, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        public ActionResult DeleteLandingPages(string postData)
        {

            try
            {
                landingPageService.DeleteLandingPages(JsonConvert.DeserializeObject<string>(postData), Convert.ToInt64(Session["UserId"]));
                return Json(new { success = true }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                logger.Fatal(string.Format("Delete LandingPages with error message : {0}", ex.ToString()));
                return Json(new { success = false }, JsonRequestBehavior.AllowGet);
            }

        }

        [HttpGet]
        public ActionResult GetCurrentEnvironment()
        {
            return Json(new { success = true, redirectUrl = GetSchemeAndHostURLPart() }, JsonRequestBehavior.AllowGet);
        }

    }
}