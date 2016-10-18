ALTER TABLE [dbo].[LandingPageTransaction] DROP CONSTRAINT [DF__LandingPa__Creat__5708E33C]
GO

/****** Object:  Table [dbo].[LandingPageTransaction]    Script Date: 29-08-2016 14:25:32 ******/
DROP TABLE [dbo].[LandingPageTransaction]
GO

/****** Object:  Table [dbo].[LandingPageTransaction]    Script Date: 29-08-2016 14:25:32 ******/
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
	[PageName] [varchar](200) NULL,
	[Heading] [text] NULL,
	[SubHeading] [text] NULL,
	[ProductName] [text] NULL,
	[ProductHighlight] [text] NULL,
	[BTNText] [varchar](100) NULL,
	[CalloutText] [text] NULL,
	[MainImage] [varchar](100) NULL,
	[TemplateId] [int] NULL,
	[Controller] [varchar](50) NULL,
	[ActionResult] [varchar](50) NULL,
	[TokenId] [varchar](32) NULL,
	[CreatedBy] [bigint] NOT NULL,
	[CreatedOn] [datetime] NULL,
	[UpdatedBy] [bigint] NOT NULL,
	[UpdatedOn] [datetime] NULL,
	[IsDeleted] [bit] NOT NULL,
	[IsDeployed] [bit] NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO

SET ANSI_PADDING OFF
GO

ALTER TABLE [dbo].[LandingPageTransaction] ADD  DEFAULT (getdate()) FOR [CreatedOn]
GO


