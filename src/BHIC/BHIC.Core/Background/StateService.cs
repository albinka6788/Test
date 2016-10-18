using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using BHIC.Contract.Background;
using BHIC.Domain.Background;
using BHIC.Common;
using BHIC.Common.Client;
using BHIC.Common.Config;

namespace BHIC.Core.Background
{
    public class StateService : IStateService
    {
        #region Methods

        #region Public Methods

        /// <summary>
        /// Return list os states based on StateAbbr and StateName
        /// </summary>
        /// <param name="args"></param>
        /// <returns></returns>
        public List<State> GetStateList(StateRequestParms args,ServiceProvider serviceProvider)
        {
            var stateResponse = SvcClient.CallService<StateResponse>(string.Concat(Constants.States, UtilityFunctions.CreateQueryString<StateRequestParms>(args)), serviceProvider);

            if (stateResponse.OperationStatus.RequestSuccessful)
            {
                return stateResponse.States;
            }
            else
            {
                throw new ApplicationException(UtilityFunctions.ConvertMessagesToString(stateResponse.OperationStatus.Messages));
            }
        }

        #endregion

        #endregion
    }
}
