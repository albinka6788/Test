
/****** Object:  Table [dbo].[tblTemplates]    Script Date: 24-08-2016 12:34:32 ******/
DROP TABLE [dbo].[tblTemplates]
GO

/****** Object:  Table [dbo].[tblTemplates]    Script Date: 24-08-2016 12:34:32 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[tblTemplates](
	[Id] [bigint] IDENTITY(1,1) NOT NULL,
	[Template] [nvarchar](max) NOT NULL,
	[Logo] [nvarchar](50) NOT NULL,
	[Status] [nvarchar](50) NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO


