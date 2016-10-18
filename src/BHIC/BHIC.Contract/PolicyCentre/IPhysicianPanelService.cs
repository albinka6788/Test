using BHIC.Common.Client;
using BHIC.Domain.Policy;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BHIC.Contract.PolicyCentre
{
    public interface IPhysicianPanelService
    {
        /// <summary>
        /// PhysicianPanelGet to get the list of Physician Documents
        /// </summary>
        /// <param name="args"></param>
        /// <returns></returns>
        List<Document> PhysicianPanelGet(PhysicianPanelRequestParms args);
    }
}
