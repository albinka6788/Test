
/*=============================================
 Author:		<Gurpreet Singh>
 Create date: <13-Nov-2015>
 Description:	<Get Saved Quotes of user>
       
@Updation History :       
   @Update-1 : {2015-Dec-10}, {Prem}, {Added ordering logic in existing query} 
   @Update-2 : {2015-Dec-14}, {Sree}, {Added condition for IsDeleted}   
   @Update-3 : {2016-Feb-22}, {Venkatesh}, { #1. Added new parameter RetrieveQuoteURL in select query  } 

exec GetUserQuotes '2'
 =============================================*/
ALTER PROCEDURE [dbo].[GetUserQuotes]
(
@UserID int
)
AS
BEGIN
	select Qt.[Id],Qt.[OrganizationUserDetailID],Qt.[OrganizationAddressID],Qt.[QuoteNumber],Qt.[LineOfBusinessId],Qt.[ExternalSystemID],Qt.[RequestDate],Qt.[ExpiryDate]
	,Qt.[PremiumAmount],Qt.[IsActive],Qt.[CreatedDate],Qt.[CreatedBy],Qt.[ModifiedDate],Qt.[ModifiedBy],Qt.[PaymentoptionId],Qt.[AgencyCode] ,Lob.LineOfBusinessName,Qt.[RetrieveQuoteURL]
	from [dbo].[Quote] Qt 
	inner join [dbo].[LineOfBusiness] Lob on Qt.LineOfBusinessId=Lob.Id
	where Qt.OrganizationUserDetailID = @UserID AND Qt.Id NOT IN (SELECT DISTINCT QuoteID FROM Policy)
	AND  IsDeleted!=1
	Order By Qt.CreatedDate DESC,CAST(Qt.[QuoteNumber] AS INT) DESC
END

