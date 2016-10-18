/****** Object:  Table [dbo].[ApiTransaction]    Script Date: 01/05/2016 5:33:29 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[ApiTransaction](
	[ApiTransactionId] [bigint] IDENTITY(1,1) NOT NULL,
	[TransactionLogId] [bigint] NOT NULL,
	[ApiCallType] [varchar](20) NULL,
	[ApiName] [varchar](100) NULL,
	[ApiCallRequestTime] [datetime] NULL,
	[ApiCallResponseTime] [datetime] NULL,
	[ApiRequestProcessTime] [bigint] NULL,
	[ApiRequestSize] [bigint] NULL,
	[ApiResponseSize] [bigint] NULL
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[DbTransaction]    Script Date: 01/05/2016 5:33:29 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[DbTransaction](
	[DbTransactionId] [bigint] IDENTITY(1,1) NOT NULL,
	[TransactionLogId] [bigint] NULL,
	[DbCallRequestTime] [datetime] NULL,
	[DbCallResponseTime] [datetime] NULL,
	[DbRequestProcessTime] [bigint] NULL,
	[DbProcName] [varchar](100) NULL
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[TransactionLog]    Script Date: 01/05/2016 5:33:29 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[TransactionLog](
	[TransactionLogId] [bigint] IDENTITY(1,1) NOT NULL,
	[UserIP] [varchar](20) NULL,
	[RequestUrl] [varchar](300) NULL,
	[RequestType] [varchar](20) NULL,
	[RequestDateTime] [datetime] NULL,
	[ResponseDateTime] [datetime] NULL,
	[RequestProcessTime] [bigint] NULL,
	[ResponseSize] [bigint] NULL,
	[TotalAPICalls] [int] NULL,
	[TotalAPIProcessTime] [bigint] NULL,
	[TotalDBCalls] [int] NULL,
	[TotalDBProcessTime] [bigint] NULL,
	[PaymentErrorDetail] [varchar](max) NULL,
	[QuoteId] [bigint] NULL,
	[UserId] [varchar](50) NULL,
	[ThreadId] [varchar](32) NULL,
 CONSTRAINT [PK_TransactionLog] PRIMARY KEY CLUSTERED 
(
	[TransactionLogId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
ALTER TABLE [dbo].[ApiTransaction] ADD  CONSTRAINT [DF__ApiTransa__ApiRe__351DDF8C]  DEFAULT ((0)) FOR [ApiRequestProcessTime]
GO
ALTER TABLE [dbo].[ApiTransaction] ADD  CONSTRAINT [DF__ApiTransa__ApiRe__361203C5]  DEFAULT ((0)) FOR [ApiRequestSize]
GO
ALTER TABLE [dbo].[ApiTransaction] ADD  CONSTRAINT [DF__ApiTransa__ApiRe__370627FE]  DEFAULT ((0)) FOR [ApiResponseSize]
GO
ALTER TABLE [dbo].[DbTransaction] ADD  DEFAULT ((0)) FOR [DbRequestProcessTime]
GO
ALTER TABLE [dbo].[TransactionLog] ADD  DEFAULT ((0)) FOR [RequestProcessTime]
GO
ALTER TABLE [dbo].[TransactionLog] ADD  DEFAULT ((0)) FOR [ResponseSize]
GO
ALTER TABLE [dbo].[TransactionLog] ADD  DEFAULT ((0)) FOR [TotalAPICalls]
GO
ALTER TABLE [dbo].[TransactionLog] ADD  DEFAULT ((0)) FOR [TotalAPIProcessTime]
GO
ALTER TABLE [dbo].[TransactionLog] ADD  DEFAULT ((0)) FOR [TotalDBCalls]
GO
ALTER TABLE [dbo].[TransactionLog] ADD  DEFAULT ((0)) FOR [TotalDBProcessTime]
GO
ALTER TABLE [dbo].[ApiTransaction]  WITH CHECK ADD  CONSTRAINT [FK_ApiTransaction_TransactionLog] FOREIGN KEY([TransactionLogId])
REFERENCES [dbo].[TransactionLog] ([TransactionLogId])
GO
ALTER TABLE [dbo].[ApiTransaction] CHECK CONSTRAINT [FK_ApiTransaction_TransactionLog]
GO
ALTER TABLE [dbo].[DbTransaction]  WITH CHECK ADD  CONSTRAINT [FK_DbTransaction_TransactionLog] FOREIGN KEY([TransactionLogId])
REFERENCES [dbo].[TransactionLog] ([TransactionLogId])
GO
ALTER TABLE [dbo].[DbTransaction] CHECK CONSTRAINT [FK_DbTransaction_TransactionLog]
GO
