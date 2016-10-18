#region Using directives

using BHIC.Domain.Policy;
using BHIC.Domain.Service;
using System;
using System.Collections.Generic;

#endregion

namespace BHIC.Contract.Policy
{
    public interface IModifierService
    {
        /// <summary>
        /// Returns list of Modifier based on QuoteId,Lob,State and ModifierId
        /// </summary>
        /// <param name="args"></param>
        /// <returns></returns>
        List<Modifier> GetModifierList(ModifierRequestParms args);

        /// <summary>
        /// Save new Modifier details
        /// </summary>
        /// <param name="args"></param>
        /// <returns></returns>
        OperationStatus AddModifier(Modifier args);

        /// <summary>
        /// Delete existing Modifier details
        /// </summary>
        /// <param name="args"></param>
        /// <returns></returns>
        OperationStatus DeleteModifier(ModifierRequestParms args);
    }
}
