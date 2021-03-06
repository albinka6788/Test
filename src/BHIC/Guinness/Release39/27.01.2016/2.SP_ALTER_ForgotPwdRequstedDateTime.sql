/*  ================================================      
     
@Author      : Krishna   
@Description : This procedure will return no  
@CreatedOn   : 2016-Jan-6
@Updation History :       
   @Update-1 : 2016-Jan-6, Krishna, created
   @update-2 : 2016-Jan-27, Albin, Added ModifiedDate, ModifiedBy in the proc       
@ExampleCall : EXEC ForgotPwdRequstedDateTime 'F' 'krishnappa.halemani@xceedance.com','05312015160010'
   
 ================================================   */    
ALTER PROCEDURE [dbo].[ForgotPwdRequstedDateTime]
(      
  @Type VARCHAR(1),
  @email VARCHAR(150), 
  @forgotPwdRequestedDateTime VARCHAR(500) 
)      
AS       
BEGIN      
 IF(@Type='F')
	BEGIN
		UPDATE OrganisationUserDetail  
		SET 
			ForgotPwdRequestedDateTime =@forgotPwdRequestedDateTime, 
			ModifiedDate = GetDate(), ModifiedBy = Id  WHERE EmailID=@email

		SELECT * FROM OrganisationUserDetail WHERE ForgotPwdRequestedDateTime = @forgotPwdRequestedDateTime AND EmailID=@email
	END
	ELSE
		BEGIN
			SELECT * FROM OrganisationUserDetail WHERE ForgotPwdRequestedDateTime = @forgotPwdRequestedDateTime AND EmailID=@email
		END
END 




