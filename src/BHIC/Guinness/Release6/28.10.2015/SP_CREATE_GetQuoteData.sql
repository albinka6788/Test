
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Prem Pratap Singh>
-- Create date: <28-Oct-2015>
-- Description:	<selecting QuoteDetails  based on QuoteNumber>
--exec GetQuoteDetails '15544'
-- =============================================
CREATE PROCEDURE GetQuoteDetails
(
	@QuoteNumber Varchar(20)
)
AS
BEGIN
SELECT [Id]
      ,[OrganizationUserDetailID]
      ,[OrganizationAddressID]
      ,[QuoteNumber]
      ,[LineOfBusinessId]
      ,[ExternalSystemID]
      ,[RequestDate]
      ,[ExpiryDate]
      ,[PremiumAmount]
      ,[IsActive]
      ,[CreatedDate]
      ,[CreatedBy]
      ,[ModifiedDate]
      ,[ModifiedBy]
      ,[PaymentoptionId]
      ,[AgencyCode] from [dbo].[Quote] where QuoteNumber = @QuoteNumber
END
GO
