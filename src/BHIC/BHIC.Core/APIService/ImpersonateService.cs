using BHIC.Contract.APIService;
using BHIC.DML.WC.DataContract;
using BHIC.DML.WC.DataService;
using System;
using System.Collections.Generic;

namespace BHIC.Core.APIService
{
    public class ImpersonateService : IImpersonateService
    {
        IAPIDataServiceProvider provider = new APIDataServiceProvider();

        public void DetachPolicy(string EmailID, List<string> PolicyCodes)
        {
            provider.DetachPolicy(EmailID, PolicyCodes);
        }

        public void AttachPolicy(string EmailID, List<string> PolicyCodes, DateTime StartTime, DateTime EndTime)
        {
            provider.AttachPolicy(EmailID, PolicyCodes, StartTime, EndTime);
        }
    }
}