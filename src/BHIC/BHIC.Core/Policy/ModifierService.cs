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
    public class ModifierService : IModifierService
    {
        #region Methods

        #region Public Methods

        /// <summary>
        /// Returns list of Modifier based on QuoteId,Lob,State and ModifierId
        /// </summary>
        /// <param name="args"></param>
        /// <returns></returns>
        public List<Modifier> GetModifierList(ModifierRequestParms args)
        {
            var modifierResponse = SvcClientOld.CallService<ModifierResponse>(string.Concat(Constants.Modifier,
                UtilityFunctions.CreateQueryString<ModifierRequestParms>(args)));

            if (modifierResponse.OperationStatus.RequestSuccessful)
            {
                return modifierResponse.Modifiers;
            }
            else
            {
                throw new ApplicationException(UtilityFunctions.ConvertMessagesToString(modifierResponse.OperationStatus.Messages));
            }
        }

        /// <summary>
        /// Save new Modifier details
        /// </summary>
        /// <param name="args"></param>
        /// <returns></returns>
        public OperationStatus AddModifier(Modifier args)
        {
            var modifierResponse = SvcClientOld.CallService<Modifier, OperationStatus>(Constants.Modifier, "POST", args);

            if (modifierResponse.RequestSuccessful)
            {
                return modifierResponse;
            }
            else
            {
                throw new ApplicationException(UtilityFunctions.ConvertMessagesToString(modifierResponse.Messages));
            }
        }

        /// <summary>
        /// Delete existing Modifier details
        /// </summary>
        /// <param name="args"></param>
        /// <returns></returns>
        public OperationStatus DeleteModifier(ModifierRequestParms args)
        {
            var modifierResponse = SvcClientOld.CallService<ModifierRequestParms, OperationStatus>(Constants.Modifier, "DELETE", args);

            if (modifierResponse.RequestSuccessful)
            {
                return modifierResponse;
            }
            else
            {
                throw new ApplicationException(UtilityFunctions.ConvertMessagesToString(modifierResponse.Messages));
            }

        }

        #endregion

        #endregion
    }
}
