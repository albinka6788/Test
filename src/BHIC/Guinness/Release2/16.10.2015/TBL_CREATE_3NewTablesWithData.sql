
/****** Object:  Table [dbo].[LineOfBusiness]    Script Date: 16-10-2015 PM 09:09:57 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[LineOfBusiness](
	[Id] [int] NOT NULL,
	[Abbreviation] [varchar](20) NOT NULL,
	[LineOfBusinessName] [varchar](50) NULL,
	[IsActive] [bit] NOT NULL,
	[CreatedDate] [datetime] NULL CONSTRAINT [DF_LineOfBusiness_CreatedDate]  DEFAULT (getdate()),
	[CreatedBy] [int] NULL,
	[ModifiedDate] [datetime] NULL CONSTRAINT [DF_LineOfBusiness_ModifiedDate]  DEFAULT (getdate()),
	[ModifiedBy] [int] NULL,
 CONSTRAINT [PK_LineOfBusiness] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[StateLineOfBusinesses]    Script Date: 16-10-2015 PM 09:09:57 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[StateLineOfBusinesses](
	[Id] [int] NOT NULL,
	[StateId] [int] NOT NULL,
	[LineOfBusinessId] [int] NOT NULL,
	[IsActive] [bit] NOT NULL,
	[CreatedDate] [datetime] NULL CONSTRAINT [DF_StateLineOfBusinesses_CreatedDate]  DEFAULT (getdate()),
	[CreatedBy] [int] NULL,
	[ModifiedDate] [datetime] NULL CONSTRAINT [DF_StateLineOfBusinesses_ModifiedDate]  DEFAULT (getdate()),
	[ModifiedBy] [int] NULL,
 CONSTRAINT [PK_StateLineOfBusinesses] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[StateMaster]    Script Date: 16-10-2015 PM 09:09:57 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[StateMaster](
	[Id] [int] NOT NULL,
	[StateCode] [char](2) NOT NULL,
	[FullName] [varchar](20) NULL,
	[IsActive] [bit] NOT NULL,
	[CreatedDate] [datetime] NULL CONSTRAINT [DF_StateMaster_CreatedDate]  DEFAULT (getdate()),
	[CreatedBy] [int] NULL,
	[ModifiedDate] [datetime] NULL CONSTRAINT [DF_StateMaster_ModifiedDate]  DEFAULT (getdate()),
	[ModifiedBy] [int] NULL,
 CONSTRAINT [PK_StateMaster] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
INSERT [dbo].[LineOfBusiness] ([Id], [Abbreviation], [LineOfBusinessName], [IsActive], [CreatedDate], [CreatedBy], [ModifiedDate], [ModifiedBy]) VALUES (1, N'WC', N'Worker Compensation', 1, CAST(N'2015-10-15 00:00:00.000' AS DateTime), 1, CAST(N'2015-10-15 00:00:00.000' AS DateTime), NULL)
GO
INSERT [dbo].[LineOfBusiness] ([Id], [Abbreviation], [LineOfBusinessName], [IsActive], [CreatedDate], [CreatedBy], [ModifiedDate], [ModifiedBy]) VALUES (2, N'BOP', N'Business Owner Policy', 1, CAST(N'2015-10-15 00:00:00.000' AS DateTime), 1, CAST(N'2015-10-15 00:00:00.000' AS DateTime), NULL)
GO
INSERT [dbo].[StateLineOfBusinesses] ([Id], [StateId], [LineOfBusinessId], [IsActive], [CreatedDate], [CreatedBy], [ModifiedDate], [ModifiedBy]) VALUES (1, 1, 1, 1, CAST(N'2015-10-16 20:19:43.953' AS DateTime), 1, CAST(N'2015-10-16 20:19:43.953' AS DateTime), NULL)
GO
INSERT [dbo].[StateLineOfBusinesses] ([Id], [StateId], [LineOfBusinessId], [IsActive], [CreatedDate], [CreatedBy], [ModifiedDate], [ModifiedBy]) VALUES (2, 2, 1, 1, CAST(N'2015-10-16 20:19:43.953' AS DateTime), 1, CAST(N'2015-10-16 20:19:43.953' AS DateTime), NULL)
GO
INSERT [dbo].[StateLineOfBusinesses] ([Id], [StateId], [LineOfBusinessId], [IsActive], [CreatedDate], [CreatedBy], [ModifiedDate], [ModifiedBy]) VALUES (3, 3, 1, 1, CAST(N'2015-10-16 20:19:43.953' AS DateTime), 1, CAST(N'2015-10-16 20:19:43.953' AS DateTime), NULL)
GO
INSERT [dbo].[StateLineOfBusinesses] ([Id], [StateId], [LineOfBusinessId], [IsActive], [CreatedDate], [CreatedBy], [ModifiedDate], [ModifiedBy]) VALUES (4, 4, 1, 1, CAST(N'2015-10-16 20:19:43.953' AS DateTime), 1, CAST(N'2015-10-16 20:19:43.953' AS DateTime), NULL)
GO
INSERT [dbo].[StateLineOfBusinesses] ([Id], [StateId], [LineOfBusinessId], [IsActive], [CreatedDate], [CreatedBy], [ModifiedDate], [ModifiedBy]) VALUES (5, 5, 1, 1, CAST(N'2015-10-16 20:19:43.953' AS DateTime), 1, CAST(N'2015-10-16 20:19:43.953' AS DateTime), NULL)
GO
INSERT [dbo].[StateLineOfBusinesses] ([Id], [StateId], [LineOfBusinessId], [IsActive], [CreatedDate], [CreatedBy], [ModifiedDate], [ModifiedBy]) VALUES (6, 6, 1, 1, CAST(N'2015-10-16 20:19:43.953' AS DateTime), 1, CAST(N'2015-10-16 20:19:43.953' AS DateTime), NULL)
GO
INSERT [dbo].[StateLineOfBusinesses] ([Id], [StateId], [LineOfBusinessId], [IsActive], [CreatedDate], [CreatedBy], [ModifiedDate], [ModifiedBy]) VALUES (7, 7, 1, 1, CAST(N'2015-10-16 20:19:43.953' AS DateTime), 1, CAST(N'2015-10-16 20:19:43.953' AS DateTime), NULL)
GO
INSERT [dbo].[StateLineOfBusinesses] ([Id], [StateId], [LineOfBusinessId], [IsActive], [CreatedDate], [CreatedBy], [ModifiedDate], [ModifiedBy]) VALUES (8, 8, 1, 1, CAST(N'2015-10-16 20:19:43.953' AS DateTime), 1, CAST(N'2015-10-16 20:19:43.953' AS DateTime), NULL)
GO
INSERT [dbo].[StateLineOfBusinesses] ([Id], [StateId], [LineOfBusinessId], [IsActive], [CreatedDate], [CreatedBy], [ModifiedDate], [ModifiedBy]) VALUES (9, 9, 1, 1, CAST(N'2015-10-16 20:19:43.953' AS DateTime), 1, CAST(N'2015-10-16 20:19:43.953' AS DateTime), NULL)
GO
INSERT [dbo].[StateLineOfBusinesses] ([Id], [StateId], [LineOfBusinessId], [IsActive], [CreatedDate], [CreatedBy], [ModifiedDate], [ModifiedBy]) VALUES (10, 10, 1, 1, CAST(N'2015-10-16 20:19:43.953' AS DateTime), 1, CAST(N'2015-10-16 20:19:43.953' AS DateTime), NULL)
GO
INSERT [dbo].[StateLineOfBusinesses] ([Id], [StateId], [LineOfBusinessId], [IsActive], [CreatedDate], [CreatedBy], [ModifiedDate], [ModifiedBy]) VALUES (11, 11, 1, 1, CAST(N'2015-10-16 20:19:43.953' AS DateTime), 1, CAST(N'2015-10-16 20:19:43.953' AS DateTime), NULL)
GO
INSERT [dbo].[StateLineOfBusinesses] ([Id], [StateId], [LineOfBusinessId], [IsActive], [CreatedDate], [CreatedBy], [ModifiedDate], [ModifiedBy]) VALUES (12, 12, 1, 1, CAST(N'2015-10-16 20:19:43.953' AS DateTime), 1, CAST(N'2015-10-16 20:19:43.953' AS DateTime), NULL)
GO
INSERT [dbo].[StateLineOfBusinesses] ([Id], [StateId], [LineOfBusinessId], [IsActive], [CreatedDate], [CreatedBy], [ModifiedDate], [ModifiedBy]) VALUES (13, 13, 1, 1, CAST(N'2015-10-16 20:19:43.953' AS DateTime), 1, CAST(N'2015-10-16 20:19:43.953' AS DateTime), NULL)
GO
INSERT [dbo].[StateLineOfBusinesses] ([Id], [StateId], [LineOfBusinessId], [IsActive], [CreatedDate], [CreatedBy], [ModifiedDate], [ModifiedBy]) VALUES (14, 14, 1, 1, CAST(N'2015-10-16 20:19:43.953' AS DateTime), 1, CAST(N'2015-10-16 20:19:43.953' AS DateTime), NULL)
GO
INSERT [dbo].[StateLineOfBusinesses] ([Id], [StateId], [LineOfBusinessId], [IsActive], [CreatedDate], [CreatedBy], [ModifiedDate], [ModifiedBy]) VALUES (15, 15, 1, 1, CAST(N'2015-10-16 20:19:43.953' AS DateTime), 1, CAST(N'2015-10-16 20:19:43.953' AS DateTime), NULL)
GO
INSERT [dbo].[StateLineOfBusinesses] ([Id], [StateId], [LineOfBusinessId], [IsActive], [CreatedDate], [CreatedBy], [ModifiedDate], [ModifiedBy]) VALUES (16, 16, 1, 1, CAST(N'2015-10-16 20:19:43.953' AS DateTime), 1, CAST(N'2015-10-16 20:19:43.953' AS DateTime), NULL)
GO
INSERT [dbo].[StateLineOfBusinesses] ([Id], [StateId], [LineOfBusinessId], [IsActive], [CreatedDate], [CreatedBy], [ModifiedDate], [ModifiedBy]) VALUES (17, 17, 1, 1, CAST(N'2015-10-16 20:19:43.953' AS DateTime), 1, CAST(N'2015-10-16 20:19:43.953' AS DateTime), NULL)
GO
INSERT [dbo].[StateLineOfBusinesses] ([Id], [StateId], [LineOfBusinessId], [IsActive], [CreatedDate], [CreatedBy], [ModifiedDate], [ModifiedBy]) VALUES (18, 18, 1, 1, CAST(N'2015-10-16 20:19:43.953' AS DateTime), 1, CAST(N'2015-10-16 20:19:43.953' AS DateTime), NULL)
GO
INSERT [dbo].[StateLineOfBusinesses] ([Id], [StateId], [LineOfBusinessId], [IsActive], [CreatedDate], [CreatedBy], [ModifiedDate], [ModifiedBy]) VALUES (19, 19, 1, 1, CAST(N'2015-10-16 20:19:43.953' AS DateTime), 1, CAST(N'2015-10-16 20:19:43.953' AS DateTime), NULL)
GO
INSERT [dbo].[StateLineOfBusinesses] ([Id], [StateId], [LineOfBusinessId], [IsActive], [CreatedDate], [CreatedBy], [ModifiedDate], [ModifiedBy]) VALUES (20, 20, 1, 1, CAST(N'2015-10-16 20:19:43.953' AS DateTime), 1, CAST(N'2015-10-16 20:19:43.953' AS DateTime), NULL)
GO
INSERT [dbo].[StateLineOfBusinesses] ([Id], [StateId], [LineOfBusinessId], [IsActive], [CreatedDate], [CreatedBy], [ModifiedDate], [ModifiedBy]) VALUES (21, 21, 1, 1, CAST(N'2015-10-16 20:19:43.953' AS DateTime), 1, CAST(N'2015-10-16 20:19:43.953' AS DateTime), NULL)
GO
INSERT [dbo].[StateLineOfBusinesses] ([Id], [StateId], [LineOfBusinessId], [IsActive], [CreatedDate], [CreatedBy], [ModifiedDate], [ModifiedBy]) VALUES (22, 22, 1, 1, CAST(N'2015-10-16 20:19:43.953' AS DateTime), 1, CAST(N'2015-10-16 20:19:43.953' AS DateTime), NULL)
GO
INSERT [dbo].[StateLineOfBusinesses] ([Id], [StateId], [LineOfBusinessId], [IsActive], [CreatedDate], [CreatedBy], [ModifiedDate], [ModifiedBy]) VALUES (23, 23, 1, 1, CAST(N'2015-10-16 20:19:43.953' AS DateTime), 1, CAST(N'2015-10-16 20:19:43.953' AS DateTime), NULL)
GO
INSERT [dbo].[StateLineOfBusinesses] ([Id], [StateId], [LineOfBusinessId], [IsActive], [CreatedDate], [CreatedBy], [ModifiedDate], [ModifiedBy]) VALUES (24, 24, 1, 1, CAST(N'2015-10-16 20:19:43.953' AS DateTime), 1, CAST(N'2015-10-16 20:19:43.953' AS DateTime), NULL)
GO
INSERT [dbo].[StateLineOfBusinesses] ([Id], [StateId], [LineOfBusinessId], [IsActive], [CreatedDate], [CreatedBy], [ModifiedDate], [ModifiedBy]) VALUES (25, 25, 1, 1, CAST(N'2015-10-16 20:19:43.953' AS DateTime), 1, CAST(N'2015-10-16 20:19:43.953' AS DateTime), NULL)
GO
INSERT [dbo].[StateLineOfBusinesses] ([Id], [StateId], [LineOfBusinessId], [IsActive], [CreatedDate], [CreatedBy], [ModifiedDate], [ModifiedBy]) VALUES (26, 26, 1, 1, CAST(N'2015-10-16 20:19:43.953' AS DateTime), 1, CAST(N'2015-10-16 20:19:43.953' AS DateTime), NULL)
GO
INSERT [dbo].[StateLineOfBusinesses] ([Id], [StateId], [LineOfBusinessId], [IsActive], [CreatedDate], [CreatedBy], [ModifiedDate], [ModifiedBy]) VALUES (27, 27, 1, 1, CAST(N'2015-10-16 20:19:43.953' AS DateTime), 1, CAST(N'2015-10-16 20:19:43.953' AS DateTime), NULL)
GO
INSERT [dbo].[StateLineOfBusinesses] ([Id], [StateId], [LineOfBusinessId], [IsActive], [CreatedDate], [CreatedBy], [ModifiedDate], [ModifiedBy]) VALUES (28, 28, 1, 1, CAST(N'2015-10-16 20:19:43.953' AS DateTime), 1, CAST(N'2015-10-16 20:19:43.953' AS DateTime), NULL)
GO
INSERT [dbo].[StateLineOfBusinesses] ([Id], [StateId], [LineOfBusinessId], [IsActive], [CreatedDate], [CreatedBy], [ModifiedDate], [ModifiedBy]) VALUES (29, 29, 1, 1, CAST(N'2015-10-16 20:19:43.953' AS DateTime), 1, CAST(N'2015-10-16 20:19:43.953' AS DateTime), NULL)
GO
INSERT [dbo].[StateLineOfBusinesses] ([Id], [StateId], [LineOfBusinessId], [IsActive], [CreatedDate], [CreatedBy], [ModifiedDate], [ModifiedBy]) VALUES (30, 30, 1, 1, CAST(N'2015-10-16 20:19:43.953' AS DateTime), 1, CAST(N'2015-10-16 20:19:43.953' AS DateTime), NULL)
GO
INSERT [dbo].[StateLineOfBusinesses] ([Id], [StateId], [LineOfBusinessId], [IsActive], [CreatedDate], [CreatedBy], [ModifiedDate], [ModifiedBy]) VALUES (31, 31, 1, 1, CAST(N'2015-10-16 20:19:43.953' AS DateTime), 1, CAST(N'2015-10-16 20:19:43.953' AS DateTime), NULL)
GO
INSERT [dbo].[StateLineOfBusinesses] ([Id], [StateId], [LineOfBusinessId], [IsActive], [CreatedDate], [CreatedBy], [ModifiedDate], [ModifiedBy]) VALUES (32, 32, 1, 1, CAST(N'2015-10-16 20:19:43.953' AS DateTime), 1, CAST(N'2015-10-16 20:19:43.953' AS DateTime), NULL)
GO
INSERT [dbo].[StateLineOfBusinesses] ([Id], [StateId], [LineOfBusinessId], [IsActive], [CreatedDate], [CreatedBy], [ModifiedDate], [ModifiedBy]) VALUES (33, 33, 1, 1, CAST(N'2015-10-16 20:19:43.953' AS DateTime), 1, CAST(N'2015-10-16 20:19:43.953' AS DateTime), NULL)
GO
INSERT [dbo].[StateLineOfBusinesses] ([Id], [StateId], [LineOfBusinessId], [IsActive], [CreatedDate], [CreatedBy], [ModifiedDate], [ModifiedBy]) VALUES (34, 34, 1, 1, CAST(N'2015-10-16 20:19:43.953' AS DateTime), 1, CAST(N'2015-10-16 20:19:43.953' AS DateTime), NULL)
GO
INSERT [dbo].[StateLineOfBusinesses] ([Id], [StateId], [LineOfBusinessId], [IsActive], [CreatedDate], [CreatedBy], [ModifiedDate], [ModifiedBy]) VALUES (35, 35, 1, 1, CAST(N'2015-10-16 20:19:43.953' AS DateTime), 1, CAST(N'2015-10-16 20:19:43.953' AS DateTime), NULL)
GO
INSERT [dbo].[StateLineOfBusinesses] ([Id], [StateId], [LineOfBusinessId], [IsActive], [CreatedDate], [CreatedBy], [ModifiedDate], [ModifiedBy]) VALUES (36, 36, 1, 1, CAST(N'2015-10-16 20:19:43.953' AS DateTime), 1, CAST(N'2015-10-16 20:19:43.953' AS DateTime), NULL)
GO
INSERT [dbo].[StateLineOfBusinesses] ([Id], [StateId], [LineOfBusinessId], [IsActive], [CreatedDate], [CreatedBy], [ModifiedDate], [ModifiedBy]) VALUES (37, 37, 1, 1, CAST(N'2015-10-16 20:19:43.953' AS DateTime), 1, CAST(N'2015-10-16 20:19:43.953' AS DateTime), NULL)
GO
INSERT [dbo].[StateLineOfBusinesses] ([Id], [StateId], [LineOfBusinessId], [IsActive], [CreatedDate], [CreatedBy], [ModifiedDate], [ModifiedBy]) VALUES (38, 38, 1, 1, CAST(N'2015-10-16 20:19:43.953' AS DateTime), 1, CAST(N'2015-10-16 20:19:43.953' AS DateTime), NULL)
GO
INSERT [dbo].[StateLineOfBusinesses] ([Id], [StateId], [LineOfBusinessId], [IsActive], [CreatedDate], [CreatedBy], [ModifiedDate], [ModifiedBy]) VALUES (39, 39, 1, 1, CAST(N'2015-10-16 20:19:43.953' AS DateTime), 1, CAST(N'2015-10-16 20:19:43.953' AS DateTime), NULL)
GO
INSERT [dbo].[StateLineOfBusinesses] ([Id], [StateId], [LineOfBusinessId], [IsActive], [CreatedDate], [CreatedBy], [ModifiedDate], [ModifiedBy]) VALUES (40, 40, 1, 1, CAST(N'2015-10-16 20:19:43.953' AS DateTime), 1, CAST(N'2015-10-16 20:19:43.953' AS DateTime), NULL)
GO
INSERT [dbo].[StateLineOfBusinesses] ([Id], [StateId], [LineOfBusinessId], [IsActive], [CreatedDate], [CreatedBy], [ModifiedDate], [ModifiedBy]) VALUES (41, 41, 1, 1, CAST(N'2015-10-16 20:19:43.953' AS DateTime), 1, CAST(N'2015-10-16 20:19:43.953' AS DateTime), NULL)
GO
INSERT [dbo].[StateLineOfBusinesses] ([Id], [StateId], [LineOfBusinessId], [IsActive], [CreatedDate], [CreatedBy], [ModifiedDate], [ModifiedBy]) VALUES (42, 42, 1, 1, CAST(N'2015-10-16 20:19:43.953' AS DateTime), 1, CAST(N'2015-10-16 20:19:43.953' AS DateTime), NULL)
GO
INSERT [dbo].[StateLineOfBusinesses] ([Id], [StateId], [LineOfBusinessId], [IsActive], [CreatedDate], [CreatedBy], [ModifiedDate], [ModifiedBy]) VALUES (43, 43, 1, 1, CAST(N'2015-10-16 20:19:43.953' AS DateTime), 1, CAST(N'2015-10-16 20:19:43.953' AS DateTime), NULL)
GO
INSERT [dbo].[StateLineOfBusinesses] ([Id], [StateId], [LineOfBusinessId], [IsActive], [CreatedDate], [CreatedBy], [ModifiedDate], [ModifiedBy]) VALUES (44, 44, 1, 1, CAST(N'2015-10-16 20:19:43.953' AS DateTime), 1, CAST(N'2015-10-16 20:19:43.953' AS DateTime), NULL)
GO
INSERT [dbo].[StateLineOfBusinesses] ([Id], [StateId], [LineOfBusinessId], [IsActive], [CreatedDate], [CreatedBy], [ModifiedDate], [ModifiedBy]) VALUES (45, 45, 1, 1, CAST(N'2015-10-16 20:19:43.953' AS DateTime), 1, CAST(N'2015-10-16 20:19:43.953' AS DateTime), NULL)
GO
INSERT [dbo].[StateLineOfBusinesses] ([Id], [StateId], [LineOfBusinessId], [IsActive], [CreatedDate], [CreatedBy], [ModifiedDate], [ModifiedBy]) VALUES (46, 46, 1, 1, CAST(N'2015-10-16 20:19:43.953' AS DateTime), 1, CAST(N'2015-10-16 20:19:43.953' AS DateTime), NULL)
GO
INSERT [dbo].[StateLineOfBusinesses] ([Id], [StateId], [LineOfBusinessId], [IsActive], [CreatedDate], [CreatedBy], [ModifiedDate], [ModifiedBy]) VALUES (47, 47, 1, 1, CAST(N'2015-10-16 20:19:43.953' AS DateTime), 1, CAST(N'2015-10-16 20:19:43.953' AS DateTime), NULL)
GO
INSERT [dbo].[StateLineOfBusinesses] ([Id], [StateId], [LineOfBusinessId], [IsActive], [CreatedDate], [CreatedBy], [ModifiedDate], [ModifiedBy]) VALUES (48, 5, 2, 1, CAST(N'2015-10-16 20:36:32.283' AS DateTime), 1, CAST(N'2015-10-16 20:36:32.283' AS DateTime), NULL)
GO
INSERT [dbo].[StateLineOfBusinesses] ([Id], [StateId], [LineOfBusinessId], [IsActive], [CreatedDate], [CreatedBy], [ModifiedDate], [ModifiedBy]) VALUES (49, 34, 2, 1, CAST(N'2015-10-16 20:36:32.283' AS DateTime), 1, CAST(N'2015-10-16 20:36:32.283' AS DateTime), NULL)
GO
INSERT [dbo].[StateLineOfBusinesses] ([Id], [StateId], [LineOfBusinessId], [IsActive], [CreatedDate], [CreatedBy], [ModifiedDate], [ModifiedBy]) VALUES (50, 42, 2, 1, CAST(N'2015-10-16 20:36:32.283' AS DateTime), 1, CAST(N'2015-10-16 20:36:32.283' AS DateTime), NULL)
GO
INSERT [dbo].[StateLineOfBusinesses] ([Id], [StateId], [LineOfBusinessId], [IsActive], [CreatedDate], [CreatedBy], [ModifiedDate], [ModifiedBy]) VALUES (51, 48, 2, 1, CAST(N'2015-10-16 20:36:32.283' AS DateTime), 1, CAST(N'2015-10-16 20:36:32.283' AS DateTime), NULL)
GO
INSERT [dbo].[StateLineOfBusinesses] ([Id], [StateId], [LineOfBusinessId], [IsActive], [CreatedDate], [CreatedBy], [ModifiedDate], [ModifiedBy]) VALUES (52, 20, 2, 1, CAST(N'2015-10-16 20:36:32.283' AS DateTime), 1, CAST(N'2015-10-16 20:36:32.283' AS DateTime), NULL)
GO
INSERT [dbo].[StateLineOfBusinesses] ([Id], [StateId], [LineOfBusinessId], [IsActive], [CreatedDate], [CreatedBy], [ModifiedDate], [ModifiedBy]) VALUES (53, 21, 2, 1, CAST(N'2015-10-16 20:36:32.283' AS DateTime), 1, CAST(N'2015-10-16 20:36:32.283' AS DateTime), NULL)
GO
INSERT [dbo].[StateLineOfBusinesses] ([Id], [StateId], [LineOfBusinessId], [IsActive], [CreatedDate], [CreatedBy], [ModifiedDate], [ModifiedBy]) VALUES (54, 7, 2, 1, CAST(N'2015-10-16 20:36:32.283' AS DateTime), 1, CAST(N'2015-10-16 20:36:32.283' AS DateTime), NULL)
GO
INSERT [dbo].[StateLineOfBusinesses] ([Id], [StateId], [LineOfBusinessId], [IsActive], [CreatedDate], [CreatedBy], [ModifiedDate], [ModifiedBy]) VALUES (55, 30, 2, 1, CAST(N'2015-10-16 20:36:32.283' AS DateTime), 1, CAST(N'2015-10-16 20:36:32.283' AS DateTime), NULL)
GO
INSERT [dbo].[StateLineOfBusinesses] ([Id], [StateId], [LineOfBusinessId], [IsActive], [CreatedDate], [CreatedBy], [ModifiedDate], [ModifiedBy]) VALUES (56, 37, 2, 1, CAST(N'2015-10-16 20:36:32.283' AS DateTime), 1, CAST(N'2015-10-16 20:36:32.283' AS DateTime), NULL)
GO
INSERT [dbo].[StateLineOfBusinesses] ([Id], [StateId], [LineOfBusinessId], [IsActive], [CreatedDate], [CreatedBy], [ModifiedDate], [ModifiedBy]) VALUES (57, 31, 2, 1, CAST(N'2015-10-16 20:36:32.283' AS DateTime), 1, CAST(N'2015-10-16 20:36:32.283' AS DateTime), NULL)
GO
INSERT [dbo].[StateLineOfBusinesses] ([Id], [StateId], [LineOfBusinessId], [IsActive], [CreatedDate], [CreatedBy], [ModifiedDate], [ModifiedBy]) VALUES (58, 49, 2, 1, CAST(N'2015-10-16 20:36:32.283' AS DateTime), 1, CAST(N'2015-10-16 20:36:32.283' AS DateTime), NULL)
GO
INSERT [dbo].[StateLineOfBusinesses] ([Id], [StateId], [LineOfBusinessId], [IsActive], [CreatedDate], [CreatedBy], [ModifiedDate], [ModifiedBy]) VALUES (59, 10, 2, 1, CAST(N'2015-10-16 20:36:32.283' AS DateTime), 1, CAST(N'2015-10-16 20:36:32.283' AS DateTime), NULL)
GO
INSERT [dbo].[StateLineOfBusinesses] ([Id], [StateId], [LineOfBusinessId], [IsActive], [CreatedDate], [CreatedBy], [ModifiedDate], [ModifiedBy]) VALUES (60, 11, 2, 1, CAST(N'2015-10-16 20:36:32.283' AS DateTime), 1, CAST(N'2015-10-16 20:36:32.283' AS DateTime), NULL)
GO
INSERT [dbo].[StateLineOfBusinesses] ([Id], [StateId], [LineOfBusinessId], [IsActive], [CreatedDate], [CreatedBy], [ModifiedDate], [ModifiedBy]) VALUES (61, 28, 2, 1, CAST(N'2015-10-16 20:36:32.283' AS DateTime), 1, CAST(N'2015-10-16 20:36:32.283' AS DateTime), NULL)
GO
INSERT [dbo].[StateLineOfBusinesses] ([Id], [StateId], [LineOfBusinessId], [IsActive], [CreatedDate], [CreatedBy], [ModifiedDate], [ModifiedBy]) VALUES (62, 41, 2, 1, CAST(N'2015-10-16 20:36:32.283' AS DateTime), 1, CAST(N'2015-10-16 20:36:32.283' AS DateTime), NULL)
GO
INSERT [dbo].[StateLineOfBusinesses] ([Id], [StateId], [LineOfBusinessId], [IsActive], [CreatedDate], [CreatedBy], [ModifiedDate], [ModifiedBy]) VALUES (63, 44, 2, 1, CAST(N'2015-10-16 20:36:32.283' AS DateTime), 1, CAST(N'2015-10-16 20:36:32.283' AS DateTime), NULL)
GO
INSERT [dbo].[StateLineOfBusinesses] ([Id], [StateId], [LineOfBusinessId], [IsActive], [CreatedDate], [CreatedBy], [ModifiedDate], [ModifiedBy]) VALUES (64, 26, 2, 1, CAST(N'2015-10-16 20:36:32.283' AS DateTime), 1, CAST(N'2015-10-16 20:36:32.283' AS DateTime), NULL)
GO
INSERT [dbo].[StateLineOfBusinesses] ([Id], [StateId], [LineOfBusinessId], [IsActive], [CreatedDate], [CreatedBy], [ModifiedDate], [ModifiedBy]) VALUES (65, 25, 2, 1, CAST(N'2015-10-16 20:36:32.283' AS DateTime), 1, CAST(N'2015-10-16 20:36:32.283' AS DateTime), NULL)
GO
INSERT [dbo].[StateLineOfBusinesses] ([Id], [StateId], [LineOfBusinessId], [IsActive], [CreatedDate], [CreatedBy], [ModifiedDate], [ModifiedBy]) VALUES (66, 17, 2, 1, CAST(N'2015-10-16 20:36:32.283' AS DateTime), 1, CAST(N'2015-10-16 20:36:32.283' AS DateTime), NULL)
GO
INSERT [dbo].[StateLineOfBusinesses] ([Id], [StateId], [LineOfBusinessId], [IsActive], [CreatedDate], [CreatedBy], [ModifiedDate], [ModifiedBy]) VALUES (67, 6, 2, 1, CAST(N'2015-10-16 20:36:32.283' AS DateTime), 1, CAST(N'2015-10-16 20:36:32.283' AS DateTime), NULL)
GO
INSERT [dbo].[StateLineOfBusinesses] ([Id], [StateId], [LineOfBusinessId], [IsActive], [CreatedDate], [CreatedBy], [ModifiedDate], [ModifiedBy]) VALUES (68, 23, 2, 1, CAST(N'2015-10-16 20:36:32.283' AS DateTime), 1, CAST(N'2015-10-16 20:36:32.283' AS DateTime), NULL)
GO
INSERT [dbo].[StateLineOfBusinesses] ([Id], [StateId], [LineOfBusinessId], [IsActive], [CreatedDate], [CreatedBy], [ModifiedDate], [ModifiedBy]) VALUES (69, 15, 2, 1, CAST(N'2015-10-16 20:36:32.283' AS DateTime), 1, CAST(N'2015-10-16 20:36:32.283' AS DateTime), NULL)
GO
INSERT [dbo].[StateLineOfBusinesses] ([Id], [StateId], [LineOfBusinessId], [IsActive], [CreatedDate], [CreatedBy], [ModifiedDate], [ModifiedBy]) VALUES (70, 39, 2, 1, CAST(N'2015-10-16 20:36:32.283' AS DateTime), 1, CAST(N'2015-10-16 20:36:32.283' AS DateTime), NULL)
GO
INSERT [dbo].[StateLineOfBusinesses] ([Id], [StateId], [LineOfBusinessId], [IsActive], [CreatedDate], [CreatedBy], [ModifiedDate], [ModifiedBy]) VALUES (71, 4, 2, 1, CAST(N'2015-10-16 20:36:32.283' AS DateTime), 1, CAST(N'2015-10-16 20:36:32.283' AS DateTime), NULL)
GO
INSERT [dbo].[StateLineOfBusinesses] ([Id], [StateId], [LineOfBusinessId], [IsActive], [CreatedDate], [CreatedBy], [ModifiedDate], [ModifiedBy]) VALUES (72, 24, 2, 1, CAST(N'2015-10-16 20:36:32.283' AS DateTime), 1, CAST(N'2015-10-16 20:36:32.283' AS DateTime), NULL)
GO
INSERT [dbo].[StateLineOfBusinesses] ([Id], [StateId], [LineOfBusinessId], [IsActive], [CreatedDate], [CreatedBy], [ModifiedDate], [ModifiedBy]) VALUES (73, 33, 2, 1, CAST(N'2015-10-16 20:36:32.283' AS DateTime), 1, CAST(N'2015-10-16 20:36:32.283' AS DateTime), NULL)
GO
INSERT [dbo].[StateMaster] ([Id], [StateCode], [FullName], [IsActive], [CreatedDate], [CreatedBy], [ModifiedDate], [ModifiedBy]) VALUES (1, N'AK', NULL, 1, CAST(N'2015-10-15 00:00:00.000' AS DateTime), 1, CAST(N'2015-10-15 00:00:00.000' AS DateTime), NULL)
GO
INSERT [dbo].[StateMaster] ([Id], [StateCode], [FullName], [IsActive], [CreatedDate], [CreatedBy], [ModifiedDate], [ModifiedBy]) VALUES (2, N'AL', NULL, 1, CAST(N'2015-10-15 00:00:00.000' AS DateTime), 1, CAST(N'2015-10-15 00:00:00.000' AS DateTime), NULL)
GO
INSERT [dbo].[StateMaster] ([Id], [StateCode], [FullName], [IsActive], [CreatedDate], [CreatedBy], [ModifiedDate], [ModifiedBy]) VALUES (3, N'AR', NULL, 1, CAST(N'2015-10-15 00:00:00.000' AS DateTime), 1, CAST(N'2015-10-15 00:00:00.000' AS DateTime), NULL)
GO
INSERT [dbo].[StateMaster] ([Id], [StateCode], [FullName], [IsActive], [CreatedDate], [CreatedBy], [ModifiedDate], [ModifiedBy]) VALUES (4, N'AZ', NULL, 1, CAST(N'2015-10-15 00:00:00.000' AS DateTime), 1, CAST(N'2015-10-15 00:00:00.000' AS DateTime), NULL)
GO
INSERT [dbo].[StateMaster] ([Id], [StateCode], [FullName], [IsActive], [CreatedDate], [CreatedBy], [ModifiedDate], [ModifiedBy]) VALUES (5, N'CA', NULL, 1, CAST(N'2015-10-15 00:00:00.000' AS DateTime), 1, CAST(N'2015-10-15 00:00:00.000' AS DateTime), NULL)
GO
INSERT [dbo].[StateMaster] ([Id], [StateCode], [FullName], [IsActive], [CreatedDate], [CreatedBy], [ModifiedDate], [ModifiedBy]) VALUES (6, N'CO', NULL, 1, CAST(N'2015-10-15 00:00:00.000' AS DateTime), 1, CAST(N'2015-10-15 00:00:00.000' AS DateTime), NULL)
GO
INSERT [dbo].[StateMaster] ([Id], [StateCode], [FullName], [IsActive], [CreatedDate], [CreatedBy], [ModifiedDate], [ModifiedBy]) VALUES (7, N'CT', NULL, 1, CAST(N'2015-10-15 00:00:00.000' AS DateTime), 1, CAST(N'2015-10-15 00:00:00.000' AS DateTime), NULL)
GO
INSERT [dbo].[StateMaster] ([Id], [StateCode], [FullName], [IsActive], [CreatedDate], [CreatedBy], [ModifiedDate], [ModifiedBy]) VALUES (8, N'DC', NULL, 1, CAST(N'2015-10-15 00:00:00.000' AS DateTime), 1, CAST(N'2015-10-15 00:00:00.000' AS DateTime), NULL)
GO
INSERT [dbo].[StateMaster] ([Id], [StateCode], [FullName], [IsActive], [CreatedDate], [CreatedBy], [ModifiedDate], [ModifiedBy]) VALUES (9, N'DE', NULL, 1, CAST(N'2015-10-15 00:00:00.000' AS DateTime), 1, CAST(N'2015-10-15 00:00:00.000' AS DateTime), NULL)
GO
INSERT [dbo].[StateMaster] ([Id], [StateCode], [FullName], [IsActive], [CreatedDate], [CreatedBy], [ModifiedDate], [ModifiedBy]) VALUES (10, N'FL', NULL, 1, CAST(N'2015-10-15 00:00:00.000' AS DateTime), 1, CAST(N'2015-10-15 00:00:00.000' AS DateTime), NULL)
GO
INSERT [dbo].[StateMaster] ([Id], [StateCode], [FullName], [IsActive], [CreatedDate], [CreatedBy], [ModifiedDate], [ModifiedBy]) VALUES (11, N'GA', NULL, 1, CAST(N'2015-10-15 00:00:00.000' AS DateTime), 1, CAST(N'2015-10-15 00:00:00.000' AS DateTime), NULL)
GO
INSERT [dbo].[StateMaster] ([Id], [StateCode], [FullName], [IsActive], [CreatedDate], [CreatedBy], [ModifiedDate], [ModifiedBy]) VALUES (12, N'HI', NULL, 1, CAST(N'2015-10-15 00:00:00.000' AS DateTime), 1, CAST(N'2015-10-15 00:00:00.000' AS DateTime), NULL)
GO
INSERT [dbo].[StateMaster] ([Id], [StateCode], [FullName], [IsActive], [CreatedDate], [CreatedBy], [ModifiedDate], [ModifiedBy]) VALUES (13, N'IA', NULL, 1, CAST(N'2015-10-15 00:00:00.000' AS DateTime), 1, CAST(N'2015-10-15 00:00:00.000' AS DateTime), NULL)
GO
INSERT [dbo].[StateMaster] ([Id], [StateCode], [FullName], [IsActive], [CreatedDate], [CreatedBy], [ModifiedDate], [ModifiedBy]) VALUES (14, N'ID', NULL, 1, CAST(N'2015-10-15 00:00:00.000' AS DateTime), 1, CAST(N'2015-10-15 00:00:00.000' AS DateTime), NULL)
GO
INSERT [dbo].[StateMaster] ([Id], [StateCode], [FullName], [IsActive], [CreatedDate], [CreatedBy], [ModifiedDate], [ModifiedBy]) VALUES (15, N'IL', NULL, 1, CAST(N'2015-10-15 00:00:00.000' AS DateTime), 1, CAST(N'2015-10-15 00:00:00.000' AS DateTime), NULL)
GO
INSERT [dbo].[StateMaster] ([Id], [StateCode], [FullName], [IsActive], [CreatedDate], [CreatedBy], [ModifiedDate], [ModifiedBy]) VALUES (16, N'IN', NULL, 1, CAST(N'2015-10-15 00:00:00.000' AS DateTime), 1, CAST(N'2015-10-15 00:00:00.000' AS DateTime), NULL)
GO
INSERT [dbo].[StateMaster] ([Id], [StateCode], [FullName], [IsActive], [CreatedDate], [CreatedBy], [ModifiedDate], [ModifiedBy]) VALUES (17, N'KS', NULL, 1, CAST(N'2015-10-15 00:00:00.000' AS DateTime), 1, CAST(N'2015-10-15 00:00:00.000' AS DateTime), NULL)
GO
INSERT [dbo].[StateMaster] ([Id], [StateCode], [FullName], [IsActive], [CreatedDate], [CreatedBy], [ModifiedDate], [ModifiedBy]) VALUES (18, N'KY', NULL, 1, CAST(N'2015-10-15 00:00:00.000' AS DateTime), 1, CAST(N'2015-10-15 00:00:00.000' AS DateTime), NULL)
GO
INSERT [dbo].[StateMaster] ([Id], [StateCode], [FullName], [IsActive], [CreatedDate], [CreatedBy], [ModifiedDate], [ModifiedBy]) VALUES (19, N'LA', NULL, 1, CAST(N'2015-10-15 00:00:00.000' AS DateTime), 1, CAST(N'2015-10-15 00:00:00.000' AS DateTime), NULL)
GO
INSERT [dbo].[StateMaster] ([Id], [StateCode], [FullName], [IsActive], [CreatedDate], [CreatedBy], [ModifiedDate], [ModifiedBy]) VALUES (20, N'MA', NULL, 1, CAST(N'2015-10-15 00:00:00.000' AS DateTime), 1, CAST(N'2015-10-15 00:00:00.000' AS DateTime), NULL)
GO
INSERT [dbo].[StateMaster] ([Id], [StateCode], [FullName], [IsActive], [CreatedDate], [CreatedBy], [ModifiedDate], [ModifiedBy]) VALUES (21, N'MD', NULL, 1, CAST(N'2015-10-15 00:00:00.000' AS DateTime), 1, CAST(N'2015-10-15 00:00:00.000' AS DateTime), NULL)
GO
INSERT [dbo].[StateMaster] ([Id], [StateCode], [FullName], [IsActive], [CreatedDate], [CreatedBy], [ModifiedDate], [ModifiedBy]) VALUES (22, N'ME', NULL, 1, CAST(N'2015-10-15 00:00:00.000' AS DateTime), 1, CAST(N'2015-10-15 00:00:00.000' AS DateTime), NULL)
GO
INSERT [dbo].[StateMaster] ([Id], [StateCode], [FullName], [IsActive], [CreatedDate], [CreatedBy], [ModifiedDate], [ModifiedBy]) VALUES (23, N'MI', NULL, 1, CAST(N'2015-10-15 00:00:00.000' AS DateTime), 1, CAST(N'2015-10-15 00:00:00.000' AS DateTime), NULL)
GO
INSERT [dbo].[StateMaster] ([Id], [StateCode], [FullName], [IsActive], [CreatedDate], [CreatedBy], [ModifiedDate], [ModifiedBy]) VALUES (24, N'MN', NULL, 1, CAST(N'2015-10-15 00:00:00.000' AS DateTime), 1, CAST(N'2015-10-15 00:00:00.000' AS DateTime), NULL)
GO
INSERT [dbo].[StateMaster] ([Id], [StateCode], [FullName], [IsActive], [CreatedDate], [CreatedBy], [ModifiedDate], [ModifiedBy]) VALUES (25, N'MO', NULL, 1, CAST(N'2015-10-15 00:00:00.000' AS DateTime), 1, CAST(N'2015-10-15 00:00:00.000' AS DateTime), NULL)
GO
INSERT [dbo].[StateMaster] ([Id], [StateCode], [FullName], [IsActive], [CreatedDate], [CreatedBy], [ModifiedDate], [ModifiedBy]) VALUES (26, N'MS', NULL, 1, CAST(N'2015-10-15 00:00:00.000' AS DateTime), 1, CAST(N'2015-10-15 00:00:00.000' AS DateTime), NULL)
GO
INSERT [dbo].[StateMaster] ([Id], [StateCode], [FullName], [IsActive], [CreatedDate], [CreatedBy], [ModifiedDate], [ModifiedBy]) VALUES (27, N'MT', NULL, 1, CAST(N'2015-10-15 00:00:00.000' AS DateTime), 1, CAST(N'2015-10-15 00:00:00.000' AS DateTime), NULL)
GO
INSERT [dbo].[StateMaster] ([Id], [StateCode], [FullName], [IsActive], [CreatedDate], [CreatedBy], [ModifiedDate], [ModifiedBy]) VALUES (28, N'NC', NULL, 1, CAST(N'2015-10-15 00:00:00.000' AS DateTime), 1, CAST(N'2015-10-15 00:00:00.000' AS DateTime), NULL)
GO
INSERT [dbo].[StateMaster] ([Id], [StateCode], [FullName], [IsActive], [CreatedDate], [CreatedBy], [ModifiedDate], [ModifiedBy]) VALUES (29, N'NE', NULL, 1, CAST(N'2015-10-15 00:00:00.000' AS DateTime), 1, CAST(N'2015-10-15 00:00:00.000' AS DateTime), NULL)
GO
INSERT [dbo].[StateMaster] ([Id], [StateCode], [FullName], [IsActive], [CreatedDate], [CreatedBy], [ModifiedDate], [ModifiedBy]) VALUES (30, N'NH', NULL, 1, CAST(N'2015-10-15 00:00:00.000' AS DateTime), 1, CAST(N'2015-10-15 00:00:00.000' AS DateTime), NULL)
GO
INSERT [dbo].[StateMaster] ([Id], [StateCode], [FullName], [IsActive], [CreatedDate], [CreatedBy], [ModifiedDate], [ModifiedBy]) VALUES (31, N'NJ', NULL, 1, CAST(N'2015-10-15 00:00:00.000' AS DateTime), 1, CAST(N'2015-10-15 00:00:00.000' AS DateTime), NULL)
GO
INSERT [dbo].[StateMaster] ([Id], [StateCode], [FullName], [IsActive], [CreatedDate], [CreatedBy], [ModifiedDate], [ModifiedBy]) VALUES (32, N'NM', NULL, 1, CAST(N'2015-10-15 00:00:00.000' AS DateTime), 1, CAST(N'2015-10-15 00:00:00.000' AS DateTime), NULL)
GO
INSERT [dbo].[StateMaster] ([Id], [StateCode], [FullName], [IsActive], [CreatedDate], [CreatedBy], [ModifiedDate], [ModifiedBy]) VALUES (33, N'NV', NULL, 1, CAST(N'2015-10-15 00:00:00.000' AS DateTime), 1, CAST(N'2015-10-15 00:00:00.000' AS DateTime), NULL)
GO
INSERT [dbo].[StateMaster] ([Id], [StateCode], [FullName], [IsActive], [CreatedDate], [CreatedBy], [ModifiedDate], [ModifiedBy]) VALUES (34, N'NY', NULL, 1, CAST(N'2015-10-15 00:00:00.000' AS DateTime), 1, CAST(N'2015-10-15 00:00:00.000' AS DateTime), NULL)
GO
INSERT [dbo].[StateMaster] ([Id], [StateCode], [FullName], [IsActive], [CreatedDate], [CreatedBy], [ModifiedDate], [ModifiedBy]) VALUES (35, N'OK', NULL, 1, CAST(N'2015-10-15 00:00:00.000' AS DateTime), 1, CAST(N'2015-10-15 00:00:00.000' AS DateTime), NULL)
GO
INSERT [dbo].[StateMaster] ([Id], [StateCode], [FullName], [IsActive], [CreatedDate], [CreatedBy], [ModifiedDate], [ModifiedBy]) VALUES (36, N'OR', NULL, 1, CAST(N'2015-10-15 00:00:00.000' AS DateTime), 1, CAST(N'2015-10-15 00:00:00.000' AS DateTime), NULL)
GO
INSERT [dbo].[StateMaster] ([Id], [StateCode], [FullName], [IsActive], [CreatedDate], [CreatedBy], [ModifiedDate], [ModifiedBy]) VALUES (37, N'PA', NULL, 1, CAST(N'2015-10-15 00:00:00.000' AS DateTime), 1, CAST(N'2015-10-15 00:00:00.000' AS DateTime), NULL)
GO
INSERT [dbo].[StateMaster] ([Id], [StateCode], [FullName], [IsActive], [CreatedDate], [CreatedBy], [ModifiedDate], [ModifiedBy]) VALUES (38, N'RI', NULL, 1, CAST(N'2015-10-15 00:00:00.000' AS DateTime), 1, CAST(N'2015-10-15 00:00:00.000' AS DateTime), NULL)
GO
INSERT [dbo].[StateMaster] ([Id], [StateCode], [FullName], [IsActive], [CreatedDate], [CreatedBy], [ModifiedDate], [ModifiedBy]) VALUES (39, N'SC', NULL, 1, CAST(N'2015-10-15 00:00:00.000' AS DateTime), 1, CAST(N'2015-10-15 00:00:00.000' AS DateTime), NULL)
GO
INSERT [dbo].[StateMaster] ([Id], [StateCode], [FullName], [IsActive], [CreatedDate], [CreatedBy], [ModifiedDate], [ModifiedBy]) VALUES (40, N'SD', NULL, 1, CAST(N'2015-10-15 00:00:00.000' AS DateTime), 1, CAST(N'2015-10-15 00:00:00.000' AS DateTime), NULL)
GO
INSERT [dbo].[StateMaster] ([Id], [StateCode], [FullName], [IsActive], [CreatedDate], [CreatedBy], [ModifiedDate], [ModifiedBy]) VALUES (41, N'TN', NULL, 1, CAST(N'2015-10-15 00:00:00.000' AS DateTime), 1, CAST(N'2015-10-15 00:00:00.000' AS DateTime), NULL)
GO
INSERT [dbo].[StateMaster] ([Id], [StateCode], [FullName], [IsActive], [CreatedDate], [CreatedBy], [ModifiedDate], [ModifiedBy]) VALUES (42, N'TX', NULL, 1, CAST(N'2015-10-15 00:00:00.000' AS DateTime), 1, CAST(N'2015-10-15 00:00:00.000' AS DateTime), NULL)
GO
INSERT [dbo].[StateMaster] ([Id], [StateCode], [FullName], [IsActive], [CreatedDate], [CreatedBy], [ModifiedDate], [ModifiedBy]) VALUES (43, N'UT', NULL, 1, CAST(N'2015-10-15 00:00:00.000' AS DateTime), 1, CAST(N'2015-10-15 00:00:00.000' AS DateTime), NULL)
GO
INSERT [dbo].[StateMaster] ([Id], [StateCode], [FullName], [IsActive], [CreatedDate], [CreatedBy], [ModifiedDate], [ModifiedBy]) VALUES (44, N'VA', NULL, 1, CAST(N'2015-10-15 00:00:00.000' AS DateTime), 1, CAST(N'2015-10-15 00:00:00.000' AS DateTime), NULL)
GO
INSERT [dbo].[StateMaster] ([Id], [StateCode], [FullName], [IsActive], [CreatedDate], [CreatedBy], [ModifiedDate], [ModifiedBy]) VALUES (45, N'VT', NULL, 1, CAST(N'2015-10-15 00:00:00.000' AS DateTime), 1, CAST(N'2015-10-15 00:00:00.000' AS DateTime), NULL)
GO
INSERT [dbo].[StateMaster] ([Id], [StateCode], [FullName], [IsActive], [CreatedDate], [CreatedBy], [ModifiedDate], [ModifiedBy]) VALUES (46, N'WI', NULL, 1, CAST(N'2015-10-15 00:00:00.000' AS DateTime), 1, CAST(N'2015-10-15 00:00:00.000' AS DateTime), NULL)
GO
INSERT [dbo].[StateMaster] ([Id], [StateCode], [FullName], [IsActive], [CreatedDate], [CreatedBy], [ModifiedDate], [ModifiedBy]) VALUES (47, N'WV', NULL, 1, CAST(N'2015-10-15 00:00:00.000' AS DateTime), 1, CAST(N'2015-10-15 00:00:00.000' AS DateTime), NULL)
GO
INSERT [dbo].[StateMaster] ([Id], [StateCode], [FullName], [IsActive], [CreatedDate], [CreatedBy], [ModifiedDate], [ModifiedBy]) VALUES (48, N'WA', NULL, 1, CAST(N'2015-10-16 20:31:15.643' AS DateTime), 1, CAST(N'2015-10-16 20:31:15.643' AS DateTime), NULL)
GO
INSERT [dbo].[StateMaster] ([Id], [StateCode], [FullName], [IsActive], [CreatedDate], [CreatedBy], [ModifiedDate], [ModifiedBy]) VALUES (49, N'OH', NULL, 1, CAST(N'2015-10-16 20:31:40.313' AS DateTime), 1, CAST(N'2015-10-16 20:31:40.313' AS DateTime), NULL)
GO
ALTER TABLE [dbo].[StateLineOfBusinesses]  WITH CHECK ADD  CONSTRAINT [FK_StateLineOfBusinesses_LineOfBusiness] FOREIGN KEY([LineOfBusinessId])
REFERENCES [dbo].[LineOfBusiness] ([Id])
GO
ALTER TABLE [dbo].[StateLineOfBusinesses] CHECK CONSTRAINT [FK_StateLineOfBusinesses_LineOfBusiness]
GO
ALTER TABLE [dbo].[StateLineOfBusinesses]  WITH CHECK ADD  CONSTRAINT [FK_StateLineOfBusinesses_StateMaster] FOREIGN KEY([StateId])
REFERENCES [dbo].[StateMaster] ([Id])
GO
ALTER TABLE [dbo].[StateLineOfBusinesses] CHECK CONSTRAINT [FK_StateLineOfBusinesses_StateMaster]
GO
