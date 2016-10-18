#region Using directives

using BHIC.Common.Client;
using BHIC.Contract.Policy;
using BHIC.Contract.Provider;
using BHIC.Domain.Policy;
using BHIC.Domain.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

#endregion

namespace BHIC.Core.Policy
{
    public class VInsuredNameFEINService : IServiceProviders, IVInsuredNameFEINService
    {
        #region Comment : Here constructor

        public VInsuredNameFEINService(ServiceProvider provider)
        {
            base.ServiceProvider = provider;
        }

        #endregion

        #region Methods

        #region Public Methods

        public OperationStatus ValidateFeinNumber(VInsuredNameFEINRequestParms args)
        {
            var feinResponse = BHIC.Common.Client.SvcClient.CallService<BHIC.Domain.Policy.VInsuredNameFEINResponse>(string.Format("VInsuredNameFEIN?FEIN={0}", args.FEIN), ServiceProvider);

            return feinResponse.OperationStatus;
        }

        #endregion

        #endregion        
    }
}
