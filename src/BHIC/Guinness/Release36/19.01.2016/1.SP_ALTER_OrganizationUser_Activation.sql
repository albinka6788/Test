-- ================================================
/*
@Author      : Prem Pratap Singh Rajpurohit
@Description : This proc will update user as an Active/Deactivate for supplied EmailId
@CreatedOn   : 2015-Nov-26 

@Updation History : 
   @Update-0 : {date}, {by}, {description} 
   @Update-1 : {2016-JAN-19}, {Prem}, {Chnage logic to update IsActive status only when } 

@ExampleCall : 
EXEC ActivateDeactivateOrganizationUser 

*/
-- ================================================

ALTER PROCEDURE [dbo].[ActivateDeactivateOrganizationUser]
(
	 @IsActive					BIT				= 0
	,@QuoteNumber				VARCHAR(20)
	,@ModifiedDate				DATETIME		= GETDATE
	,@ModifiedBy				INT				= 1
)
AS 
BEGIN

	-- SET NOCOUNT ON added to prevent extra result sets from
	SET NOCOUNT OFF;

	--Comment : Here Update based on following scenario's
	--1. UID or user-account created on purchase page will emains In-Active 
	--2. Finally when user will generate Payment & Policy then this user-account will be activated to login in PolicyCenter dashboard
	BEGIN		
		declare @OrgnizationUserId int

		SELECT @OrgnizationUserId=OrganizationUserDetailID FROM Quote where QuoteNumber=@QuoteNumber

		UPDATE OrganisationUserDetail SET ModifiedDate=@ModifiedDate,ModifiedBy=@ModifiedBy,IsActive=(CASE WHEN IsActive = 1 THEN IsActive ELSE @IsActive END) WHERE Id=@OrgnizationUserId
	END

END
