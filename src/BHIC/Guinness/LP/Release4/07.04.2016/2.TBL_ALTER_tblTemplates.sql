

/****** Object:  Table [dbo].[tblTemplates]    Script Date: 04/07/2016 12:31:09 PM ******/
Drop TABLE [dbo].[tblTemplates]
GO

CREATE TABLE [dbo].[tblTemplates](
	[Id] [bigint] IDENTITY(1,1) NOT NULL,
	[Template] [nvarchar](max) NOT NULL,
	[Logo] [nvarchar](50) NOT NULL,
	[Status] [nvarchar](50) NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
SET IDENTITY_INSERT [dbo].[tblTemplates] ON 

GO
INSERT [dbo].[tblTemplates] ([Id], [Template], [Logo], [Status]) VALUES (1, N'Template1.cshtml', N'/LandingPage/Images/Logo/logo.png', N'Active')
GO
INSERT [dbo].[tblTemplates] ([Id], [Template], [Logo], [Status]) VALUES (2, N'Template2.cshtml', N'/LandingPage/Images/Logo/logo.png', N'Active')
GO
INSERT [dbo].[tblTemplates] ([Id], [Template], [Logo], [Status]) VALUES (3, N'Template3.cshtml', N'/LandingPage/Images/Logo/logo.png', N'Active')
GO
INSERT [dbo].[tblTemplates] ([Id], [Template], [Logo], [Status]) VALUES (4, N'Template4.cshtml', N'/LandingPage/Images/Logo/logo.png', N'Active')
GO
INSERT [dbo].[tblTemplates] ([Id], [Template], [Logo], [Status]) VALUES (5, N'Template5.cshtml',  N'/LandingPage/Images/Logo/logo.png', N'Active')
GO
SET IDENTITY_INSERT [dbo].[tblTemplates] OFF
GO


