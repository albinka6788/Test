#region Using Directives

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using BHIC.Contract.Background;
using BHIC.Domain.XMod;
using BHIC.Common;
using BHIC.Contract.Provider;
using BHIC.Common.Client;
using BHIC.Common.Config;

#endregion

namespace BHIC.Core.XMod
{
    public class XModService : IServiceProviders, IXModService
    {
        #region Comment : Here constructor

        public XModService(ServiceProvider provider)
        {
            base.ServiceProvider = provider;
        }

        #endregion

        #region Methods

        /// <summary>
        /// Get xmod factor for particular RiskId
        /// </summary>
        /// <param name="args"></param>
        /// <returns></returns>
        public RiskXmodFactorResponse GetModRiskId(XModRequestParms args)
        {
            #region Comment : Here first of all get Configuration parameters from NCCI ConfigSection and other details from provider class

            var ncciServiceProvider = new NcciServiceProvider();
            args = ncciServiceProvider.SetXModRequestParms(args);

            #endregion

            var riskXmodFactorResponse = SvcClient.CallService<RiskXmodFactorResponse>(
                string.Concat(Constants.XModRiskId, UtilityFunctions.CreateQueryString<XModRequestParms>(args)), ServiceProvider);

            if (riskXmodFactorResponse != null)
            {
                return riskXmodFactorResponse;
            }
            else
            {
                throw new ApplicationException(string.Format("Unable to fetch XMod factor for RiskId : {0}",args.RiskId));
            }
        }

        /// <summary>
        /// Get xmod factor for particular RiskId
        /// </summary>
        /// <param name="args"></param>
        /// <returns></returns>
        public RiskXmodFactorResponse GetModFein(XModRequestParms args)
        {
            #region Comment : Here first of all get Configuration parameters from NCCI ConfigSection and other details from provider class

            var ncciServiceProvider = new NcciServiceProvider();
            args = ncciServiceProvider.SetXModRequestParms(args);

            #endregion

            var riskXmodFactorResponse = SvcClient.CallService<RiskXmodFactorResponse>(
                string.Concat(Constants.XModFein, UtilityFunctions.CreateQueryString<XModRequestParms>(args)), ServiceProvider);

            if (riskXmodFactorResponse != null)
            {
                return riskXmodFactorResponse;
            }
            else
            {
                throw new ApplicationException(string.Format("Unable to fetch XMod factor for FeinNumber : {0}", args.RiskId));
            }
        }

        #endregion
    }
}
