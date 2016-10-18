using BHIC.Common.Client;
using BHIC.Contract.Background;
using BHIC.Contract.Policy;
using BHIC.Core.Background;
using BHIC.Core.Policy;
using BHIC.DML.WC.DataContract;
using BHIC.DML.WC.DataService;
using BHIC.DML.WC.DTO;
using BHIC.Domain.Background;
using BHIC.Domain.Policy;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BHIC.Core.PurchasePath
{
    public class PurchaseQuote
    {
        ServiceProvider guardServiceProvider = new GuardServiceProvider() { ServiceCategory = ServiceProviderConstants.GuardServiceCategoryWC };

        public Quote GetPurchaseQuoteData(int quoteId, string policyId)
        {
            IQuoteService quoteService = new QuoteService(guardServiceProvider);
            var resp = quoteService.GetQuote(new QuoteRequestParms { QuoteId = quoteId, PolicyId = policyId ,
                                                                     IncludeRelatedPolicyData = false,
                                                                     IncludeRelatedRatingData = false,
                                                                     IncludeRelatedPaymentTerms = false,
                                                                     IncludeRelatedInsuredNames = true,
                                                                     IncludeRelatedExposuresGraph = false,
                                                                     IncludeRelatedOfficers = false,
                                                                     IncludeRelatedLocations = true,
                                                                     IncludeRelatedContactsGraph = true,
                                                                     IncludeRelatedQuestions = false,
                                                                     IncludeRelatedQuoteStatus = false
            });
            return resp;
        }

        public bool CreateAccount(OrganisationUserDetailDTO user, out Int64? organisationUserId)
        {
            IOrganisationUserDetailDataProvider organisationUserDetailDataProvider = new OrganisationUserDetailDataProvider();
            return organisationUserDetailDataProvider.AddOrganisationUserDetail(user, out organisationUserId);
        }

        public bool MaintainUserAccount(OrganisationUserDetailDTO user, out Int64? organisationUserId)
        {
            IOrganisationUserDetailDataProvider organisationUserDetailDataProvider = new OrganisationUserDetailDataProvider();
            return organisationUserDetailDataProvider.MaintainUserAccountDetail(user, out organisationUserId);
        }

        public bool UpdateQuoteUserId(OrganisationUserDetailDTO user, QuoteDTO quote)
        {
            IQuoteDataProvider quoteDataProvider = new QuoteDataProvider();
            return quoteDataProvider.UpdateQuoteUserId(user, quote);
        }

        public OrganisationUserDetailDTO GetUserCredentialDetails(OrganisationUserDetailDTO organisationUser)
        {
            IOrganisationUserDetailDataProvider organisationUserDetailDataProvider = new OrganisationUserDetailDataProvider();
            return organisationUserDetailDataProvider.GetUserCredentialDetails(organisationUser);
        }

        public List<CoverageState> GetCoverageStatesData(int quoteId)
        {
            ICoverageStateService quoteService = new CoverageStateService(guardServiceProvider);
            var resp = quoteService.GetCoverageStateList(new CoverageStateRequestParms { QuoteId = quoteId, IncludeRelated = true });
            return resp;
        }

        public StateUINumberClass GetStatesData(string stateAbbr, string stateName)
        {
            IStateService stateService = new StateService();
            StateUINumberClass suiNumber = null;
            try
            {
                var stateResponse = stateService.GetStateList(new StateRequestParms { StateAbbr = stateAbbr, StateName = stateName }, guardServiceProvider);
                if (stateResponse != null && stateResponse.Count > 0)
                {
                    if (stateResponse.Any(x => x.EmployerCodeRequired == true))
                    {
                        suiNumber = new StateUINumberClass();
                        if (stateResponse.Any(x => x.StateName == "Utah" || x.StateName == "Minnesota"))
                        {
                            suiNumber.InputMask = "9999999";
                            suiNumber.PlaceHolder = "0000000";
                            suiNumber.SUILength = 7;
                        }
                        else
                        {
                            suiNumber.InputMask = "9999999999";
                            suiNumber.PlaceHolder = "0000000000";
                            suiNumber.SUILength = 10;
                        }

                    }
                    else
                    {
                        return null;
                    }

                }
            }
            catch (Exception ex)
            {
                BHIC.Common.Logging.ILoggingService logger = BHIC.Common.Logging.LoggingService.Instance;
                logger.Fatal(string.Format("Service {0} Call with error message : {1}", "StateService GET", ex.ToString()));
            }
            return suiNumber;
        }

        public BHIC.DML.WC.DTO.OrganisationUserDetailDTO GetUserCredentials(string emailId)
        {
            BHIC.DML.WC.DTO.OrganisationUserDetailDTO organisationUser = new DML.WC.DTO.OrganisationUserDetailDTO();
            organisationUser.EmailID = emailId;

            //Comment : Here get user data 

            BHIC.DML.WC.DTO.OrganisationUserDetailDTO orgUserCredential = GetUserCredentialDetails(organisationUser);
            return orgUserCredential;
        }
    }
}
