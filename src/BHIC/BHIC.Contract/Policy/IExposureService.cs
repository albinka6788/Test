#region Using directives

using BHIC.Common.Client;
using BHIC.Domain.Policy;
using BHIC.Domain.Service;
using System;
using System.Collections.Generic;

#endregion

namespace BHIC.Contract.Policy
{
    public interface IExposureService
    {
        /// <summary>
        /// Returns list of Exposure based on QuoteId,Lob,State and ExposureId
        /// </summary>
        /// <param name="args"></param>
        /// <returns></returns>
        List<Exposure> GetExposureList(ExposureRequestParms args,ServiceProvider provider);

        /// <summary>
        /// Save new Exposure details
        /// </summary>
        /// <param name="args"></param>
        /// <returns></returns>
        OperationStatus AddExposure(Exposure args, ServiceProvider provider);

        /// <summary>
        /// Delete existing Exposure details
        /// </summary>
        /// <param name="args"></param>
        /// <returns></returns>
        OperationStatus DeleteExposure(ExposureRequestParms args, ServiceProvider provider);

        /// <summary>
        /// Validate Minimum Exposure Amount
        /// </summary>
        /// <param name="args"></param>
        /// <returns></returns>
        VExposuresMinPayrollResponse ValidateMinimumExposureAmount(VExposuresMinPayrollRequestParms args, ServiceProvider provider);
    }
}
