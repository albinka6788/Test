
ALTER TABLE [dbo].[CTAMessages] DROP CONSTRAINT [DF__CTAMessag__Creat__58F12BAE]
GO

/****** Object:  Table [dbo].[CTAMessages]    Script Date: 24-08-2016 12:33:14 ******/
DROP TABLE [dbo].[CTAMessages]
GO

/****** Object:  Table [dbo].[CTAMessages]    Script Date: 24-08-2016 12:33:14 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

SET ANSI_PADDING ON
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


