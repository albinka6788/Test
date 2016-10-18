#region Using directives

using BHIC.Domain.Service;
using System;
using System.Collections.Generic;

#endregion

namespace BHIC.Domain.Policy
{
    public class ProspectInfoResponse
    {
        // ----------------------------------------
        // constructor
        // ----------------------------------------

        public ProspectInfoResponse()
        {
            // init objects to help avoid issues related to null reference exceptions
            ProspectInfoList = new List<ProspectInfo>();
            OperationStatus = new OperationStatus();
        }

        // ----------------------------------------
        // properties
        // ----------------------------------------

        public List<ProspectInfo> ProspectInfoList { get; set; }
        public OperationStatus OperationStatus { get; set; }
    }
}
