/****** Object:  StoredProcedure [dbo].[CreateQuote]    Script Date: 22-02-2016 13:04:23 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- ================================================
/*
@Author      : Anuj Kumar Singh
@Description : This proc will insert quote details into Quote table
@CreatedOn   : 2015-oct-27 

@Updation History : 
   @Update-1 : {2015-Oct-28}, {Prem}, { #1. Updated to set default value to reduce additional data passing, #2. Altred to update details in case already exists } 
   @Update-2 : {2015-Oct-28}, {Guru}, { #2. Added New Parameters PaymentOptionId and AgencyCode}
   @Update-3 : {2015-Nov-28}, {Prem}, { #3. Added 2 New Parameters to make feasible this proc to insert data in case of PolicyCenter work-flow}
   @Update-4 : {2016-Feb-22}, {Venkatesh}, { #4. Added 1 New Parameters (RetrieveQuoteURL) to save BOP Quote Retrieve URL}
*/
-- ================================================

ALTER PROCEDURE [dbo].[CreateQuote] 
(
	       @QuoteNumber               VARCHAR(20)
		  ,@OrganizationUserDetailID  INT				= NULL
		  ,@OrganizationAddressID     INT				= NULL
	      ,@LineOfBusinessId          INT				= 1
	      ,@ExternalSystemID          INT				= 1
	      ,@RequestDate  	          DATETIME			= GETDATE
	      ,@ExpiryDate                DATETIME			= NULL
	      ,@PremiumAmount             NUMERIC(18,2)
		  ,@PaymentoptionId			  INT				= NULL
		  ,@AgencyCode				  VARCHAR(20)		= NULL
	      ,@IsActive                  BIT				= 1
	      ,@CreatedDate               DATETIME			= GETDATE
	      ,@CreatedBy                 INT				= 1
	      ,@ModifiedDate              DATETIME			= GETDATE
		  ,@ModifiedBy                INT				= 1
		  ,@RetrieveQuoteURL          VARCHAR(500)		= NULL
)
AS 
BEGIN

	-- SET NOCOUNT ON added to prevent extra result sets from
	SET NOCOUNT OFF;

	--Comment : Here If record exists then update otherwise insert new one
	IF NOT EXISTS(SELECT 1 FROM Quote WHERE QuoteNumber = @QuoteNumber)
	BEGIN
		--Comment : Insert record into table
		INSERT INTO [dbo].[Quote]
			  ([Id]
			  ,[QuoteNumber]
			  ,[OrganizationUserDetailID]
			  ,[OrganizationAddressID]
			  ,[LineOfBusinessId]
			  ,[ExternalSystemID]
			  ,[RequestDate]
			  ,[ExpiryDate]
			  ,[PremiumAmount]
			  ,[PaymentoptionId]
			  ,[AgencyCode]
			  ,[IsActive]
			  ,[CreatedDate]
			  ,[CreatedBy]
			  ,[ModifiedDate]
			  ,[ModifiedBy]
			  ,[RetrieveQuoteURL])
     
			  VALUES(
			NEXT VALUE FOR [SEQUENCEQuote]
			  ,@QuoteNumber
			  ,@OrganizationUserDetailID
			  ,@OrganizationAddressID   
			  ,@LineOfBusinessId
			  ,@ExternalSystemID
			  ,@RequestDate
			  ,@ExpiryDate
			  ,@PremiumAmount
			  ,@PaymentoptionId
			  ,@AgencyCode
			  ,@IsActive
			  ,@CreatedDate
			  ,@CreatedBy
			  ,@ModifiedDate
			  ,@ModifiedBy
			  ,@RetrieveQuoteURL
		   )
	END
	ELSE
	BEGIN
		UPDATE Quote SET 			   
			   @OrganizationUserDetailID = @OrganizationUserDetailID
			  ,@OrganizationAddressID	 = @OrganizationAddressID
			  ,[LineOfBusinessId]		 = @LineOfBusinessId
			  ,[ExternalSystemID]		 = @ExternalSystemID
			  ,[RequestDate]			 = @RequestDate
			  ,[ExpiryDate]				 = @ExpiryDate
			  ,[PremiumAmount]			 = @PremiumAmount
			  ,[PaymentoptionId]		 = @PaymentoptionId
			  ,[AgencyCode]				 = @AgencyCode
			  ,[IsActive]				 = @IsActive
			  ,[ModifiedDate]			 = @ModifiedDate
			  ,[ModifiedBy]				 = @ModifiedBy
			  ,[RetrieveQuoteURL]        = @RetrieveQuoteURL
		WHERE QuoteNumber = @QuoteNumber;
	END


END






