using System;
using System.Collections.Generic;

namespace BHIC.Contract.APIService
{
    public interface IImpersonateService
    {
        void DetachPolicy(string EmailID, List<string> PolicyCodes);
        void AttachPolicy(string EmailID, List<string> PolicyCodes, DateTime dateTime1, DateTime dateTime2);
    }
}
