-- ================================================
/*
@Author      : Perm Pratap Singh Rajpurohit
@Description : This proc will update reference OrganizationUserId for supplied QuoteId
@CreatedOn   : 2015-Nov-04 

@Updation History : 
   @Update-0 : {date}, {by}, {description} 
   @Update-1 : {2015-Nov-16}, {Prem}, {Made changes to allow update data based on either UID or EmailId} 

@ExampleCall : 
EXEC UpdateQuoteOrganizationUserId 16444,14
EXEC UpdateQuoteOrganizationUserId 16444,'prem@gmail.com'

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
		UPDATE Quote SET 
		 ModifiedDate=@ModifiedDate
		,ModifiedBy=@ModifiedBy
		,OrganizationUserDetailID=ISNULL(@OrgnizationUserId, (SELECT Id FROM OrganisationUserDetail WHERE EmailID=@EmailId))
		WHERE QuoteNumber=@QuoteNumber
	END

END


