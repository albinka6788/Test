#region Using directives

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using BLL = BHIC.DML.WC.DTO;

#endregion

namespace BHIC.DML.WC.DataContract
{
    public interface ICustomSession
    {
        /// <summary>
        /// Add or update application current session data into DB layer
        /// </summary>
        /// <param name="customSession"></param>
        /// <returns></returns>
        bool AddCustomSession(BLL::CustomSession customSession);

        /// <summary>
        /// Retrieve string data of stored quote session data
        /// </summary>
        /// <param name="quoteId"></param>
        /// <returns></returns>
        string GetCustomSession(int quoteId,int userId);
    }
}