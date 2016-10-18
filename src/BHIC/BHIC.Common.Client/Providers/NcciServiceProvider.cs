using BHIC.Domain.XMod;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace BHIC.Common.Client
{
    public class NcciServiceProvider : ServiceProvider
    {
        #region Comment : Here variables/properties declaration

        //Comment : Here set default config section and default API for this Vendor
        //Return the InsuranceSystemUrl value from the web.config
        private static ServiceConnectionElement defaultNcciApi
        {
            get
            {
                var services = new ServiceConnections("serviceConnectionsNCCI");
                return services[ServiceProviderConstants.DefaultNcciApi];
            }
        }

        #endregion

        #region Comment : Here class constructor

        //Constructor
        public NcciServiceProvider() { }

        #endregion                
        
        #region Comment : Here class public methods

        private ServiceConnections GetServices(string serviceConnectionName = ServiceProviderConstants.DefaultServiceConnectionNameNCCI)
        {
            return new ServiceConnections(serviceConnectionName);
        }

        /// <summary>
        ///  Get service/api url to communicate 
        /// </summary>
        /// <returns></returns>
        public override string ServiceUrl()
        {
            string ApiUrl = null;

            //Comment : here based on service category like "XMod, Others"
            switch (base.ServiceCategory)
            {
                case ServiceProviderConstants.NcciServiceCategoryXMod:
                    ApiUrl = defaultNcciApi.Url;
                    break;

                case ServiceProviderConstants.NcciServiceCategoryOther:
                    var services = this.GetServices();
                    ApiUrl = services[ServiceProviderConstants.NcciApiOther].Url;
                    break;
            }

            return string.IsNullOrEmpty(ApiUrl) == true ? "" : ApiUrl;
        }

        /// <summary>
        /// Method to add custom header based on serice method type and requirement
        /// </summary>
        /// <param name="serviceMethod"></param>
        /// <param name="client"></param>
        public override void SetWebClientHeaders(OperationType serviceMethod, ref WebClient client)
        {
            //Comment : here based on service method like "GET/POST" set headers
            switch (serviceMethod)
            {
                case OperationType.GET:
                    {
                        client.Headers.Add("UserAgent", "MODS On Demand");
                    }
                    break;

                case OperationType.POST:
                    {
                        client.Headers.Add("Content-Type", "application/json");
                    }
                    break;
            }
        }

        public XModRequestParms SetXModRequestParms(XModRequestParms args)
        {
            //Comment : Here get all provider specific service sections and then get provider specific service elements 
            var serviceConnectionElement = defaultNcciApi;

            return new XModRequestParms
            {
                RiskId = args.RiskId,
                Fein = args.Fein,
                //Test = ServiceProviderConstants.NcciTestParam,    //Old line
                Test = serviceConnectionElement.TestCall.ToString().ToUpper(),
                Format = ServiceProviderConstants.NcciFormatParam,
                ModType = ServiceProviderConstants.NcciModTypeParam,

                //Comment : Here get these from configuration section
                UserId = serviceConnectionElement.Username,
                Password = serviceConnectionElement.Password,
                SiteNumber = serviceConnectionElement.SiteNumber
            };
        }

        #endregion        
    }
}
