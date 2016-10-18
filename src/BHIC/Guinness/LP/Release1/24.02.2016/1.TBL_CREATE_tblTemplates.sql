CREATE TABLE [dbo].[tblTemplates](
	[Id] [bigint] IDENTITY(1,1) NOT NULL,
	[Template] [nvarchar](max) NOT NULL,
	[Logo] [nvarchar](50) NOT NULL,
	[Status] [nvarchar](50) NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO


