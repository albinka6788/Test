-- ================================================
/*
@Author      : Anuj Kumar Singh
@Description : This proc will insert policy details into Policy table
@CreatedOn   : 2015-oct-14 

@Updation History : 
   @Update-1 : {date}, {by}, {description} 

*/
-- ================================================

CREATE PROCEDURE [dbo].[CreatePolicy] 
(
  	  @QuoteID			INT
	 ,@PolicyNumber		VARCHAR(20)
	 ,@EffectiveDate	DATETIME
	 ,@ExpiryDate		DATETIME
	 ,@PremiumAmount	NUMERIC(18,2)
	 ,@PaymentOptionID	INT
	 ,@IsActive			BIT
	 ,@CreatedDate		DATETIME
	 ,@CreatedBy		INT
	 ,@ModifiedDate		DATETIME
	 ,@ModifiedBy		INT
)
AS 
BEGIN

	-- SET NOCOUNT ON added to prevent extra result sets from
	SET NOCOUNT OFF;

	--Comment : Insert record into table
			
		INSERT INTO Policy(Id,QuoteID,PolicyNumber,EffectiveDate,ExpiryDate,PremiumAmount,PaymentOptionID,IsActive,CreatedDate,CreatedBy,ModifiedDate,ModifiedBy) 
		VALUES((NEXT VALUE FOR SEQUENCEPolicy),@QuoteID,@PolicyNumber,@EffectiveDate,@ExpiryDate,@PremiumAmount,@PaymentOptionID,@IsActive,@CreatedDate,@CreatedBy,@ModifiedDate,@ModifiedBy);
END



