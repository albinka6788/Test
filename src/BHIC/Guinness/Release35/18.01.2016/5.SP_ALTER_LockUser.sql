/****** Object:  StoredProcedure [dbo].[LockUser]    Script Date: 1/18/2016 3:40:40 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
/*  ================================================      
     
@Author      : Sreeram B   
@Description : This procedure will update the number for unsuccessful attempts based on Email  
@CreatedOn   : 2016-Jan-08       
@Updation History :       
   @Update-1 : 2016-Jan-18, Gurpreet, code logic moved to procedure
   
@ExampleCall : EXEC LockUser 'gurpreet.singh@xceedance.com',0,3,1
   
 ================================================   */    
ALTER PROCEDURE [dbo].[LockUser]
(  
  @Email VARCHAR(150),    
  @IsValidPassword bit, 
  @MaxLoginAttempt int,
  @MaxTimeToUnlock int
)      
AS       
BEGIN      
	-- SET NOCOUNT ON added to prevent extra result sets from
	SET NOCOUNT OFF;

	DECLARE @LoginAttempt int
	DECLARE @AccountLockedDateTime datetime
	DECLARE @CurrentTime datetime=getDate()
	DECLARE @ResponseMessage VARCHAR(500) ='OK'
	
	SELECT @LoginAttempt=LoginAttempt,@AccountLockedDateTime=AccountLockedDateTime FROM OrganisationUserDetail WHERE EmailID=@Email

	DECLARE @LockTime int = DATEDIFF("HH",@AccountLockedDateTime,@CurrentTime)

	IF(@LockTime>=@MaxTimeToUnlock)
	BEGIN
		UPDATE OrganisationUserDetail SET AccountLockedDateTime = null, LoginAttempt = 0  WHERE EmailID=@Email
	END

	IF(@IsValidPassword=0)
	BEGIN
		IF(@LoginAttempt<@MaxLoginAttempt)
		BEGIN
			SET @LoginAttempt+=1
			UPDATE OrganisationUserDetail SET AccountLockedDateTime = @CurrentTime, LoginAttempt = @LoginAttempt WHERE EmailID=@Email
			SET @ResponseMessage='Please provide valid login credentials.'			
		END
		IF(@LoginAttempt=@MaxLoginAttempt)
		BEGIN
			SET @ResponseMessage='Your account has been locked. Please try after sometime.'
		END

	END	
	ELSE IF(@LockTime>=@MaxTimeToUnlock and @LoginAttempt=@MaxLoginAttempt)
	BEGIN
		UPDATE OrganisationUserDetail SET AccountLockedDateTime = @CurrentTime, LoginAttempt = 0 WHERE EmailID=@Email
	END
	ELSE IF(@LockTime<@MaxTimeToUnlock)
	BEGIN
		SET @ResponseMessage='Your account has been locked. Please try after sometime.'
	END
	
	SELECT @ResponseMessage

END


