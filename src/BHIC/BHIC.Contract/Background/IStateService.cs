using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using BHIC.Domain.Background;
using BHIC.Common.Client;

namespace BHIC.Contract.Background
{
    public interface IStateService
    {
        /// <summary>
        /// Return list os states based on StateAbbr and StateName
        /// </summary>
        /// <param name="args"></param>
        /// <returns></returns>
        List<State> GetStateList(StateRequestParms args, ServiceProvider serviceProvider);
    }
}
