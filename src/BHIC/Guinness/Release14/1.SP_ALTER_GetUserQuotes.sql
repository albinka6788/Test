/*=============================================
 Author:		<Gurpreet Singh>
 Create date: <13-Nov-2015>
 Description:	<Get Saved Quotes of user>
exec GetUserQuotes '2'
 =============================================*/
ALTER PROCEDURE [dbo].[GetUserQuotes]
(
@UserID int
)
AS
BEGIN
	select Qt.[Id],Qt.[OrganizationUserDetailID],Qt.[OrganizationAddressID],Qt.[QuoteNumber],Qt.[LineOfBusinessId],Qt.[ExternalSystemID],Qt.[RequestDate],Qt.[ExpiryDate]
	,Qt.[PremiumAmount],Qt.[IsActive],Qt.[CreatedDate],Qt.[CreatedBy],Qt.[ModifiedDate],Qt.[ModifiedBy],Qt.[PaymentoptionId],Qt.[AgencyCode] ,Lob.LineOfBusinessName
	from [dbo].[Quote] Qt 
	inner join [dbo].[LineOfBusiness] Lob on Qt.LineOfBusinessId=Lob.Id
	where Qt.OrganizationUserDetailID =  @UserID AND Qt.Id NOT IN (SELECT DISTINCT QuoteID FROM Policy)

END
