/*      
@Author      : Sreeram       
@Description : This procedure will policy id based on email     
@CreatedOn   : 2015-Oct-29       
      
@Updation History :       
   @Update-1 : 2015-Oct-29, Sreeram, created
   @update-2 : 2015-Oct-30, Gurpreet, retrive policy number from user table if not found then from policy table
   @update-3 : 2015-Dec-28, Gurpreet, check for user is active or not
      
@ExampleCall : EXEC GetPolicyID 'aa@xceedance.com'      
      
*/      
-- ================================================  

Alter PROCEDURE [dbo].[GetPolicyID] 
(        
  @Email VARCHAR(150)    
)        
AS         
BEGIN        
        
 -- SET NOCOUNT ON added to prevent extra result sets from        
 SET NOCOUNT ON;   
 SELECT distinct policy.PolicyNumber PolicyID  
	FROM OrganisationUserDetail Org
	inner join Quote quote on Org.id=quote.OrganizationUserDetailID    
	inner join Policy policy on quote.id=policy.QuoteID
	WHERE policy.IsActive=1 and Org.EmailID = @Email         
END 

