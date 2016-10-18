USE [Guinness_DB]
GO


/****** Object:  StoredProcedure [dbo].[ForgotPwdRequstedDateTime]    Script Date: 07-01-2016 14:22:52 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO



/*  ================================================      
     
@Author      : Krishna   
@Description : This procedure will return no  
@CreatedOn   : 2016-Jan-6       
@ExampleCall : EXEC ForgotPwdRequstedDateTime 'F' 'krishnappa.halemani@xceedance.com','05312015160010'
   
 ================================================   */    
CREATE PROCEDURE [dbo].[ForgotPwdRequstedDateTime]
(      
  @Type VARCHAR(1),
  @email VARCHAR(150), 
  @forgotPwdRequestedDateTime VARCHAR(500) 
)      
AS       
BEGIN      
 IF(@Type='F')
	BEGIN
	   UPDATE OrganisationUserDetail  SET ForgotPwdRequestedDateTime =@forgotPwdRequestedDateTime  WHERE EmailID=@email
	   SELECT * FROM OrganisationUserDetail WHERE ForgotPwdRequestedDateTime = @forgotPwdRequestedDateTime AND EmailID=@email
	END
 ELSE
		SELECT * FROM OrganisationUserDetail WHERE ForgotPwdRequestedDateTime = @forgotPwdRequestedDateTime AND EmailID=@email

END 




GO


