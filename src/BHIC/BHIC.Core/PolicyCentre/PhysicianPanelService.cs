using BHIC.Common;
using BHIC.Common.Client;
using BHIC.Common.Config;
using BHIC.Contract.PolicyCentre;
using BHIC.Contract.Provider;
using BHIC.Domain.Policy;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BHIC.Core.PolicyCentre
{
    public class PhysicianPanelService : IServiceProviders,IPhysicianPanelService
    {
        public PhysicianPanelService(ServiceProvider provider)
        {
            base.ServiceProvider = provider;
        }
        /// <summary>
        /// Returns list of Physician Documents based on PolicyCode and SessionId
        /// </summary>
        /// <param name="args"></param>
        /// <returns></returns>
        public List<Document> PhysicianPanelGet(PhysicianPanelRequestParms args)
        {
            var physicianPanelResponse = SvcClient.CallService<PhysicianPanelResponse>(string.Concat(Constants.PhysicianPanels,
                UtilityFunctions.CreateQueryString<PhysicianPanelRequestParms>(args)), ServiceProvider);

            if (physicianPanelResponse.OperationStatus.RequestSuccessful)
            {
                return physicianPanelResponse.PhysicianPanels;
            }
            else
            {
                throw new ApplicationException(UtilityFunctions.ConvertMessagesToString(physicianPanelResponse.OperationStatus.Messages));
            }
        }      
    }
}
