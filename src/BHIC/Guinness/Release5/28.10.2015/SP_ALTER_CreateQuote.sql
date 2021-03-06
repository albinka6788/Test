
-- ================================================
/*
@Author      : Anuj Kumar Singh
@Description : This proc will insert quote details into Quote table
@CreatedOn   : 2015-oct-27 

@Updation History : 
   @Update-1 : {2015-Oct-28}, {Prem}, { #1. Updated to set default value to reduce additional data passing, #2. Altred to update details in case already exists } 

*/
-- ================================================

ALTER PROCEDURE [dbo].[CreateQuote] 
(
	       @QuoteNumber               VARCHAR(20)
	      ,@LineOfBusinessId          INT				= 1
	      ,@ExternalSystemID          INT				= 1
	      ,@RequestDate  	          DATETIME			= GETDATE
	      ,@ExpiryDate                DATETIME			= NULL
	      ,@PremiumAmount             NUMERIC(18,2)
	      ,@IsActive                  BIT				= 1
	      ,@CreatedDate               DATETIME			= GETDATE
	      ,@CreatedBy                 INT				= 1
	      ,@ModifiedDate              DATETIME			= GETDATE
		  ,@ModifiedBy                INT				= 1
)
AS 
BEGIN

	-- SET NOCOUNT ON added to prevent extra result sets from
	SET NOCOUNT OFF;

	--Comment : Here If record exists then update otherwise insert new one
	IF NOT EXISTS(SELECT Id,QuoteNumber FROM Quote WHERE QuoteNumber = @QuoteNumber)
	BEGIN
		--Comment : Insert record into table
		INSERT INTO [dbo].[Quote]
			  ([Id]
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
			  ,[ModifiedBy])
     
			  VALUES(
			NEXT VALUE FOR [SEQUENCEQuote]
			  ,@QuoteNumber
			  ,@LineOfBusinessId
			  ,@ExternalSystemID
			  ,@RequestDate
			  ,@ExpiryDate
			  ,@PremiumAmount
			  ,@IsActive
			  ,@CreatedDate
			  ,@CreatedBy
			  ,@ModifiedDate
			  ,@ModifiedBy
		   )
	END
	ELSE
	BEGIN
		UPDATE Quote SET 			   
			   [LineOfBusinessId]   = @LineOfBusinessId
			  ,[ExternalSystemID]   = @ExternalSystemID
			  ,[RequestDate]		= @RequestDate
			  ,[ExpiryDate]			= @ExpiryDate
			  ,[PremiumAmount]		= @PremiumAmount
			  ,[IsActive]			= @IsActive
			  ,[CreatedDate]		= @CreatedDate
			  ,[CreatedBy]			= @CreatedBy
			  ,[ModifiedDate]		= @ModifiedDate
			  ,[ModifiedBy]			= @ModifiedBy
		WHERE QuoteNumber = @QuoteNumber;
	END


END





