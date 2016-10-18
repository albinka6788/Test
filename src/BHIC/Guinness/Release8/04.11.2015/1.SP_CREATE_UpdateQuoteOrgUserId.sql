
-- ================================================
/*
@Author      : Perm Pratap Singh Rajpurohit
@Description : This proc will update reference OrganizationUserId for supplied QuoteId
@CreatedOn   : 2015-Nov-04 

@Updation History : 
   @Update-0 : {date}, {by}, {description} 

@ExampleCall : 
EXEC UpdateQuoteOrganizationUserId 16444,14

*/
-- ================================================

CREATE PROCEDURE [dbo].[UpdateQuoteOrganizationUserId]
(
	 @QuoteId				INT
	,@OrgnizationUserId		INT
)
AS 
BEGIN

	-- SET NOCOUNT ON added to prevent extra result sets from
	SET NOCOUNT OFF;

	--Comment : Here If record exists then update otherwise insert new one
	BEGIN
		UPDATE Quote SET 
		OrganizationUserDetailID=@OrgnizationUserId 
		--,OrganizationAddressID=@OrganizationAddressID 
		WHERE QuoteNumber=@QuoteId
	END

END


