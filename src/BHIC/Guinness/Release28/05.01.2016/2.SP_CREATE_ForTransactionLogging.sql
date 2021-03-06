
/****** Object:  StoredProcedure [dbo].[AddApiTransaction]    Script Date: 01/05/2016 5:33:29 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[AddApiTransaction] 
	@TransactionLogId bigint,
	@ApiCallType varchar(20) ,
	@ApiName varchar(100) ,
	@ApiCallRequestTime datetime ,
	@ApiCallResponseTime datetime = null,
	@ApiRequestProcessTime bigint = null ,
	@ApiRequestSize bigint = null,
	@ApiResponseSize bigint = null 
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	Insert into [dbo].[ApiTransaction] Select @TransactionLogId,@ApiCallType,@ApiName,@ApiCallRequestTime,@ApiCallResponseTime,@ApiRequestProcessTime,@ApiRequestSize,@ApiResponseSize
END

GO
/****** Object:  StoredProcedure [dbo].[AddDbTransaction]    Script Date: 01/05/2016 5:33:29 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		Navneet Kumar
-- Create date: 12/14/2015
-- Description:	Add DB Transaction Log
-- =============================================
CREATE PROCEDURE [dbo].[AddDbTransaction]
@TransactionLogId bigint ,
@DbCallRequestTime datetime ,
@DbCallResponseTime datetime = null ,
@DbRequestProcessTime bigint= null ,
@DbProcName varchar(100) = null
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;
	Insert into DbTransaction Select @TransactionLogId,@DbCallRequestTime,@DbCallResponseTime,@DbRequestProcessTime,@DbProcName
 
END

GO
/****** Object:  StoredProcedure [dbo].[AddTransactionLog]    Script Date: 01/05/2016 5:33:29 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		Navneet Kumar
-- Create date: 12/09/2015
-- Description:	Use for Transaction Log
-- =============================================
CREATE PROCEDURE [dbo].[AddTransactionLog] 
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
	@TransactionLogId BIGINT OUTPUT
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- INTerfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
 Insert INTo TransactionLog Select @UserIP, @RequestUrl,@RequestType, @RequestDateTime, @ResponseDateTime, @RequestProcessTime ,
 @ResponseSize , @TotalAPICalls , @TotalAPIProcessTime , @TotalDBCalls , @TotalDBProcessTime , @PaymentErrorDetail,@QuoteId,@UserId,@ThreadId
 Select @TransactionLogId = SCOPE_IDENTITY(); 

END

GO