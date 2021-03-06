/*  ================================================      
     
@Author      : Sreeram   
@Description : This procedure will return no  
@CreatedOn   : 2015-Nov-6
@Updation History :       
   @Update-1 : 2015-Nov-6, Sreeram, created
   @update-2 : 2016-Jan-27, Albin, Added ModifiedDate, ModifiedBy in the proc
          
@ExampleCall : EXEC ChangePassword 'k@123.com','123456'
   
 ================================================   */    
ALTER PROCEDURE [dbo].[ChangePassword]
(      
  @email VARCHAR(150), 
  @newpassword VARCHAR(500) 
)      
AS       
BEGIN      
	UPDATE OrganisationUserDetail
	SET 
	Password =@newpassword, ForgotPwdRequestedDateTime = null,
	ModifiedDate = GetDate(), ModifiedBy = Id	 
	WHERE EmailID=@email
END 




