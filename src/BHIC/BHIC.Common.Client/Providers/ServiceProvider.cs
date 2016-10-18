using System.Net;

namespace BHIC.Common.Client
{    
    public class ServiceProvider
    {
        #region Comment : Here variables/properties declaration

        public ProviderNames ProviderName { get; set; }

        public string ServiceCategory { get; set; }

        public bool IsActive { get; set; }

        public bool SupportBatchProcess { get; set; }

        public enum OperationType
        {
            /// <summary>
            /// POST Operation
            /// </summary>
            POST = 1,

            /// <summary>
            /// PUT Operation
            /// </summary>
            PUT = 2,

            /// <summary>
            /// GET Operation
            /// EXAMPLE: 'Your policy has been created!'
            /// </summary>
            GET = 3,

            /// <summary>
            /// DELETE Operation
            /// </summary>
            DELETE = 4,
        }

        #endregion
        
        #region Comment : Here class constructor

        //Constructor
        public ServiceProvider() { }

        #endregion

        #region Comment : Here class public methods

        /// <summary>
        /// Return string of calling api URL prefix based on Provider "Config Section Name" and "Section Service Element" name
        /// </summary>
        /// <returns></returns>
        public virtual string ServiceUrl()
        {
            string ApiUrl = null;

            return string.IsNullOrEmpty(ApiUrl) == true ? "" : ApiUrl;
        }

        /// <summary>
        /// Method will set web client headers based on provider and provider service category
        /// </summary>
        /// <param name="serviceMethod"></param>
        /// <param name="client"></param>
        public virtual void SetWebClientHeaders(OperationType serviceMethod, ref WebClient client)
        {
            
        }

        /// <summary>
        /// Overload Method will reattempt and set web client headers based on provider and provider service category
        /// </summary>
        /// <param name="serviceMethod"></param>
        /// <param name="client"></param>
        /// <param name="reAttemptNumber"></param>
        public virtual void SetWebClientHeaders(OperationType serviceMethod, ref WebClient client, int reAttemptNumber)
        {

        }

        /// <summary>
        /// Method will set web client headers for "Batch POST" based on provider and provider service category
        /// </summary>
        /// <param name="serviceMethod"></param>
        /// <param name="client"></param>
        public virtual void SetBatchWebClientHeaders(OperationType serviceMethod, ref WebClient client)
        {

        }

        /// <summary>
        /// Overload Method will reattempt and set web client headers for "Batch POST" based on provider and provider service category
        /// </summary>
        /// <param name="serviceMethod"></param>
        /// <param name="client"></param>
        /// <param name="reAttemptNumber"></param>
        public virtual void SetBatchWebClientHeaders(OperationType serviceMethod, ref WebClient client, int reAttemptNumber)
        {

        }

        /// <summary>
        /// Method to prepare header based on serice method type and requirement
        /// </summary>
        /// <param name="serviceMethod"></param>
        /// <param name="client"></param>
        public virtual void SetBatchNameValueCollection(BHIC.Domain.Service.BatchActionList batchActionList, ref System.Collections.Specialized.NameValueCollection inputs)
        {
            //Comment : here based on provider set batch collection
        }

        #endregion
    }
}
