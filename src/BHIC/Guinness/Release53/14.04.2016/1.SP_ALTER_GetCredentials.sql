/****** Object:  StoredProcedure [dbo].[GetCredentials]    Script Date: 4/14/2016 1:21:17 AM ******/
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
   @update-3 : 2015-nov-02, Gurpreet, retrive password to encypt and match    
   @update-4 : 2015-nov-27, Sreeram, retrive PhoneNumber 
   @update-5 : 2015-dec-5, Sreeram, retrive IsEmailVerified   
   @update-6 : 2015-dec-12, Sreeram, retrive OrganizationName   
   @update-7 : 2016-jan-8, Gurpreet, isActive=1 added
   @update-8 : 2016-jan-8, Sreeram, retrive AccountLockedDateTime,LoginAttempt
   @update-9 : 2016-Apr-14, Guru, return phone number as 0 when have null value in DB
@ExampleCall : EXEC GetCredentials 'Test@234.com'      
      
*/      
-- ================================================      
  
ALTER PROCEDURE [dbo].[GetCredentials]      
(      
  @Email VARCHAR(150)   
)      
AS       
BEGIN      
      
 -- SET NOCOUNT ON added to prevent extra result sets from      
SET NOCOUNT ON;    
SELECT 	Id,EmailID,FirstName,LastName,password,ISNULL(PhoneNumber, 0) PhoneNumber,IsEmailVerified,OrganizationName,isnull(AccountLockedDateTime,getdate()) 'AccountLockedDateTime',
		isnull(LoginAttempt,0) 'LoginAttempt'
		from OrganisationUserDetail where EmailID=@Email and IsActive=1           
END 

