using BHIC.Domain.Dashboard;
using System;
using System.Web.Mvc;
using System.Collections.Generic;
using System.Reflection;
using System.Text;
using BHIC.Common.Config;

namespace BHIC.Portal.Dashboard.Areas.PC.Controllers
{
    [CustomAuthorize]
    public class SideMenuController : BaseController
    {
        [HttpPost]
        public string GetMenus(string CYB, string key = "")
        {
            try
            {
                CYB = CYB.Split(new string[] { "CYB" }, StringSplitOptions.None)[0];
                PolicyInformation policyInformation = DecryptedCYBKey(CYB.Split(new string[] { "CYB" }, StringSplitOptions.None)[0]);
                if (policyInformation.Status == null)
                {
                    return "false";
                }
                 //to eliminate the dynamic concatenated date or other parameters with the status
                //string policyStatus = DisplayStatus(policyInformation.Status); // Commented By Krishna Not necessory this functionality.

                string policyStatus = policyInformation.Status;

                int lob = policyInformation.LOB;
                StringBuilder sb = new StringBuilder();
                string lobType = (lob == 1) ? Constants.WC : Constants.BOP;

                // Side menu Header Text Settint : WC or BOP
                var dynamicLOBType = "<li class='dynamicMenu productName blue-bg'> <h3 class='dynamicMenu panel-title'>" + lobType + "" +
                                  "<span class='dynamicMenu policyNumber text-small mb0'> (" + policyInformation.PolicyCode + ")</span>" +
                                  "</h3></li>";

                sb.Append(dynamicLOBType);

                Dictionary<string, string> menuList = new Dictionary<string, string>();

                // Side menu Links based on LOB WC with Active Status
                if (policyStatus == Constants.Active)
                {
                    menuList.Add("PolicyInformation", "<a id='PolicyInformation' href='#/PolicyInformation' >View Policy Information</a>");
                    menuList.Add("MakePayment", "<a id='MakePayment' href='#/MakePayment/" + CYB + "1" + "'>Make Payment</a></li>");
                    menuList.Add("PolicyDocument", "<a id='PolicyDocument' href='#/PolicyDocument'>View Policy Documents</a></li>");
                    menuList.Add("RequestCertificate", "<a id='RequestCertificate' href='#/RequestCertificate'>Request Certificate of Insurance</a></li>");
                    menuList.Add("RequestPolicyChange", "<a id='RequestPolicyChange' href='#/RequestPolicyChange'>Request a Policy Change</a></li>");
                    menuList.Add("ReportClaim", "<a id='ReportClaim' href='#/ReportClaim'>Report a Claim</a></li>");
                    if (lob == 1)
                    {
                        menuList.Add("PhysicianPanel", "<a id='PhysicianPanel' href='#/PhysicianPanel'>View Physician Panel</a></li>");
                    }
                    menuList.Add("RequestLossRun", "<a id='RequestLossRun' href='#/RequestLossRun'>Request Loss Runs</a></li>");
                    menuList.Add("UploadDocuments", "<a id='UploadDocuments' href='#/UploadDocuments'>Upload Documents</a></li>");
                    menuList.Add("CancelPolicy", "<a id='CancelPolicy' href='#/CancelPolicy'>Cancel Policy</a></li>");
                }
                else
                {
                    // Side menu Links based on LOB & other Status
                    menuList = MenuByStatus(policyStatus, CYB, lob);
                }

                foreach (var item in menuList)
                {
                    if (item.Key == key.Trim())
                        sb.Append("<li class='dynamicMenu' >" + item.Value.Insert(3, " class='activeLink' ") + "</li>");
                    else
                        sb.Append("<li class='dynamicMenu' >" + item.Value + "</li>");
                }

                return sb.ToString();
            }
            catch (Exception ex)
            {
                loggingService.Fatal(string.Format("Method {0} executed with error message : {1}", MethodBase.GetCurrentMethod().Name, ex.ToString()));
                //return "<li class='dynamicMenu' style='color:white; margin-left:25%; margin-top:15%; '>No Links Available</li>";
                return "false";
            }

        }

        private Dictionary<string, string> MenuByStatus(string policyStatus, string enPolicyCode, int LOB)
        {
            Dictionary<string, string> menuList = new Dictionary<string, string>();
            switch (policyStatus)
            {
                case Constants.ActiveSoon:
                    {
                        menuList.Add("PolicyInformation", "<a id='PolicyInformation' href='#/PolicyInformation'>View Policy Information</a>");
                        menuList.Add("MakePayment", "<a id='MakePayment' href='#/MakePayment/" + enPolicyCode + "1" + "'>Make Payment</a></li>");
                        menuList.Add("PolicyDocument", "<a id='PolicyDocument' href='#/PolicyDocument'>View Policy Documents</a></li>");
                        menuList.Add("RequestPolicyChange", "<a id='RequestPolicyChange' href='#/RequestPolicyChange'>Request a Policy Change</a></li>");
                        if (LOB == 1)
                        {
                            menuList.Add("PhysicianPanel", "<a id='PhysicianPanel' href='#/PhysicianPanel'>View Physician Panel</a></li>");
                        }
                        menuList.Add("UploadDocuments", "<a id='UploadDocuments' href='#/UploadDocuments'>Upload Documents</a></li>");
                        break;
                    }
                case Constants.Expired:
                    {
                        menuList.Add("PolicyInformation", "<a id='PolicyInformation' href='#/PolicyInformation'>View Policy Information</a>");
                        menuList.Add("PolicyDocument", "<a id='PolicyDocument' href='#/PolicyDocument'>View Policy Documents</a></li>");
                        menuList.Add("ReportClaim", "<a id='ReportClaim' href='#/ReportClaim'>Report a Claim</a></li>");
                        if (LOB == 1)
                        {
                            menuList.Add("PhysicianPanel", "<a id='PhysicianPanel' href='#/PhysicianPanel'>View Physician Panel</a></li>");
                        }
                        menuList.Add("RequestLossRun", "<a id='RequestLossRun' href='#/RequestLossRun'>Request Loss Runs</a></li>");
                        menuList.Add("UploadDocuments", "<a id='UploadDocuments' href='#/UploadDocuments'>Upload Documents</a></li>");
                        break;
                    }
                case Constants.Cancelled:
                    {
                        menuList.Add("PolicyInformation", "<a id='PolicyInformation' href='#/PolicyInformation'>View Policy Information</a>");
                        menuList.Add("PolicyDocument", "<a id='PolicyDocument' href='#/PolicyDocument'>View Policy Documents</a></li>");
                        menuList.Add("ReportClaim", "<a id='ReportClaim' href='#/ReportClaim'>Report a Claim</a></li>");
                        if (LOB == 1)
                        {
                            menuList.Add("PhysicianPanel", "<a id='PhysicianPanel' href='#/PhysicianPanel'>View Physician Panel</a></li>");
                        }
                        menuList.Add("RequestLossRun", "<a id='RequestLossRun' href='#/RequestLossRun'>Request Loss Runs</a></li>");
                        menuList.Add("UploadDocuments", "<a id='UploadDocuments' href='#/UploadDocuments'>Upload Documents</a></li>");
                        break;
                    }
                case Constants.PendingCancellation:
                    {
                        menuList.Add("PolicyInformation", "<a id='PolicyInformation' href='#/PolicyInformation'>View Policy Information</a>");
                        menuList.Add("MakePayment", "<a id='MakePayment' href='#/MakePayment/" + enPolicyCode + "1" + "'>Make Payment</a></li>");
                        menuList.Add("PolicyDocument", "<a id='PolicyDocument' href='#/PolicyDocument'>View Policy Documents</a></li>");
                        menuList.Add("RequestPolicyChange", "<a id='RequestPolicyChange' href='#/RequestPolicyChange'>Request a Policy Change</a></li>");
                        menuList.Add("ReportClaim", "<a id='ReportClaim' href='#/ReportClaim'>Report a Claim</a></li>");
                        if (LOB == 1)
                        {
                            menuList.Add("PhysicianPanel", "<a id='PhysicianPanel' href='#/PhysicianPanel'>View Physician Panel</a></li>");
                        }
                        menuList.Add("RequestLossRun", "<a id='RequestLossRun' href='#/RequestLossRun'>Request Loss Runs</a></li>");
                        menuList.Add("UploadDocuments", "<a id='UploadDocuments' href='#/UploadDocuments'>Upload Documents</a></li>");
                        break;
                    }
                case Constants.NoCoverage:
                    menuList.Add("Blank", "<li class='dynamicMenu' style='color:white; margin-left:25%; margin-top:15%; '>No Links Available</li>");
                    break;
                default:
                    menuList.Add("Blank", "<li class='dynamicMenu' style='color:white; margin-left:25%; margin-top:15%; '>No Links Available</li>");
                    break;
            }
            return menuList;
        }

        [HttpPost]
        public JsonResult IsRequestValid(string CYBKey, string menuName)
        {
            return Json(new { success = DecryptedCYBKey(CYBKey.Split(new string[] { "CYB" }, StringSplitOptions.None)[0], menuName) }, JsonRequestBehavior.AllowGet);
        }
    }
}
