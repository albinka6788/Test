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
    public class ClassCodeService : IClassCodeService
    {
        #region Methods

        #region Public Methods

        /// <summary>
        /// Returns list of Class codes based on StateAbbr, Class Code, ClassSuffix
        /// </summary>
        /// <returns></returns>
        public List<ClassCode> GetClassCodeList(ClassCodeRequestParms args)
        {
            var classCodeResponse = SvcClientOld.CallService<ClassCodeResponse>(string.Concat(Constants.ServiceNames.ClassCodes, UtilityFunctions.CreateQueryString<ClassCodeRequestParms>(args)));

            if (classCodeResponse.OperationStatus.RequestSuccessful)
            {
                return classCodeResponse.ClassCodes;
            }
            else
            {
                throw new ApplicationException(UtilityFunctions.ConvertMessagesToString(classCodeResponse.OperationStatus.Messages));
            }
        }

        #endregion

        #endregion
    }
}
