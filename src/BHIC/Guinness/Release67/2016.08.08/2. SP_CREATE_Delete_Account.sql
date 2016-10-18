SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

/* =============================================
   Author:		Amit Kumar
   Create date: 2016-Aug-04
   Description:	Delete an Account


	@Updation History : 
	   @Update-1 : {date}, {by}, {description} 


   ================================================*/
CREATE PROCEDURE [dbo].[DeleteAccount]
(
@EmailID varchar(250)
)
AS
BEGIN
BEGIN TRANSACTION T1
	Declare @UserID int	;
	-- Get UserID of the user to be deleted
	DECLARE @UserIdDeleted BIGINT=0;
	select @UserID = Id from OrganisationUserDetail where EmailID = @EmailID;
	BEGIN TRY	
	    SET NOCOUNT OFF;		
			if(@UserID>0)
				Begin					
					Declare @NumQuotes int = (select count(Id) from Quote where OrganizationUserDetailID = @UserID);					
					if(@NumQuotes > 0)
						Begin
							RAISERROR(N'%s can not be deleted. %d Quotes are linked with it.',16,1,@EmailID,@NumQuotes);
						End
					Else
						Begin
						    select @UserIdDeleted = @UserID;
							Delete from OraganisationAddress where OrganizationID = @UserID;
							Delete from OrganisationUserDetail where Id = @UserID	;															
						End		
				End			
	END TRY
	BEGIN CATCH
		ROLLBACK TRANSACTION T1;		
		THROW; 
	END CATCH	
	COMMIT TRANSACTION T1  	
	RETURN @UserIdDeleted
END  

GO


