
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
SELECT 	Id,EmailID,FirstName,LastName,password,PhoneNumber,IsEmailVerified,OrganizationName,isnull(AccountLockedDateTime,getdate()) 'AccountLockedDateTime',
		isnull(LoginAttempt,0) 'LoginAttempt'
		from OrganisationUserDetail where EmailID=@Email and IsActive=1           
END 


