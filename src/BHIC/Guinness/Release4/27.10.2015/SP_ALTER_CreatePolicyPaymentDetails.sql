
/****** Object:  StoredProcedure [dbo].[CreatePolicyPaymentDetail]    Script Date: 10/27/2015 8:25:13 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- ================================================
/*
@Author      : Anuj Kumar Singh
@Description : This proc will insert policy payment details into PolicyPaymentDetail table
@CreatedOn   : 2015-oct-14 

@Updation History : 
   @Update-1 : {date}, {by}, {description} 

*/
-- ================================================

ALTER PROCEDURE [dbo].[CreatePolicyPaymentDetail] 
(
	  @PolicyID				INT
	 ,@TransactionCode		VARCHAR(200)
	 ,@AmountPaid			NUMERIC(18,2)
	 ,@IsActive				BIT
	 ,@CreatedDate			DATETIME
	 ,@CreatedBy			INT
	 ,@ModifiedDate			DATETIME
	 ,@ModifiedBy			INT
)
AS 
BEGIN

	-- SET NOCOUNT ON added to prevent extra result sets from
	SET NOCOUNT OFF;

	--Comment : Insert record into table
	BEGIN
		INSERT INTO PolicyPaymentDetail(Id,PolicyID,TransactionCode,AmountPaid,IsActive,CreatedDate,CreatedBy,ModifiedDate,ModifiedBy) 
		VALUES((NEXT VALUE FOR SEQUENCEPolicyPaymentDetail),@PolicyID,@TransactionCode,@AmountPaid,@IsActive,@CreatedDate,@CreatedBy,@ModifiedDate,@ModifiedBy);
	END

END



