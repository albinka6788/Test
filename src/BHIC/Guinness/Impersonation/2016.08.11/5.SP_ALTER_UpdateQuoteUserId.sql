-- ================================================
/*
@Author      : Perm Pratap Singh Rajpurohit
@Description : This proc will update reference OrganizationUserId for supplied QuoteId
@CreatedOn   : 2015-Nov-04 

@Updation History : 
   @Update-0 : {date}, {by}, {description} 
   @Update-1 : {2015-Nov-16}, {Prem}, {Made changes to allow update data based on either UID or EmailId} 
   @Update-2 : {2016-Aug-10}, {Prem}, { Inserted new quote-id in "UserQuotes" table also to acheive one to many relationship for impersonation feature.

@ExampleCall : 
EXEC UpdateQuoteOrganizationUserId 16444,14
EXEC UpdateQuoteOrganizationUserId 19506,108,'prem@gmail.com','2015-11-25',1

*/
-- ================================================

ALTER PROCEDURE [dbo].[UpdateQuoteOrganizationUserId]
(
	 @QuoteNumber				VARCHAR(20)
	,@OrgnizationUserId			INT				= NULL
	,@EmailId					VARCHAR(150)	= NULL
	,@ModifiedDate				DATETIME		= GETDATE
	,@ModifiedBy				INT				= 1
)
AS 
BEGIN

	-- SET NOCOUNT ON added to prevent extra result sets from
	SET NOCOUNT OFF;

	--Comment : Here Update based on following scenario's
	--1. In case UID available then update it directly
	--2. Otherwise in case of EmailId available then get associated Id from "OrganisationUserDetail" and then let's update it in dependent table "Quote"
	BEGIN
		
		BEGIN--Comment : Here get QuoteId based on QuoteNumber and UserId based on EmailId

		DECLARE @QuoteId BIGINT = 0;
		DECLARE @OrganizationUserDetailID BIGINT = 0;

		SELECT @QuoteId = Id FROM Quote WHERE QuoteNumber=@QuoteNumber;
		SET @OrganizationUserDetailID = ISNULL(@OrgnizationUserId, (SELECT TOP 1 Id FROM OrganisationUserDetail WHERE EmailID=@EmailId));

		END

		UPDATE Quote SET 
		 ModifiedDate=@ModifiedDate
		,ModifiedBy=@ModifiedBy
		,OrganizationUserDetailID=ISNULL(@OrgnizationUserId, (SELECT TOP 1 Id FROM OrganisationUserDetail WHERE EmailID=@EmailId))
		WHERE QuoteNumber=@QuoteNumber
		   
		--Comment : Here Inserted new quote-id in UserQuotes table also to acheive one to many relationship for impersonation feature 
		--Paramteres orders @QuoteId,@OrganizationUserDetailID,@CreatedDate,@CreatedBy,@ModifiedDate,@ModifiedBy,@UpdateOnly
		EXEC MaintainUserQuotesLinking @QuoteId,@OrganizationUserDetailID,NULL,NULL,NULL,@OrganizationUserDetailID,1

	END

END

