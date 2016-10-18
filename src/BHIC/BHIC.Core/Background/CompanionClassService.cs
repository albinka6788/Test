using BHIC.Common;
using BHIC.Common.Client;
using BHIC.Common.Config;
using BHIC.Contract.Background;
using BHIC.Contract.Provider;
using BHIC.Domain.Background;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BHIC.Core.Background
{
    public class CompanionClassService : IServiceProviders, ICompanionClassService, IDisposable
    {
        public CompanionClassService(ServiceProvider provider)
        {
            base.ServiceProvider = provider;
        }

        /// <summary>
        /// Get Companion Classes
        /// </summary>
        /// <param name="args"></param>
        /// <returns></returns>
        public List<CompanionClass> GetCompanionClasses(CompanionClassRequestParms args)
        {
            var companionClassResponse = SvcClient.CallService<CompanionClassResponse>(string.Concat(Constants.ServiceNames.CompanionClasses,
                UtilityFunctions.CreateQueryString<CompanionClassRequestParms>(args)), ServiceProvider);

            if (companionClassResponse.OperationStatus.RequestSuccessful)
            {
                if (!companionClassResponse.CompanionClasses.IsNull() && companionClassResponse.CompanionClasses.Count > 0)
                {
                    return companionClassResponse.CompanionClasses.Where(x => !string.IsNullOrWhiteSpace(x.PromptText) && !string.IsNullOrEmpty(x.PromptText)).ToList();
                }
                else
                {
                    return null;
                }
            }
            else
            {
                throw new ApplicationException(UtilityFunctions.ConvertMessagesToString(companionClassResponse.OperationStatus.Messages));
            }
        }

        /// <summary>
        /// Dispose the Object
        /// </summary>
        public void Dispose()
        {
            this.Dispose();
            GC.SuppressFinalize(this);
        }
    }
}
