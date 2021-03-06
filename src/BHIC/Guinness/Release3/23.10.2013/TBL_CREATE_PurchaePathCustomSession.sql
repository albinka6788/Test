
/****** Object:  Table [dbo].[PurchaePathCustomSession]    Script Date: 26-10-2015 PM 05:31:51 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[PurchaePathCustomSession](
	[Id] [int] NOT NULL,
	[QuoteId] [bigint] NOT NULL,
	[SessionData] [varchar](max) NULL,
	[IsActive] [bit] NULL,
	[CreatedDate] [datetime] NULL CONSTRAINT [DF_PurchaePathCustomSession_CreatedDate]  DEFAULT (getdate()),
	[CreatedBy] [int] NULL,
	[ModifiedDate] [datetime] NULL CONSTRAINT [DF_PurchaePathCustomSession_ModifiedDate]  DEFAULT (getdate()),
	[ModifiedBy] [int] NULL,
 CONSTRAINT [PK_PurchaePathCustomSession] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
