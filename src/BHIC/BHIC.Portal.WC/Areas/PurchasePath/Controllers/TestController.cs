using BHIC.Common.Client;
using BHIC.Common.DataAccess;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Web.Mvc;

namespace BHIC.Portal.WC.Areas.PurchasePath.Controllers
{
    public class TestController : BaseController
    {

        public TestController()
        {
            //bool xModExists = true;
            //decimal xModValue = GetFeinXModFactorNonPaid("", out xModExists);
            //xModExists = UpdateFeinXModFactorInLocalDB("123456789", Convert.ToDecimal(0.75), DateTime.Now);
        }

        //
        // GET: /PurchasePath/Test/

        public ActionResult Index()
        {
            return View();
        }

        // These are the actions that show streaming HTML with the BOP walkthrough
        public ActionResult WalkthroughTest()
        {
            //var postData = new InsuranceServiceDTO.Policy.WalkThrough() { AjaxProxyUrl = "https://xdsml.guard.com/cybshell/Home/WalkthroughAjax" };
            var postData = new BHIC.Domain.Policy.WalkThrough() { AjaxProxyUrl = "http://localhost:50350/Test/WalkthroughAjax" };
            var results = SvcClient.CallServicePost<BHIC.Domain.Policy.WalkThrough, BHIC.Domain.Policy.WalkThroughResponse>("WalkThrough", postData, guardServiceProvider);

            //if (!results.OperationStatus.RequestSuccessful)
            // results.WalkThrough.Response = results.OperationStatus.Messages[0].Text.ToString();

            return View(results.WalkThrough);
        }

        public ActionResult WalkthroughAjax(string u)
        {
            var body = "";
            foreach (String key in Request.Params.AllKeys)
            {
                body += key + "=" + Request.Params[key] + "&";
            }

            //var postData = new InsuranceServiceDTO.Policy.WalkThrough() { WalkThroughCode = "N9BP123456", EncodedUrl = u, AjaxProxyUrl = "https://xdsml.guard.com/cybshell/Home/WalkthroughAjax", Request = body };
            var postData = new BHIC.Domain.Policy.WalkThrough() { WalkThroughCode = "N9BP123456", EncodedUrl = u, AjaxProxyUrl = "http://localhost:50350/Test/WalkthroughAjax", Request = body };

            var results = SvcClient.CallServicePost<BHIC.Domain.Policy.WalkThrough, BHIC.Domain.Policy.WalkThroughResponse>("WalkThrough", postData, guardServiceProvider);

            return Content(results.WalkThrough.Response, results.WalkThrough.ResponseType);
        }

        private BHICDBBase GetDbConnector()
        {
            BHICDBBase dbConnector = new BHICDBBase();
            dbConnector.DBName = "GuinnessDB";
            //dbConnector.DBConnectionString = "Data Source=192.168.27.34;Initial Catalog=Guinness_DB;User Id=Guinness;Password=g@1231;";
            //dbConnector.CreateDBObjects(Providers.SqlServer);
            //dbConnector.CreateConnection();

            return dbConnector;
        }

        /// <summary>
        /// Return xMod factor for supplied FEIN number from local DataSource/DB without any service api charges
        /// </summary>
        /// <param name="feinNumber"></param>
        /// <param name="xModExists"></param>
        /// <returns></returns>
        private decimal GetFeinXModFactorNonPaid(string feinNumber, out bool xModExists)
        {
            //Comment : Here set default
            xModExists = false;

            try
            {
                //Comment : Here get DbConnector object
                var dataSet = GetDbConnector().LoadDataSet("GetFeinXModFactor", QueryCommandType.StoredProcedure,
                    new List<System.Data.IDbDataParameter> { new SqlParameter() { ParameterName = "@FeinNumber", Value = feinNumber, SqlDbType = SqlDbType.VarChar, Size = 9 } });

                if (dataSet.Tables[0].Rows.Count > 0)
                {
                    var premiumFactor = dataSet.Tables[0].Rows[0]["XModValue"];

                    xModExists = premiumFactor != null ? true : false;
                    return premiumFactor != null ? Convert.ToDecimal(premiumFactor) : 0;
                }
            }
            catch { }

            return 0;
        }

        /// <summary>
        /// Return premium threshold based on supplied state-code
        /// </summary>
        /// <param name="stateCode"></param>
        /// <returns></returns>
        private bool UpdateFeinXModFactorInLocalDB(string feinNumber, decimal feinXModFactor, DateTime expiryDate)
        {
            try
            {
                //Comment : Here get DbConnector object
                var rowEffeted = GetDbConnector().ExecuteNonQuery("UpdateFeinXModFactor", QueryCommandType.StoredProcedure,
                                new List<System.Data.IDbDataParameter> 
                                { 
                                    new SqlParameter() { ParameterName = "@FeinNumber", Value = feinNumber, SqlDbType = SqlDbType.Char, Size = 9 } ,
                                    new SqlParameter() { ParameterName = "@XModValue", Value = feinXModFactor, SqlDbType = SqlDbType.Float },
                                    new SqlParameter() { ParameterName = "@ExpiryDate", Value = expiryDate, SqlDbType = SqlDbType.DateTime }
                                });

                if (rowEffeted > 0)
                {
                    return true;
                }
            }
            catch
            {
                //Comment : Here in case of DB updation failure log it into XML for future re-trying by schedular service
            }

            return false;
        }
    }
}
