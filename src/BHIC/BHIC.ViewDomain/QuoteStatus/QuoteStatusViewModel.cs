using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using BHIC.Domain.Background;
using BHIC.Domain.Policy;
using BHIC.ViewDomain.QuestionEngine;
using BHIC.ViewDomain.Landing;

namespace BHIC.ViewDomain
{
    public class QuoteStatusViewModel
    {
        #region Comment : Here constructor / initialization

        public QuoteStatusViewModel()
        {
            // init objects to help avoid issues related to null reference exceptions
        }

        #endregion

        #region Variables : Page Level Local Variables Decalration

        public DateTime? LandingSaved { get; set; }
        public DateTime? ClassesSelected { get; set; }
        public DateTime? ExposuresSaved { get; set; }
        public DateTime? QuestionsSaved { get; set; }
        public DateTime? ContactRequested { get; set; }

        #endregion
    }
}
