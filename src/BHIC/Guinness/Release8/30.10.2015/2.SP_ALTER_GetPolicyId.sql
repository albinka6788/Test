/****** Object:  StoredProcedure [dbo].[GetPolicyID]    Script Date: 10/29/2015 8:08:04 PM ******/
DROP PROCEDURE [dbo].[GetPolicyID]
GO

/****** Object:  StoredProcedure [dbo].[GetCredentials]    Script Date: 10/29/2015 8:08:04 PM ******/
DROP PROCEDURE [dbo].[GetCredentials]
GO

/****** Object:  StoredProcedure [dbo].[GetCredentials]    Script Date: 10/29/2015 8:08:04 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

-- ================================================      
/*      
@Author      : Sreeram       
@Description : This procedure will return Email/Passowrd for supplied Email & password      
@CreatedOn   : 2015-Oct-15       
      
@Updation History :       
   @Update-1 : 2015-Oct-16, Sreeram, created
   @update-2 : 2015-Oct-29, Gurpreet, retrive user detail      
      
@ExampleCall : EXEC GetCredentials 'Test@234.com','123'      
      
*/      
-- ================================================      
  
CREATE PROCEDURE [dbo].[GetCredentials]      
(      
  @Email VARCHAR(150),    
  @Password VARCHAR(256)    
)      
AS       
BEGIN      
      
 -- SET NOCOUNT ON added to prevent extra result sets from      
 SET NOCOUNT ON;      
    
     
 select Id,EmailID,FirstName,LastName  from OrganisationUserDetail where EmailID=@Email and Password=@Password;    
 
       
END 



GO

/****** Object:  StoredProcedure [dbo].[GetPolicyID]    Script Date: 10/29/2015 8:08:04 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[GetPolicyID]        
(        
  @Email VARCHAR(25)    
)        
AS         
BEGIN        
        
 -- SET NOCOUNT ON added to prevent extra result sets from        
 SET NOCOUNT ON;         
       
    
SELECT    
    distinct policy.PolicyNumber  as PolicyID  
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


