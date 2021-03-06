/*  ================================================      
     
@Author      : Sreeram B   
@Description : This procedure will update the number for unsuccessful attempts based on Email  
@CreatedOn   : 2016-Jan-08       
@ExampleCall : EXEC LockUser 'L','sreeram.bheemala@xceedance.com','2015-10-23 11:03:50.940',1
   
 ================================================   */    
CREATE PROCEDURE [dbo].[LockUser]
(  
  @Type VARCHAR(1),    
  @Email VARCHAR(150), 
  @AccountLockedDateTime DATETIME,
  @LoginAttempt int
)      
AS       
BEGIN      

	-- SET NOCOUNT ON added to prevent extra result sets from
	SET NOCOUNT OFF;
	IF(@Type = 'L')
		BEGIN
			UPDATE	OrganisationUserDetail  
			SET		AccountLockedDateTime = @AccountLockedDateTime, LoginAttempt = LoginAttempt + @LoginAttempt 
			WHERE	EmailID=@Email

			SELECT * FROM OrganisationUserDetail WHERE EmailID=@Email
		END
	ELSE
		BEGIN
			UPDATE	OrganisationUserDetail  
			SET		AccountLockedDateTime = @AccountLockedDateTime, LoginAttempt = @LoginAttempt  
			WHERE	EmailID=@Email

			SELECT * FROM OrganisationUserDetail WHERE EmailID=@Email
		END
END

