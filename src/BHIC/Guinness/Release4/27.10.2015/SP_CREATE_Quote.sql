
/****** Object:  StoredProcedure [dbo].[CreateQuote]    Script Date: 10/27/2015 6:32:18 PM ******/
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
   @Update-1 : {date}, {by}, {description} 

*/
-- ================================================

CREATE PROCEDURE [dbo].[CreateQuote] 
(
	       @QuoteNumber               VARCHAR(20)
	      ,@LineOfBusinessId          INT
	      ,@ExternalSystemID          INT
	      ,@RequestDate  	          DATETIME
	      ,@ExpiryDate                DATETIME
	      ,@PremiumAmount             NUMERIC(18,2)
	      ,@IsActive                  BIT
	      ,@CreatedDate               DATETIME
	      ,@CreatedBy                 INT
	      ,@ModifiedDate              DATETIME    
		  ,@ModifiedBy                INT
)
AS 
BEGIN

	-- SET NOCOUNT ON added to prevent extra result sets from
	SET NOCOUNT OFF;

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



