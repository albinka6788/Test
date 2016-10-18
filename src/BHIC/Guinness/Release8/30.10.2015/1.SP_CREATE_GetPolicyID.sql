
-- ================================================        
/*        
@Author      : Sreeram B        
@Description : This proc will return Policy ID        
@CreatedOn   : 2015-Oct-26         
        
@Updation History :         
   @Update-1 : {date}, {by}, {description}         
        
@ExampleCall : EXEC GetPolicyID 'a@xceedance.com'      
        
*/        
-- ================================================        
        
CREATE PROCEDURE [dbo].[GetPolicyID]        
(        
  @Email VARCHAR(25)    
)        
AS         
BEGIN        
        
 -- SET NOCOUNT ON added to prevent extra result sets from        
 SET NOCOUNT ON;         
       
    
SELECT    
    policy.PolicyNumber  as PolicyID  
FROM    
    OrganisationUserDetail as Org,    
    Quote as quote,    
    Policy as policy   
WHERE    
    Org.EmailID = @Email    
    AND Org.id=quote.OrganizationUserDetailID    
    AND quote.id=policy.QuoteID;     
         
END 

GO


