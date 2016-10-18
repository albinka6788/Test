-- ================================================
/*
@Author      : Perm Pratap Singh Rajpurohit
@Description : This proc will update reference OrganizationUserId for supplied QuoteId
@CreatedOn   : 2015-Nov-17 

@Updation History : 
   @Update-0 : {date}, {by}, {description} 

@ExampleCall : 
EXEC UpdateQuoteOrganizationAddressID 16444,21
EXEC UpdateQuoteOrganizationAddressID 16444,'prem@gmail.com'

*/
-- ================================================

CREATE PROCEDURE [dbo].[UpdateQuoteOrganizationAddressID]
(
	 @QuoteNumber				VARCHAR(20)
	,@OrganizationAddressID		INT				= NULL
	,@EmailId					VARCHAR(150)	= NULL
	,@ModifiedDate				DATETIME		= NULL
	,@ModifiedBy				INT				= NULL
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
		 ModifiedDate=ISNULL(@ModifiedDate,GETDATE())
		,ModifiedBy=ISNULL(@ModifiedBy,1)
		,OrganizationAddressID=ISNULL(@OrganizationAddressID, (SELECT T2.Id FROM OrganisationUserDetail T1,OraganisationAddress T2 WHERE T1.Id=T2.OrganizationID AND T1.EmailID=@EmailId))
		WHERE QuoteNumber=@QuoteNumber
	END

END
