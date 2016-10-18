using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BHIC.Contract.Background
{
    /// <summary>
    /// This contract is for the saver for later background process
    /// </summary>
    public interface IBackgroundSaveForLaterService
    {
        void TriggerSaveForLaterMail();
      
    }
}
