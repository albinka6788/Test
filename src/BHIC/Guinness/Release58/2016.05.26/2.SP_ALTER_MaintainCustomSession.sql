
-- ================================================
/*
@Author      : Perm Pratap Singh Rajpurohit
@Description : This proc will add/update application[Guinness] custom session for supplied QuoteId which will be used to get stored data 
@CreatedOn   : 2015-Oct-23 

@Updation History : 
   @Update-0 : {date}, {by}, {description} 
   @Update-1 : {2016-MAY-05}, {Prem}, {As per GURU's suggestion updated CreatedBy with @ModifiedBy. This will reolves problem of SaveForLater on UserInfo page in PurchasePath} 
   @Update-2 : {2016-MAY-26}, {Prem}, {As per GURU's suggestion remove CreatedBy in Update statement. This will reolves problem of SaveForLater on UserInfo page in PurchasePath} 

@ExampleCall : 
EXEC MaintainApplicationCustomSession '14214', 'stringyfy data'

--Select * From PurchaePathCustomSession

*/
-- ================================================

ALTER PROCEDURE [dbo].[MaintainApplicationCustomSession]
(
	 @QuoteId			BIGINT
	,@SessionData		VARCHAR(MAX)
	,@IsActive			BIT				= 1
	,@CreatedDate       DATETIME		= GETDATE
	,@CreatedBy         INT				= 1
	,@ModifiedDate      DATETIME		= GETDATE
	,@ModifiedBy        INT				= 1
)
AS 
BEGIN

	-- SET NOCOUNT ON added to prevent extra result sets from
	SET NOCOUNT OFF;

	--Comment : Here If record exists then update otherwise insert new one
	IF NOT EXISTS(SELECT QuoteId,SessionData FROM PurchaePathCustomSession WHERE QuoteId=@QuoteId)
	BEGIN		
		INSERT INTO PurchaePathCustomSession(Id,QuoteId,SessionData,CreatedBy,CreatedDate,IsActive) VALUES
		((NEXT VALUE FOR SEQUENCEPurchaePathCustomSession),@QuoteId,CAST(@SessionData AS VARCHAR(MAX)),@CreatedBy,@CreatedDate,@IsActive);
	END
	ELSE
	BEGIN
		UPDATE PurchaePathCustomSession SET SessionData=CAST(@SessionData AS VARCHAR(MAX)),ModifiedBy=@ModifiedBy,ModifiedDate=@ModifiedDate WHERE QuoteId=@QuoteId;
	END

END





