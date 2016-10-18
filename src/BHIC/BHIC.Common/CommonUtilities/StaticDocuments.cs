using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using BHIC.Common.Logging;
using BHIC.Common.Caching;

namespace BHIC.Common.CommonUtilities
{
    public static class StaticDocuments
    {
        public static Dictionary<string, object> GetStaticDocuments()
        {
            Dictionary<string, object> staticDocuments = new Dictionary<string, object>();
            if (!CacheHelper.Instance.IsExists("StaticDocumentsCache"))
            {
                ILoggingService logger = LoggingService.Instance;
                try
                {
                    List<string> fileList = new List<string>();

                    //Policy Document
                    fileList.Add(Encryption.EncryptText("PolicyWC000000C.pdf"));
                    fileList.Add(Encryption.EncryptText("direct-draft-form.pdf"));
                    staticDocuments.Add("PolicyDocument", fileList);

                    //Report a claim
                    fileList = new List<string>();
                    fileList.Add(Encryption.EncryptText("First_Report_of_injury_WC.pdf"));
                    fileList.Add(Encryption.EncryptText("BH_Direct_First_Fill_Letter_English_Spanish_vt.3.pdf"));

                    //BOP Report Claim Forms .zip file
                    fileList.Add(Encryption.EncryptText("BOPClaimForms.zip"));
                    staticDocuments.Add("ReportClaim", fileList);

                    //Resources
                    fileList = new List<string>();
                    fileList.Add(Encryption.EncryptText("DirectDraftform-CYB_withfields.pdf"));
                    staticDocuments.Add("Resources", fileList);

                    //EmployerNotices
                    fileList = new List<string>();
                    fileList.Add(Encryption.EncryptText("id_posting_notice-NL&F.pdf"));
                    fileList.Add(Encryption.EncryptText("id_posting_notice-bhdic.pdf"));
                    fileList.Add(Encryption.EncryptText("nh_posting_notice-NL&F.pdf"));
                    fileList.Add(Encryption.EncryptText("nh_posting_notice-bhdic.pdf"));
                    fileList.Add(Encryption.EncryptText("nj_posting_notice-NL&F.pdf"));
                    fileList.Add(Encryption.EncryptText("nj_posting_notice-bhdic.pdf"));
                    fileList.Add(Encryption.EncryptText("nj_posting_notice-spanish-NL&F.pdf"));
                    fileList.Add(Encryption.EncryptText("nj_posting_notice-spanish-bhdic.pdf"));
                    fileList.Add(Encryption.EncryptText("nj_panel-NL&F.pdf"));
                    fileList.Add(Encryption.EncryptText("nj_panel_bhdic.pdf"));
                    fileList.Add(Encryption.EncryptText("ny_posting_notice-NL&F.pdf"));
                    fileList.Add(Encryption.EncryptText("ny_posting_notice-bhdic.pdf"));
                    fileList.Add(Encryption.EncryptText("ny_c1051_acknowledgement of WC law.pdf"));
                    fileList.Add(Encryption.EncryptText("pa_posting_notice-bhdic.pdf"));
                    fileList.Add(Encryption.EncryptText("pa_acknowledgment.pdf"));
                    fileList.Add(Encryption.EncryptText("pa_instructions.pdf"));
                    fileList.Add(Encryption.EncryptText("pa_instructions-bhdic.pdf"));//not there
                    fileList.Add(Encryption.EncryptText("pa_information.pdf"));
                    fileList.Add(Encryption.EncryptText("wv_posting_notice-NL&F.pdf"));
                    fileList.Add(Encryption.EncryptText("wv_posting_notice-bhdic.pdf"));
                    fileList.Add(Encryption.EncryptText("pa_posting_notice-NL&F.pdf"));
                    staticDocuments.Add("EmployerNotices", fileList);

                    //Connecticut
                    fileList = new List<string>();
                    fileList.Add(Encryption.EncryptText("1CT_MCP_Reference_Flyer.pdf"));
                    fileList.Add(Encryption.EncryptText("2CT_MCP_Employer_Application_Form.pdf"));
                    fileList.Add(Encryption.EncryptText("3CT_MCP_Summary_of_Employer_Responsibilities.pdf"));
                    fileList.Add(Encryption.EncryptText("4CT MCP Employer Handbook.pdf"));
                    fileList.Add(Encryption.EncryptText("5CT MCP Initial Employee Notice.pdf"));
                    fileList.Add(Encryption.EncryptText("6CT MCP Employee Notice at Time of Injury.pdf"));
                    staticDocuments.Add("Connecticut", fileList);

                    //Texas
                    fileList = new List<string>();
                    fileList.Add(Encryption.EncryptText("TX_Coventry_Network_Notice.pdf"));
                    fileList.Add(Encryption.EncryptText("TX_Coventry_Employee_Information_Materials.pdf"));
                    fileList.Add(Encryption.EncryptText("TX_Coventry_Employee_Acknowledgment.pdf"));
                    fileList.Add(Encryption.EncryptText("TX_Coventry_Provider_Instruction_Form.pdf"));
                    staticDocuments.Add("Texas", fileList);

                    //Claims
                    fileList = new List<string>();
                    fileList.Add(Encryption.EncryptText("DWCForm7_2010--NO_MPN.pdf"));
                    fileList.Add(Encryption.EncryptText("CA_English_Time-of-Hire_Pamphlet.pdf"));
                    fileList.Add(Encryption.EncryptText("CA_Spanish_Time-of-Hire_Pamphlet.pdf"));
                    fileList.Add(Encryption.EncryptText("CA_DWCForm1_and_NOPE.pdf"));
                    fileList.Add(Encryption.EncryptText("DWCForm7_2010.pdf"));
                    staticDocuments.Add("CaliforniaClaims", fileList);
                    CacheHelper.Instance.Add<Dictionary<string, object>>("StaticDocumentsCache", staticDocuments, 86400);

                }
                catch (Exception ex)
                {
                    logger.Fatal(string.Format("Error in GetStaticDocuments. Error message : {0}", ex.ToString()));
                    throw;
                }

            }
            else
            {
                staticDocuments = CacheHelper.Instance.Get<Dictionary<string, object>>("StaticDocumentsCache");
            }
            return staticDocuments;
        }

    }

}
