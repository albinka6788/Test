using BHIC.Common.XmlHelper;
using BHIC.Contract.LP;
using BHIC.Core.LP;
using BHIC.Domain.LP;
using BHIC.Portal.LP.Areas.LandingPage.Models;
using BHIC.Portal.LP.Controllers;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web.Mvc;
using MvcPaging;
using BHIC.DML.WC.DTO;
using BHIC.Common.Logging;

namespace BHIC.Portal.LP.Areas.LandingPage.Controllers
{
    public class HomeController : BaseController
    {
        private static ILoggingService logger = LoggingService.Instance;
        ILandingPageService landingPageService;
        public HomeController()
        {
            landingPageService = new LandingPageService();
        }
        // GET: LandingPage/Home
        public ActionResult Index()
        {
            var homeViewModel = new HomeViewModel();
            homeViewModel.masterCollection = new MasterCollection();
            Master_Helper(homeViewModel.masterCollection);
            return View(homeViewModel);
        }

        [HttpPost]
        public ActionResult Index(string submitButton, HomeViewModel homeViewModel)
        {
           
                switch (submitButton)
                {
                    case "Generate":
                        AddLandingPage(homeViewModel.landingPageTransaction);
                        return RedirectToAction("LandingPageAccount");

                    case "Preview":
                        return View("LandingPageReview", homeViewModel);
                }
                return View("LandingPageReview", homeViewModel);
        }

        [HttpGet]
        public ActionResult GetAd(string AdId)
        {
            var homeViewModel = new HomeViewModel();
            homeViewModel.landingPageTransaction = landingPageService.GetLandingPage(AdId);
            if (homeViewModel.landingPageTransaction.Id > 0)
                return View(homeViewModel);
            else
                return Redirect(GetSchemeAndHostURLPart());
        }

        [HttpPost]
        public ActionResult GetAd(HomeViewModel homeViewModel)
        {
            #region Comment : Here create mail embedded link

            string wcAppBaseUrl = ConfigCommonKeyReader.PurchasePathAppBaseURL;
            wcAppBaseUrl = wcAppBaseUrl.EndsWith("/") ? wcAppBaseUrl.Substring(0, wcAppBaseUrl.Length - 1) : wcAppBaseUrl;

            var targeturl = ConfigCommonKeyReader.LandingPageAppBaseURL + "Index?tokenId=" + homeViewModel.landingPageTransaction.TokenId + "&zipCode=" + homeViewModel.landingPageTransaction.ZipCode;

            targeturl = string.Format("{0}{1}{2}", GetSchemeAndHostURLPart(), wcAppBaseUrl, targeturl);

            #endregion

            //homeViewModel.landingPageTransaction = landingPageService.GetLandingPage(homeViewModel.landingPageTransaction.TokenId);            
            return Redirect(targeturl);
        }


        public ActionResult Account()
        {
            var homeViewModel = new HomeViewModel();
            homeViewModel.landingPageTransactions = landingPageService.GetLandingPages(homeViewModel.filter);
            return View(homeViewModel);
        }

        private const int DefaultPageSize = 10;
        public ActionResult LandingPageAccount(int? page, HomeViewModel homeViewModel, string searchfilter)
        {
            var t = Request.Form["Search"];
            int currentPageIndex = page.HasValue ? page.Value : 1;

            if (Request.Form["Edit"] != null)
            {
                var id = homeViewModel.SelectedUser;
                ModelState.Clear();
                homeViewModel.landingPageTransaction = landingPageService.GetLandingPage(id);
                homeViewModel.masterCollection = new MasterCollection();
                Master_Helper(homeViewModel.masterCollection);
                return View("Index", homeViewModel);
            }
            if ((Request.Form["Search"] != null) && (string.IsNullOrWhiteSpace(homeViewModel.SearchFilter) == false || string.IsNullOrWhiteSpace(searchfilter) == false))
            {
                if (string.IsNullOrWhiteSpace(searchfilter) == false)
                {
                    homeViewModel.SearchFilter = searchfilter;
                }
                homeViewModel.landingPageTransactionlist = landingPageService.GetLandingPages(homeViewModel.SearchFilter).ToPagedList(currentPageIndex, DefaultPageSize);
                return View(homeViewModel);
            }
            homeViewModel.landingPageTransactionlist = landingPageService.GetLandingPages(homeViewModel.SearchFilter).ToPagedList(currentPageIndex, DefaultPageSize);
            return View(homeViewModel);
        }
        private void AddLandingPage(LandingPageTransaction landingPageTransaction)
        {
            landingPageTransaction.TokenId = Guid.NewGuid().ToString("N");
            landingPageTransaction.TemplateId = 1;
            landingPageTransaction.TransactionCounter = 0;

            landingPageService.CreateLandingPage(landingPageTransaction, 0);
        }

        private void Master_Helper(MasterCollection masterCollection)
        {
            masterCollection.Lob.Add(new SelectListItem { Value = "1", Text = "WC" });
            masterCollection.Lob.Add(new SelectListItem { Value = "2", Text = "BOP" });
            masterCollection.Lob.Add(new SelectListItem { Value = "3", Text = "CA" });

            masterCollection.State.Add(new SelectListItem { Value = "NY", Text = "NY" });
            masterCollection.State.Add(new SelectListItem { Value = "PA", Text = "PA" });
            masterCollection.State.Add(new SelectListItem { Value = "NJ", Text = "NJ" });

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
        }

        [HttpGet]
        public JsonResult GetValidZipDetail(string zipCode)
        {
            try
            {
                zipCode = zipCode.Trim();
                if (!string.IsNullOrEmpty(zipCode) && IsZipExists(zipCode))
                    return GetStateListByZipCode(zipCode);
                return Json(new { success = false, }, JsonRequestBehavior.AllowGet);
            }
            catch(Exception ex)
            {
                logger.Fatal(string.Format("GetValidZipDetail with error message : {0}", ex.ToString()));
                return Json(new { success = false, }, JsonRequestBehavior.AllowGet);
            }

            
        }

        /// <summary>
        /// Get State list by LOB detail for current state or specified state
        /// </summary>
        /// <param name="zipCode"></param>
        /// <param name="stateCode"></param>
        /// <returns></returns>
        private JsonResult GetStateListByZipCode(string zipCode, string stateCode = "", string selectedLobId = "")
        {
            if (!string.IsNullOrEmpty(zipCode))
            {
                List<ZipCodeStates> county = GetAllStateByZip(zipCode);

                //if county service is not available, throw error
                if (county == null || county.Count <= 0)
                {
                    throw new Exception("Unable to fetch specified zipcode state detail due to some technical issue in DB or Guard API");
                }

                
                //return detail for zip code, state and lob list
                return Json(new
                {
                    success = true,
                    county = county,
                    lobResult = ((county.Count == 1) ? GetLobList(county.FirstOrDefault().StateCode) : (string.IsNullOrEmpty(stateCode) ? null : GetLobList(stateCode))),
                    selectedState = (string.IsNullOrEmpty(stateCode)) ? string.Empty : stateCode,
                    selectedLob = (string.IsNullOrEmpty(selectedLobId)) ? string.Empty : selectedLobId
                }, JsonRequestBehavior.AllowGet);
            }

            return Json(new
            {
                success = false,
            });
        }


        /// <summary>
        /// Get Name of lob based on state code
        /// </summary>
        /// <param name="stateCode">State code</param>
        /// <returns>Returns lob filterd by state</returns>
        public JsonResult GetLobList(string stateCode)
        {

            List<LineOfBusiness> lobList = new List<LineOfBusiness>();

            if (!string.IsNullOrEmpty(stateCode))
            {
                lobList = landingPageService.GetAllLineOfBusiness().Where(x => x.StateCode.Equals(stateCode)).ToList();
            }

            //if given parameter value exists return true,false otherwise
            return Json(new
            {
                success = lobList.Count > 0 ? true : false,
                lob = lobList
            }, JsonRequestBehavior.AllowGet);
        }
    }
}