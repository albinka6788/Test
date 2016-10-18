
/****** Object:  Table [dbo].[CTAMessages]    Script Date: 14-03-2016 17:06:21 ******/
DROP TABLE [dbo].[CTAMessages]
GO

CREATE TABLE [dbo].[CTAMessages](
	[Id] [bigint] IDENTITY(1,1) NOT NULL,
	[TokenId] [varchar](32) NULL,
	[Message] [text] NULL,
	[CreatedBy] [bigint] NOT NULL,
	[CreatedOn] [datetime] NULL,
	[UpdatedBy] [bigint] NULL,
	[UpdatedOn] [datetime] NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO

SET ANSI_PADDING OFF
GO

ALTER TABLE [dbo].[CTAMessages] ADD  DEFAULT (getdate()) FOR [CreatedOn]
GO


