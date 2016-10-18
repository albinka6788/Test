USE [Guinness_DB]
GO
/****** Object:  StoredProcedure [dbo].[GetAPICredentials]    Script Date: 24-12-2015 21:50:21 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO


  
  
-- ================================================      
/*      
@Author      : Venkatesh       
@Description : This procedure will return Email/Password for supplied Email & password      
@CreatedOn   : 2015-Oct-15       
      
@Updation History :       
      
@ExampleCall : EXEC GetAPICredentials 'Test@234.com','123'      
      
*/      
-- ================================================      
  
CREATE PROCEDURE [dbo].[GetAPICredentials]      
(      
  @Email VARCHAR(150)   
)      
AS       
BEGIN      
      
 -- SET NOCOUNT ON added to prevent extra result sets from      
 SET NOCOUNT ON;      
    
     
 SELECT Id,EmailID,password from APIUserDetails where EmailID=@Email and IsActive = 1

 
       
END 



