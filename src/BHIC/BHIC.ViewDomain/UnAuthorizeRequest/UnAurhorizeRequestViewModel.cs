using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BHIC.ViewDomain
{
    public class UnAurhorizeRequestViewModel
    {        
        #region Comment : Here constructor / initialization

        public UnAurhorizeRequestViewModel()
        {
            // init objects to help avoid issues related to null reference exceptions
            ErrorMessage = string.Empty;
            ErrorHtml = string.Empty;
        }

        #endregion

        #region Variables : Page Level Local Variables Decalration

        public string ErrorMessage { get; set; }
        public string ErrorHtml { get; set; }

        #endregion
    }
}
