#region Use Directive

using System;
using System.Collections.Generic;

using BLL = BHIC.Contract.PurchasePath;
using DML_DC = BHIC.DML.WC.DataContract;
using DML_DTO = BHIC.DML.WC.DTO;
using DML_DS = BHIC.DML.WC.DataService;

using VM = BHIC.ViewDomain;
using Newtonsoft.Json;
using BHIC.Common;
using BHIC.Domain.Service;
using BHIC.Contract.Policy;
using BHIC.Core.Policy;
using BHIC.Domain.Policy;
using BHIC.Common.Client;
using BHIC.Domain.Background;
using BHIC.Common.Config;
using BHIC.Contract.PurchasePath;

#endregion

namespace BHIC.Core.PurchasePath
{
    public class CommonFunctionality : BLL::ICommonFunctionality
    {
        internal ServiceProvider guardServiceProvider = new GuardServiceProvider() { ServiceCategory = ServiceProviderConstants.GuardServiceCategoryWC };

        #region Methods

        /// <summary>
        /// Add or update application current session data into DB layer
        /// </summary>
        /// <param name="customSession"></param>
        /// <returns></returns>
        public bool AddApplicationCustomSession(DML_DTO::CustomSession CustomSession)
        {
            #region Comment : Here CustomSession interface refernce to do/make process all DB logic

            DML_DC::ICustomSession customSession = GetCustomSessionProviderDML();

            #endregion

            return customSession.AddCustomSession(CustomSession);
        }

        /// <summary>
        /// Retrieve string data of stored quote session data
        /// </summary>
        /// <param name="quoteId"></param>
        /// <returns></returns>
        public string GetApplicationCustomSession(int quoteId, int userId)
        {
            #region Comment : Here CustomSession interface refernce to do/make process all DB logic

            DML_DC::ICustomSession customSession = GetCustomSessionProviderDML();

            #endregion

            return customSession.GetCustomSession(quoteId, userId);
        }

        /// <summary>
        /// Method will stringify custom session object into string 
        /// </summary>
        /// <param name="customSession"></param>
        public string StringifyCustomSession(VM::CustomSession customSession)
        {
            return JsonConvert.SerializeObject(customSession);
        }

        /// <summary>
        /// Method will convert phone number from old mask format to new format
        /// </summary>
        /// <param name="phoneNumber"></param>
        /// <returns></returns>     
        public static string RemoveOlderPhoneMask(string phoneNumber)
        {
            return phoneNumber.Replace(")(", "-").Replace("(", "").Replace(")", "");
        }

        /// <summary>
        /// Method will return deserialize custom session object from stringified object of custom session
        /// </summary>
        /// <param name="customSession"></param>
        public VM::CustomSession GetDeserializedCustomSession(string customSession)
        {
            return JsonConvert.DeserializeObject<VM::CustomSession>(customSession);
        }

        /// <summary>
        /// To Fetch Loss Contol File Name from Guid
        /// </summary>
        /// <param name="Guid"></param>
        /// <returns></returns>
        public string GetLossControlFileName(string Guid)
        {
            ILossControlFileService lCFile = new LossControlFileService();
            return lCFile.GetLossControlFileName(Guid);
        }


        #region Additional Methods

        /// <summary>
        /// Return interface refernce to make all database manipulation language(DML) logic
        /// </summary>
        /// <returns></returns>
        private DML_DC::ICustomSession GetCustomSessionProviderDML()
        {
            #region Comment : Here Questionnaire interface refernce to do/make process all business logic

            DML_DC::ICustomSession customSession = new DML_DS::CustomSessionService();

            #endregion

            return customSession;
        }

        /// <summary>
        /// Validate FEIN/SSN/TIN number
        /// </summary>
        /// <param name="taxIdNumber"></param>
        /// <returns>return true if valid string, false otherwise</returns>
        public bool ValidateTaxIdAndSSN(string taxIdNumber)
        {
            bool validated = false;

            if (!string.IsNullOrEmpty(taxIdNumber))
            {
                try
                {
                    taxIdNumber = UtilityFunctions.ToNumeric(taxIdNumber).Trim();

                    validated = taxIdNumber.Length > 0 ? true : false;

                    //if given id is valid and has valid ssn number, return true
                    if (validated && ValidateFeinTinSsnNumber(taxIdNumber).RequestSuccessful)
                    {
                        return true;
                    }
                }
                catch (Exception)
                {
                    throw;
                }
            }

            return false;
        }

        /// <summary>
        /// Return tax-id validation messages
        /// </summary>
        /// <param name="taxId"></param>
        /// <returns></returns>
        private OperationStatus ValidateFeinTinSsnNumber(string taxId)
        {
            IVInsuredNameFEINService vInsuredNameFEINService = new VInsuredNameFEINService(guardServiceProvider);

            return vInsuredNameFEINService.ValidateFeinNumber(new VInsuredNameFEINRequestParms { FEIN = taxId });
        }

        /// <summary>
        /// Compare PaymentPlan objects
        /// </summary>
        /// <param name="selectedPaymentPlan"></param>
        /// <param name="matchedPaymentPlan"></param>
        /// <returns></returns>
        public List<string> ComparePlanObjects(PaymentPlan selectedPaymentPlan, PaymentPlan matchedPaymentPlan)
        {
            var listOfErrors = new List<string>();

            if (matchedPaymentPlan != null)
            {
                //Get the type of the object
                Type type = typeof(PaymentPlan);

                #region Loop through each properties inside class and get values for the property from both the objects and compare

                foreach (System.Reflection.PropertyInfo property in type.GetProperties())
                {
                    string Object1Value = string.Empty;
                    string Object2Value = string.Empty;

                    if (type.GetProperty(property.Name).GetValue(selectedPaymentPlan, null) != null)
                    {
                        if (type.GetProperty(property.Name).PropertyType == typeof(decimal))
                        {
                            Object1Value = String.Format("{0:0.00}", type.GetProperty(property.Name).GetValue(selectedPaymentPlan, null));
                        }
                        else
                        {
                            Object1Value = Convert.ToString(type.GetProperty(property.Name).GetValue(selectedPaymentPlan, null));
                        }
                    }

                    if (type.GetProperty(property.Name).GetValue(matchedPaymentPlan, null) != null)
                    {
                        if (type.GetProperty(property.Name).PropertyType == typeof(decimal))
                        {
                            Object2Value = String.Format("{0:0.00}", type.GetProperty(property.Name).GetValue(matchedPaymentPlan, null));
                        }
                        else
                        {
                            Object2Value = Convert.ToString(type.GetProperty(property.Name).GetValue(matchedPaymentPlan, null));
                        }
                    }

                    //Comment : Here if any one porperty value has not been matched then stop execution & return error to calle
                    if (Object1Value.Trim() != Object2Value.Trim())
                    {
                        listOfErrors.Add(Constants.PaymentPlanDoesNotExists);
                        break;
                    }
                }

                #endregion
            }
            else
            {
                listOfErrors.Add(Constants.PaymentPlanDoesNotExists);
            }

            return listOfErrors;
        }

        #endregion

        #endregion
    }
}
