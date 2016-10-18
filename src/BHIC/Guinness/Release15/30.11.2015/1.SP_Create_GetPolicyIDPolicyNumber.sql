/****** Object:  StoredProcedure [dbo].[GetPolicyIDByPolicyNumber]    Script Date: 11/30/2015 12:32:15 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

-- ================================================      
/*      
@Author      : Anuj Kumar Singh       
@Description : This procedure will fetch policy id based on policy number     
@CreatedOn   : 2015-Nov-30       
      
@Updation History :       
   @Update-1 : 2015-Nov-30, Anuj Kumar Singh, created
      
@ExampleCall : EXEC GetPolicyIDByPolicyNumber 'N9WC637456'      
      
*/      
-- ================================================  

CREATE PROCEDURE [dbo].[GetPolicyIDByPolicyNumber]
(        
  @policyNumber VARCHAR(20)    
)        
AS         
BEGIN        
        
 -- SET NOCOUNT ON added to prevent extra result sets from        
 SET NOCOUNT ON;   
 SELECT distinct policy.Id PolicyID  
	FROM Policy policy
	WHERE policy.PolicyNumber= @policyNumber
END 



