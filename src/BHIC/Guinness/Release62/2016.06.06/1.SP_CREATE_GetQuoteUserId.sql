/*=============================================
Author:		<Amit Kumar>
Create date: <03-June-2016>
Description:	<selecting QuoteUserId  based on QuoteNumber>
exec GetQuoteUserId '55'
=============================================*/
ALTER PROCEDURE [GetQuoteUserId]
(
	@QuoteNumber VARCHAR(20)
)
AS
BEGIN
	SELECT [OrganizationUserDetailID] FROM [Quote] WHERE QuoteNumber =  @QuoteNumber
END
