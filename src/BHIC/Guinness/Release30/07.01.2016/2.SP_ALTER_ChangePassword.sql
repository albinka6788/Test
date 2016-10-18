USE [Guinness_DB]
GO

/****** Object:  StoredProcedure [dbo].[ChangePassword]    Script Date: 07-01-2016 14:21:56 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO



/*  ================================================      
     
@Author      : Sreeram   
@Description : This procedure will return no  
@CreatedOn   : 2015-Nov-6       
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
 SET Password =@newpassword,
 ForgotPwdRequestedDateTime = null
 WHERE EmailID=@email
END 




GO


