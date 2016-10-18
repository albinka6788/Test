using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using BHIC.Domain.Background;

namespace BHIC.Contract.Background
{
    public interface IAvailableCarriersService
    {
        /// <summary>
        /// Returns list of Available Carriers based on EffectiveDate, LOB
        /// </summary>
        /// <param name="args"></param>
        /// <returns></returns>
        List<string> GetAvailableCarriersList(AvailableCarriersRequestParms args);
    }
}
