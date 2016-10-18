/****** Object:  Table [dbo].[LandingPageTransaction]    Script Date: 24-02-2016 17:42:39 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

SET ANSI_PADDING ON
GO

CREATE TABLE [dbo].[LandingPageTransaction](
	[Id] [bigint] IDENTITY(1,1) NOT NULL,
	[lob] [varchar](3) NULL,
	[State] [varchar](3) NULL,
	[TransactionCounter] [bigint] NULL,
	[logo] [varchar](200) NULL,
	[Title] [text] NULL,
	[Description] [text] NULL,
	[BTNText] [varchar](100) NULL,
	[CTABoxTitle] [varchar](100) NULL,
	[header] [varchar](100) NULL,
	[footer] [varchar](100) NULL,
	[MainImage] [varchar](100) NULL,
	[TemplateId] [int] NULL,
	[Controller] [varchar](50) NULL,
	[ActionResult] [varchar](50) NULL,
	[TokenId] [varchar](32) NULL,
	[CreatedOn] [datetime] NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO

SET ANSI_PADDING OFF
GO

ALTER TABLE [dbo].[LandingPageTransaction] ADD  DEFAULT (getdate()) FOR [CreatedOn]
GO


