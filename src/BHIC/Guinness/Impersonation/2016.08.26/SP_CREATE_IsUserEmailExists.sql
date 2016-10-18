SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO


-- ================================================      
/*      
@Author      : Amit Kumar   
@Description : This procedure will return OrganisationUserDetail data for supplied Email
@CreatedOn   : 2016-Aug-26      
      
@Updation History :       
  
@ExampleCall : EXEC IsUserEmailExists 'Test@234.com'      
      
*/      
-- ================================================      
  
CREATE PROCEDURE [dbo].[IsUserEmailExists]      
(      
  @Email VARCHAR(150)   
)      
AS       
BEGIN      
 -- SET NOCOUNT ON added to prevent extra result sets from      
 SET NOCOUNT ON;    
 SELECT Id,EmailID,FirstName,LastName,password,PhoneNumber,IsEmailVerified,OrganizationName
 from OrganisationUserDetail where EmailID=@Email  
END 

GO