CREATE TABLE [dbo].[UserQuotes](
	[Id] [bigint] NOT NULL,
	[OrganizationUserDetailID] [bigint] NOT NULL,
	[QuoteId] [bigint] NOT NULL,
	[StartDate] [datetime] NOT NULL,
	[EndDate] [datetime] NULL,
	[CreatedDate] [datetime] NOT NULL,
	[CreatedBy] [int] NOT NULL,
	[ModifiedDate] [datetime] NOT NULL,
	[ModifiedBy] [int] NOT NULL,
 CONSTRAINT [PK_UserQuote] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

ALTER TABLE [dbo].[UserQuotes] ADD  CONSTRAINT [DF_UserQuote_StartDate]  DEFAULT (getdate()) FOR [StartDate]
GO

ALTER TABLE [dbo].[UserQuotes] ADD  CONSTRAINT [DF_UserQuote_CreatedDate]  DEFAULT (getdate()) FOR [CreatedDate]
GO


