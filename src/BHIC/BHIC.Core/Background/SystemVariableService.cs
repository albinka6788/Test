#region Using directives

using BHIC.Common.Caching;
using BHIC.Common.Client;
using BHIC.Common.Config;
using BHIC.Common.XmlHelper;
using BHIC.Contract.Background;
using BHIC.Contract.Provider;
using BHIC.Domain.Background;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Caching;
using System.Web;

#endregion

namespace BHIC.Core.Background
{
    public class SystemVariableService : IServiceProviders, ISystemVariableService
    {
        #region Variables

        CacheHelper cache;
        private readonly object cacheLock = new object();

        #endregion

        #region Constructor

        public SystemVariableService(ServiceProvider provider)
        {
            cache = CacheHelper.Instance;
            base.ServiceProvider = provider;
        }

        #endregion

        #region Methods

        #region Interface implementation

        /// <summary>
        /// It will fetch Session Variable list from cache
        /// </summary>
        /// <returns></returns>
        List<SystemVariable> ISystemVariableService.GetSystemVariables()
        {
            return GetSystemVariableListFromCacheOrService();
        }

        /// <summary>
        /// Get system variable value filterd by key
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        string ISystemVariableService.GetSystemVariableByKey(string key)
        {
            try
            {
                //if key is empty, return empty string
                if (!string.IsNullOrEmpty(key))
                {
                    List<SystemVariable> systemVariableList = GetSystemVariableListFromCacheOrService();

                    if (systemVariableList != null && systemVariableList.Count > 0 && systemVariableList.Any(res => res.Key.Equals(key, StringComparison.OrdinalIgnoreCase)))
                    {
                        //if key not found,return empty string
                        return systemVariableList.Where(res => res.Key.Equals(key, StringComparison.OrdinalIgnoreCase)).ToList()[0].Value;
                    }
                }

                return string.Empty;
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// Get all payement related system variables
        /// </summary>
        /// <returns>return payment related information</returns>
        List<SystemVariable> ISystemVariableService.GetPGCredentials()
        {
            List<SystemVariable> pgValues = new List<SystemVariable>();

            //fetch payment related key from config
            List<string> pgFilters = new List<string> 
                { 
                    ConfigCommonKeyReader.CreditCardPayments_AuthorizeNET_InTesting,
                    ConfigCommonKeyReader.CreditCardPayments_AuthorizeNET_LoginID,
                    ConfigCommonKeyReader.CreditCardPayments_AuthorizeNET_transactionKey
                };

            try
            {
                if (pgFilters != null)
                {
                    //Fetch all system variables
                    var systemVariable = ((ISystemVariableService)this).GetSystemVariables();

                    if (systemVariable != null)
                    {
                        //filter system variable list by given parameter
                        pgFilters.ForEach(res => pgValues.Add(new SystemVariable
                        {
                            Key = systemVariable.Where(x => x.Key.Equals(res)).FirstOrDefault().Key,
                            Value = systemVariable.Where(x => x.Key.Equals(res)).FirstOrDefault().Value
                        }));
                    }
                }
            }
            catch (Exception)
            {
                throw;
            }

            return pgValues;
        }

        #endregion

        #region Private Methods

        private List<SystemVariable> GetSystemVariableListFromCacheOrService()
        {
            lock (cacheLock)
            {
                try
                {
                    List<SystemVariable> systemVariable = cache.Get<List<SystemVariable>>(Constants.SystemVariableCache);

                    //if system variable does not exists,store it into cache
                    if (systemVariable == null)
                    {
                        SetSystemVariables();

                        return cache.Get<List<SystemVariable>>(Constants.SystemVariableCache);
                    }

                    return systemVariable;
                }
                catch (Exception)
                {
                    throw;
                }
            }
        }

        private void SetSystemVariables()
        {
            if (cache.Get<List<SystemVariable>>(Constants.SystemVariableCache) == null)
            {
                try
                {
                    var systemVariable = new List<SystemVariable>();

                    //fetch cache list from county api service
                    systemVariable = this.GetAllSystemVariables(new SystemVariableRequestParms
                    {
                        Agency = string.Empty,
                        Carrier = string.Empty,
                        Domain = ConfigCommonKeyReader.HostURL,
                        PolicyCode = string.Empty
                    });

                    //add cache item policy
                    CacheItemPolicy policy = new CacheItemPolicy();

                    //add expiration duration to 30  minutes
                    policy.AbsoluteExpiration = DateTimeOffset.Now.AddMinutes(Convert.ToDouble(ConfigCommonKeyReader.SystemVariableCacheDuration));

                    if (cache.IsExists(Constants.SystemVariableCache))
                    {
                        cache.Remove(Constants.SystemVariableCache);
                    }

                    //format MailingClaims_NewClaimPhone into US Phone number format
                    List<SystemVariable> phoneField = systemVariable.Where(x => x.Key.Equals(ConfigCommonKeyReader.ApplicationContactInfo["MailingClaims_NewClaimPhone"])).ToList();

                    if (phoneField != null && phoneField.Count > 0 && !phoneField[0].Value.Contains("-"))
                    {
                        phoneField[0].Value = String.Format("{0:###-###-####}", double.Parse(phoneField[0].Value));
                    }

                    //add item into cache
                    cache.Add(Constants.SystemVariableCache, systemVariable, policy);
                }
                catch (Exception ex)
                {
                    //log exception
                    throw new Exception(ex.Message);
                }
            }
        }

        /// <summary>
        /// Get the SystemVariable(s) specified by the associated request parameters
        /// </summary>
        /// <param name="args"></param>
        /// <returns></returns>
        private List<SystemVariable> GetAllSystemVariables(SystemVariableRequestParms args)
        {
            var systemVariableResponse = SvcClient.CallService<SystemVariableResponse>(string.Concat(Constants.SystemVariables,
                BHIC.Common.UtilityFunctions.CreateQueryString<SystemVariableRequestParms>(args)), ServiceProvider);

            if (systemVariableResponse.OperationStatus.RequestSuccessful)
            {
                return systemVariableResponse.SystemVariables;
            }
            else
            {
                throw new ApplicationException(BHIC.Common.UtilityFunctions.ConvertMessagesToString(systemVariableResponse.OperationStatus.Messages));
            }
        }

        #endregion

        #endregion
    }
}
