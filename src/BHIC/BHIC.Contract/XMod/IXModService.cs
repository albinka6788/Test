#region Using Directives

using BHIC.Domain.Background;
using BHIC.Domain.XMod;

#endregion

namespace BHIC.Contract.Background
{
    public interface IXModService
    {
        /// <summary>
        /// Get xmod factor for particular RiskId
        /// </summary>
        /// <param name="args"></param>
        /// <returns></returns>
        RiskXmodFactorResponse GetModRiskId(XModRequestParms args);

        /// <summary>
        /// Get xmod factor for particular FeinNumber
        /// </summary>
        /// <param name="args"></param>
        /// <returns></returns>
        RiskXmodFactorResponse GetModFein(XModRequestParms args);
    }
}
