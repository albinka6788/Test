-- ================================================
/*
@Author      : Navneet Kumar
@Description : This proc will use for Transaction Log
@CreatedOn   : 2015-Dec-12 

@Updation History : 
   @Update-0 : {01/06/2015}, {Navneet}, {added browser,version,lob} 
   @Update-1 : {02/24/2016}, {Navneet}, {added AdId}
@ExampleCall : 
EXEC AddTransactionLog

*/
-- ================================================
ALTER PROCEDURE [dbo].[AddTransactionLog] 
	@UserIP varchar(20) ,
	@RequestUrl varchar(300) ,
	@RequestType varchar(20) ,
	@RequestDateTime datetime ,
	@ResponseDateTime datetime = null ,
	@RequestProcessTime bigint = null ,
	@ResponseSize bigint = null ,
	@TotalAPICalls int = null  ,
	@TotalAPIProcessTime bigint = null ,
	@TotalDBCalls int = null ,
	@TotalDBProcessTime bigint = null ,
	@PaymentErrorDetail varchar(max) = null,
	@QuoteId bigint = null,
	@UserId varchar(50) = null,
	@ThreadId varchar(32) =null,
	@Browser varchar(50) = null,
	@BrowserVersion varchar(20) = null,
	@Lob varchar(2) = null,
	@AdId bigint = null,
	@TransactionLogId BIGINT OUTPUT
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- INTerfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
 Insert into TransactionLog Select @UserIP, @RequestUrl,@RequestType, @RequestDateTime, @ResponseDateTime, @RequestProcessTime ,
 @ResponseSize , @TotalAPICalls , @TotalAPIProcessTime , @TotalDBCalls , @TotalDBProcessTime , @PaymentErrorDetail,@QuoteId,@UserId,@ThreadId,@Browser,@BrowserVersion,@Lob,@AdId 
 Select @TransactionLogId = SCOPE_IDENTITY(); 

END


go

Alter table TransactionLog add AdId bigint