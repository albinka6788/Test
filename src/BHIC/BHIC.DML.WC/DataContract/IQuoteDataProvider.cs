#region Using directives

using BHIC.DML.WC.DTO;

#endregion

namespace BHIC.DML.WC.DataContract
{
    public interface IQuoteDataProvider
    {
        /// <summary>
        /// Add new quote details 
        /// </summary>
        /// <param name="quote"></param>
        /// <returns></returns>
        bool AddQuoteDetails(QuoteDTO quote);

        /// <summary>
        /// Get existing quote details based on quote-number
        /// </summary>
        /// <param name="quoteNumber"></param>
        /// <returns></returns>
        QuoteDTO GetQuoteDetails(string quoteNumber);

        /// <summary>
        /// Get specified quote User ID
        /// </summary>
        /// <param name="quoteNumber"></param>
        /// <returns></returns>
        int GetQuoteUserId(string quoteNumber);

        /// <summary>
        /// Update quotes user-id refernce 
        /// </summary>
        /// <param name="user"></param>
        /// <param name="quote"></param>
        /// <returns></returns>
        bool UpdateQuoteUserId(OrganisationUserDetailDTO user, QuoteDTO quote);

        /// <summary>
        /// Update quotes organization-address-id refernce 
        /// </summary>
        /// <param name="user"></param>
        /// <param name="orgAddress"></param>
        /// <param name="quote"></param>
        /// <returns></returns>
        bool UpdateQuoteOrganizationAddressId(OrganisationUserDetailDTO user, OrganisationAddress orgAddress, QuoteDTO quote);


        /// <summary>
        /// Insert CYB Class Search Keywords
        /// </summary>
        /// <param name="cybClassKeyword"></param>
        /// <returns></returns>
        bool AddClassKeywords(string cybClassKeyword);
    }
}
