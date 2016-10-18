-- ================================================
/*
@Author      : Perm Pratap Singh Rajpurohit
@Description : This proc will add/update application[Guinness] custom session for supplied QuoteId which will be used to get stored data 
@CreatedOn   : 2015-Oct-23 

@Updation History : 
   @Update-0 : {date}, {by}, {description} 

@ExampleCall : 
EXEC MaintainApplicationCustomSession '14214', 'stringyfy data'

--Select * From PurchaePathCustomSession

*/
-- ================================================

ALTER PROCEDURE [dbo].[MaintainApplicationCustomSession]
(
	 @QuoteId			BIGINT
	,@SessionData		VARCHAR(MAX)
	,@IsActive			BIT = 1
)
AS 
BEGIN

	-- SET NOCOUNT ON added to prevent extra result sets from
	SET NOCOUNT OFF;

	BEGIN --Comment : Here local varibales declration and initialization

		DECLARE @CreatedBy INT = 1;

	END

	--Comment : Here If record exists then update otherwise insert new one
	IF NOT EXISTS(SELECT QuoteId,SessionData FROM PurchaePathCustomSession WHERE QuoteId=@QuoteId)
	BEGIN		
		INSERT INTO PurchaePathCustomSession(Id,QuoteId,SessionData,CreatedBy,IsActive) VALUES((NEXT VALUE FOR SEQUENCEPurchaePathCustomSession),@QuoteId,CAST(@SessionData AS VARCHAR(MAX)),@CreatedBy,@IsActive);
	END
	ELSE
	BEGIN
		UPDATE PurchaePathCustomSession SET SessionData=CAST(@SessionData AS VARCHAR(MAX)),ModifiedDate=GETDATE() WHERE QuoteId=@QuoteId;
	END

END





