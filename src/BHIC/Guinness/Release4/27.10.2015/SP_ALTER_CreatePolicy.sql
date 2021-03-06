
/****** Object:  StoredProcedure [dbo].[CreatePolicy]    Script Date: 10/27/2015 6:32:18 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- ================================================
/*
@Author      : Anuj Kumar Singh
@Description : This proc will insert policy details into Policy table
@CreatedOn   : 2015-oct-14 

@Updation History : 
   @Update-1 : {date}, {by}, {description} 

*/
-- ================================================

ALTER PROCEDURE [dbo].[CreatePolicy] 
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
	 ,@policy_identity  INT    OUTPUT
)
AS 
BEGIN

	-- SET NOCOUNT ON added to prevent extra result sets from
	SET NOCOUNT OFF;

	--Comment : Insert record into table

		SELECT @policy_identity = (NEXT VALUE FOR SEQUENCEPolicy)

			
		INSERT INTO Policy(Id,QuoteID,PolicyNumber,EffectiveDate,ExpiryDate,PremiumAmount,PaymentOptionID,IsActive,CreatedDate,CreatedBy,ModifiedDate,ModifiedBy) 
		VALUES(@policy_identity,@QuoteID,@PolicyNumber,@EffectiveDate,@ExpiryDate,@PremiumAmount,@PaymentOptionID,@IsActive,@CreatedDate,@CreatedBy,@ModifiedDate,@ModifiedBy);

END



