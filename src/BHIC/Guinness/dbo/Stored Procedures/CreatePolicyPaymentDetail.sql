-- ================================================
/*
@Author      : Anuj Kumar Singh
@Description : This proc will insert policy payment details into PolicyPaymentDetail table
@CreatedOn   : 2015-oct-14 

@Updation History : 
   @Update-1 : {date}, {by}, {description} 

*/
-- ================================================

CREATE PROCEDURE [dbo].[CreatePolicyPaymentDetail] 
(
	  @PolicyID				INT
	 ,@CreditCardNumber		VARCHAR(256)
	 ,@CardExpiryDate		DATETIME
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
		INSERT INTO PolicyPaymentDetail(Id,PolicyID,CreditCardNumber,CardExpiryDate,TransactionCode,AmountPaid,IsActive,CreatedDate,CreatedBy,ModifiedDate,ModifiedBy) 
		VALUES((NEXT VALUE FOR SEQUENCEPolicyPaymentDetail),@PolicyID,@CreditCardNumber,@CardExpiryDate,@TransactionCode,@AmountPaid,@IsActive,@CreatedDate,@CreatedBy,@ModifiedDate,@ModifiedBy);
	END

END



