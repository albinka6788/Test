#region Using directives

using System;
using System.Collections.Generic;

using BHIC.Contract.Policy;
using BHIC.Domain.Policy;
using BHIC.Common;
using BHIC.Common.Config;
using BHIC.Domain.Service;
using BHIC.Common.Client;
using BHIC.Contract.Provider;
using Newtonsoft.Json;
using System.Linq;
using BHIC.Domain.PolicyCentre;

#endregion

namespace BHIC.Core.Policy
{
    public class QuoteService : IServiceProviders, IQuoteService
    {

        public QuoteService(ServiceProvider provider)
        {
            base.ServiceProvider = provider;
        }

        #region Methods

        #region Public Methods

        public Quote GetQuote(QuoteRequestParms args)
        {
            var quoteStatusResponse = SvcClient.CallService<QuoteResponse>(string.Concat(Constants.Quote, UtilityFunctions.CreateQueryString<QuoteRequestParms>(args)), ServiceProvider);

            if (quoteStatusResponse.OperationStatus.RequestSuccessful)
            {
                return quoteStatusResponse.Quote;
            }
            else
            {
                throw new ApplicationException(UtilityFunctions.ConvertMessagesToString(quoteStatusResponse.OperationStatus.Messages));
            }
        }

        public Quote GetQuote(QuoteRequestParms args, bool raiseExeption)
        {
            var quoteStatusResponse = SvcClient.CallService<QuoteResponse>(string.Concat(Constants.Quote, UtilityFunctions.CreateQueryString<QuoteRequestParms>(args)), ServiceProvider);

            if (raiseExeption)
            {
                if (quoteStatusResponse.OperationStatus.RequestSuccessful)
                {
                    return quoteStatusResponse.Quote;
                }
                else
                {
                    throw new ApplicationException(UtilityFunctions.ConvertMessagesToString(quoteStatusResponse.OperationStatus.Messages));
                }
            }
            return quoteStatusResponse.Quote;
        }

        public List<Quote> GetContactFromQuoteList(List<string> policyNumber)
        {
            List<Quote> lstQuoteResponse = new List<Quote>();
            //QuoteResponse lstQuoteResponse = null;
            #region Comment : Here create batch actions

            /*----------------------------------------
                    populate a BatchActionList that contains all requests to be sent to the Gaurd API
                    ----------------------------------------*/

            var batchActionList = new BatchActionList();	// BatchActionList: list of actions routed to the Insurance Service for response
            BatchResponseList batchResponseList;			// BatchResponseList: list of responses returned from the Insurance Service
            string jsonResponse;							// JSON-formated response data returned from the Insurance Service

            // populate a BatchAction that will be used to submit the InsuredName DTO to the Insurance Service

            foreach (var item in policyNumber)
            {
                var quoteDetailsAction = new BatchAction { ServiceName = Constants.Quote, ServiceMethod = "GET", RequestIdentifier = "Get Quote Details " + item };
                quoteDetailsAction.BatchActionParameters.Add(new BatchActionParameter
                {
                    Name = "QuoteRequestParms",
                    Value = JsonConvert.SerializeObject(new QuoteRequestParms
                        {
                            PolicyId = item,
                            IncludeRelatedPolicyData = true,
                            IncludeRelatedRatingData = false,
                            IncludeRelatedPaymentTerms = false,
                            IncludeRelatedInsuredNames = false,
                            IncludeRelatedExposuresGraph = false,
                            IncludeRelatedOfficers = false,
                            IncludeRelatedLocations = false,
                            IncludeRelatedContactsGraph = true,
                            IncludeRelatedQuestions = false,
                            IncludeRelatedQuoteStatus = false
                        })
                });
                batchActionList.BatchActions.Add(quoteDetailsAction);
            }


            #endregion

            // submit the BatchActionList to the Insurance Service
            jsonResponse = SvcClient.CallServiceBatch(batchActionList, ServiceProvider);

            // deserialize the results into a BatchResponseList
            batchResponseList = JsonConvert.DeserializeObject<BatchResponseList>(jsonResponse);

            // deserialize each response returned by the service, and copy the OperationStatus to the view model for demo purposes
            OperationStatus operationStatus = new OperationStatus();
            if (batchResponseList != null)
            {
                foreach (var item in policyNumber)
                {
                    var quoteDataResponse = batchResponseList.BatchResponses.SingleOrDefault(m => m.RequestIdentifier == "Get Quote Details " + item).JsonResponse;
                    var quoteResponse = JsonConvert.DeserializeObject<QuoteResponse>(quoteDataResponse);
                    lstQuoteResponse.Add(quoteResponse.Quote);
                }
            }

            return lstQuoteResponse;
        }

        public List<QuoteResponse> GetQuoteList(List<string> quoteNumber)
        {
            List<QuoteResponse> lstQuoteResponse = null;
            #region Comment : Here create batch actions

            /*----------------------------------------
                    populate a BatchActionList that contains all requests to be sent to the Gaurd API
                    ----------------------------------------*/

            var batchActionList = new BatchActionList();	// BatchActionList: list of actions routed to the Insurance Service for response
            BatchResponseList batchResponseList;			// BatchResponseList: list of responses returned from the Insurance Service
            string jsonResponse;							// JSON-formated response data returned from the Insurance Service

            // populate a BatchAction that will be used to submit the InsuredName DTO to the Insurance Service

            foreach (var item in quoteNumber)
            {
                var quoteDetailsAction = new BatchAction { ServiceName = Constants.Quote, ServiceMethod = "GET", RequestIdentifier = "Get Quote Details " + item };
                quoteDetailsAction.BatchActionParameters.Add(new BatchActionParameter { Name = "QuoteRequestParms", Value = JsonConvert.SerializeObject(new QuoteRequestParms { QuoteId = Convert.ToInt32(item) }) });
                batchActionList.BatchActions.Add(quoteDetailsAction);
            }


            #endregion

            // submit the BatchActionList to the Insurance Service
            jsonResponse = SvcClient.CallServiceBatch(batchActionList, ServiceProvider);

            // deserialize the results into a BatchResponseList
            batchResponseList = JsonConvert.DeserializeObject<BatchResponseList>(jsonResponse);

            // deserialize each response returned by the service, and copy the OperationStatus to the view model for demo purposes
            OperationStatus operationStatus = new OperationStatus();
            if (batchResponseList != null)
            {
                lstQuoteResponse = new List<QuoteResponse>();
                foreach (var item in quoteNumber)
                {
                    var quoteDataResponse = batchResponseList.BatchResponses.SingleOrDefault(m => m.RequestIdentifier == "Get Quote Details " + item).JsonResponse;
                    var quoteResponse = JsonConvert.DeserializeObject<QuoteResponse>(quoteDataResponse);
                    lstQuoteResponse.Add(quoteResponse);
                }
            }

            return lstQuoteResponse;
        }

        public List<UserQuote> ViewSavedQuoteList(QuoteRequestParms args, List<UserQuote> lstUserQuote)
        {
            string zipCode = "-", businessType = "-";
            var quoteStatusResponse = SvcClient.CallService<QuoteResponse>(string.Concat(Constants.Quote, UtilityFunctions.CreateQueryString<QuoteRequestParms>(args)), ServiceProvider);
            if (quoteStatusResponse.OperationStatus.RequestSuccessful)
            {
                for (int i = 0; i < lstUserQuote.Count; i++)
                {
                    Quote quote = quoteStatusResponse.Quotes.Where(t => t.PolicyData.QuoteId == Convert.ToInt32(lstUserQuote[i].QuoteID)).FirstOrDefault();

                    if (quote != null)
                    {
                        if (quote.LobDataList.FirstOrDefault() != null)
                        {
                            if (quote.LobDataList.FirstOrDefault().CoverageStates.FirstOrDefault() != null)
                            {
                                var exposure = quote.LobDataList.FirstOrDefault().CoverageStates.FirstOrDefault().Exposures;

                                if (exposure != null && exposure.Count > 0)
                                {
                                    zipCode = exposure[0].ZipCode ?? (lstUserQuote[i].ZipCode ?? zipCode);
                                    if (exposure[0].ClassDescription != null)
                                    {
                                        businessType = (exposure[0].ClassDescriptionKeyword != null ?
                                            exposure[0].ClassDescriptionKeyword.Keyword :
                                           (!string.IsNullOrEmpty(exposure[0].ClassDescription.Description) ?
                                           exposure[0].ClassDescription.Description : exposure[0].ClassDescription.FriendlyLabel));
                                    }
                                }
                            }
                        }
                        lstUserQuote[i].ZipCode = zipCode;
                        //GUIN - 436 to show other if there is no class keyword selected
                        lstUserQuote[i].BusinessType = lstUserQuote[i].ClassDescriptionKeywordId == -1 ? "Others" : businessType ?? "-";
                        lstUserQuote[i].QuotedDate = quote.PolicyData.DateUpdated ?? lstUserQuote[i].QuotedDate;
                        lstUserQuote[i].PremiumAmt = quote.RatingData.Premium ?? 0;
                    }
                    else
                    {
                        //comment below line as per prem and neha show those records also either guard have data or not.
                        //lstUserQuote.RemoveAt(i);
                        //i--;
                    }
                    if (lstUserQuote[i].ClassDescriptionKeywordId == -1)
                    {
                        lstUserQuote[i].BusinessType = "Others";
                        lstUserQuote[i].PremiumAmt = 0;
                    }
                }

                return lstUserQuote;
            }
            else
            {
                throw new ApplicationException(UtilityFunctions.ConvertMessagesToString(quoteStatusResponse.OperationStatus.Messages));
            }
        }
        /// <summary>
        /// To get the BOP Policy Zip Code & Business Class information.
        /// </summary>
        /// <param name="args"></param>
        /// <param name="lstUserQuote"></param>
        /// <returns></returns>
        public List<UserQuote> ViewSavedBOPQuoteList(PCQuoteInformationRequestParms args, List<UserQuote> lstUserQuote)
        {
            var quoteStatusResponse = SvcClient.CallService<PCQuoteInformationResponse>(string.Concat(Constants.PCQuoteInformation, UtilityFunctions.CreateQueryString<PCQuoteInformationRequestParms>(args)), ServiceProvider);
            if (quoteStatusResponse.OperationStatus.RequestSuccessful)
            {
                foreach (PCQuoteInformation bopQuote in quoteStatusResponse.PCQuoteInformationList)
                {
                    UserQuote userQuote = lstUserQuote.Where(t => t.QuoteID == bopQuote.QuoteId.ToString()).FirstOrDefault();
                    if (userQuote != null)
                    {
                        userQuote.BusinessType = bopQuote.ClassDescription;
                        userQuote.ZipCode = bopQuote.ZipCode;
                    }
                }
                return lstUserQuote;
            }
            else
            {
                throw new ApplicationException(UtilityFunctions.ConvertMessagesToString(quoteStatusResponse.OperationStatus.Messages));
            }
        }



        #endregion

        #endregion
    }
}
