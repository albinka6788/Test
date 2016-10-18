
CREATE TABLE [dbo].[CTAMessages](
	[Id] [bigint] IDENTITY(1,1) NOT NULL,
	[TokenId] [varchar](32) NULL,
	[Message] [text] NULL,
	[CreatedOn] [datetime] NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO

SET ANSI_PADDING OFF
GO

ALTER TABLE [dbo].[CTAMessages] ADD  DEFAULT (getdate()) FOR [CreatedOn]
GO


