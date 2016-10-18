#region Use Directive

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

#endregion

namespace BHIC.Contract.PurchasePath
{
    public interface ILossControlFileService
    {
      /// <summary>
      /// To Fetch Loss Contol File Name from Guid
      /// </summary>
      /// <param name="Guid"></param>
      /// <returns></returns>
       string GetLossControlFileName(string Guid);
    }
}
