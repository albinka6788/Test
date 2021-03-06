/****** Object:  StoredProcedure [dbo].[GetPolicyID]    Script Date: 10/30/2015 3:06:37 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

-- ================================================      
/*      
@Author      : Sreeram       
@Description : This procedure will policy id based on email     
@CreatedOn   : 2015-Oct-29       
      
@Updation History :       
   @Update-1 : 2015-Oct-29, Sreeram, created
   @update-2 : 2015-Oct-30, Gurpreet, retrive policy number from user table if not found then from policy table
      
@ExampleCall : EXEC GetPolicyID 'aa@xceedance.com'      
      
*/      
-- ================================================  

ALTER PROCEDURE [dbo].[GetPolicyID] 
(        
  @Email VARCHAR(25)    
)        
AS         
BEGIN        
        
 -- SET NOCOUNT ON added to prevent extra result sets from        
 SET NOCOUNT ON;    
  IF NOT EXISTS(SELECT PolicyCode FROM OrganisationUserDetail WHERE EmailID=@Email)
 BEGIN 
	SELECT distinct policy.PolicyNumber PolicyID  
	FROM OrganisationUserDetail Org
	LEFT JOIN Quote quote ON Org.id=quote.OrganizationUserDetailID    
	LEFT JOIN Policy policy ON quote.id=policy.QuoteID
	WHERE Org.EmailID = @Email  
 END
 ELSE
 BEGIN
	SELECT PolicyCode PolicyID FROM OrganisationUserDetail WHERE EmailID=@Email
 END
         
END 




