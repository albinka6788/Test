/*      
@Author      : Sreeram       
@Description : This procedure will policy id based on email     
@CreatedOn   : 2015-Oct-29       
      
@Updation History :       
   @Update-1 : 2015-Oct-29, Sreeram, created
   @update-2 : 2015-Oct-30, Gurpreet, retrive policy number from user table if not found then from policy table
   @update-3 : 2015-Dec-28, Gurpreet, check for user is active or not
   @update-4 : 2016-Jan-04, Guru, Fixed sorting issue
   @update-5 : 2016-Feb-02, Venkatesh, added LOB field in select query
   @update-6 : 2016-Mar-17, Albin, Removed group by from the query   
   @update-7 : 2016-Aug-04, Albin, 1. Created innerjoin with UserQuotes table to fetch all the policies to acheive many to many relationship for impersonation feature.
								   2. Fetching policies which are not expired w.r.t current date or whose end date is null
@ExampleCall : EXEC GetPolicyID 'vijay@chauhan.com'      
      
*/      
-- ================================================  

ALTER PROCEDURE [dbo].[GetPolicyID] 
(        
  @Email VARCHAR(150)    
)        
AS         
BEGIN        
        
 -- SET NOCOUNT ON added to prevent extra result sets from        
 SET NOCOUNT ON;   
	Select PolicyNumber as PolicyID,quote.LineOfBusinessId LOB 
	FROM OrganisationUserDetail Org	
	inner join UserQuotes usrquotes on Org.id=usrquotes.OrganizationUserDetailID   
	inner join Quote quote on usrquotes.QuoteId = quote.id 
	inner join Policy policy on quote.id=policy.QuoteID
	WHERE Org.EmailID = @Email and policy.IsActive=1 
	and (GETDATE()< EndDate or EndDate is null)
	order by policy.Id desc
END

 