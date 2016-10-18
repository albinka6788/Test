#region Using directives

using BHIC.Common;
using BHIC.Common.Client;
using BHIC.Common.Config;
using BHIC.Contract.Policy;
using BHIC.Domain.Policy;
using BHIC.Domain.Service;
using System;
using System.Collections.Generic;

#endregion

namespace BHIC.Core.Policy
{
    public class ExposureService : IExposureService
    {
        #region Methods

        #region Public Methods

        /// <summary>
        /// Returns list of Exposure based on QuoteId,Lob,State and ExposureId
        /// </summary>
        /// <param name="args"></param>
        /// <returns></returns>
        public List<Exposure> GetExposureList(ExposureRequestParms args, ServiceProvider provider)
        {
            var exposureResponse = SvcClient.CallService<ExposureResponse>(string.Concat(Constants.Exposure,
                UtilityFunctions.CreateQueryString<ExposureRequestParms>(args)), provider);

            if (exposureResponse.OperationStatus.RequestSuccessful)
            {
                return exposureResponse.Exposures;
            }
            else
            {
                throw new ApplicationException(UtilityFunctions.ConvertMessagesToString(exposureResponse.OperationStatus.Messages));
            }
        }

        /// <summary>
        /// Save new Exposure details
        /// </summary>
        /// <param name="args"></param>
        /// <returns></returns>
        public OperationStatus AddExposure(Exposure args, ServiceProvider provider)
        {
            var exposureResponse = SvcClient.CallService<Exposure, OperationStatus>(Constants.Exposure, "POST", args, provider);

            if (exposureResponse.RequestSuccessful)
            {
                return exposureResponse;
            }
            else
            {
                throw new ApplicationException(UtilityFunctions.ConvertMessagesToString(exposureResponse.Messages));
            }
        }

        /// <summary>
        /// Delete existing Exposure details
        /// </summary>
        /// <param name="args"></param>
        /// <returns></returns>
        public OperationStatus DeleteExposure(ExposureRequestParms args, ServiceProvider provider)
        {
            var exposureResponse = SvcClient.CallService<ExposureRequestParms, OperationStatus>(Constants.Exposure, "DELETE", args, provider);

            if (exposureResponse.RequestSuccessful)
            {
                return exposureResponse;
            }
            else
            {
                throw new ApplicationException(UtilityFunctions.ConvertMessagesToString(exposureResponse.Messages));
            }
        }

        public VExposuresMinPayrollResponse ValidateMinimumExposureAmount(VExposuresMinPayrollRequestParms args, ServiceProvider provider)
        {
            var response = SvcClient.CallService<VExposuresMinPayrollResponse>(string.Concat(Constants.ServiceNames.ValidateExposureMinimumPayroll,
                UtilityFunctions.CreateQueryString<VExposuresMinPayrollRequestParms>(args)), provider);
            if (!UtilityFunctions.IsNull(response))
            {
                return response;
            }
            else
            {
                throw new ApplicationException(Constants.ServiceResponseNotRecieved);
            }
            
        }
        #endregion

        #endregion
    }
}
