
/****** Object:  Table [dbo].[TypeOfState]    Script Date: 13-10-2015 PM 03:08:06 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[TypeOfState](
	[id] [int] NOT NULL,
	[StateCode] [char](2) NOT NULL,
	[IsGoodState] [bit] NOT NULL,
	[EffectiveDate] [datetime] NULL,
	[ExpiryDate] [datetime] NULL,
	[IsActive] [bit] NULL,
	[CreatedDate] [datetime] NULL CONSTRAINT [DF_TypeOfState_CreatedDate]  DEFAULT (getdate()),
	[CreatedBy] [int] NULL,
	[ModifiedDate] [datetime] NULL CONSTRAINT [DF_TypeOfState_ModifiedDate]  DEFAULT (getdate()),
	[ModifiedBy] [int] NULL,
 CONSTRAINT [PK_TypeOfState] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
INSERT [dbo].[TypeOfState] ([id], [StateCode], [IsGoodState], [EffectiveDate], [ExpiryDate], [IsActive], [CreatedDate], [CreatedBy], [ModifiedDate], [ModifiedBy]) VALUES (1, N'AK', 0, CAST(N'2015-10-12 00:00:00.000' AS DateTime), CAST(N'2099-10-12 00:00:00.000' AS DateTime), 1, CAST(N'2015-10-12 00:00:00.000' AS DateTime), 1, CAST(N'2015-10-12 00:00:00.000' AS DateTime), NULL)
GO
INSERT [dbo].[TypeOfState] ([id], [StateCode], [IsGoodState], [EffectiveDate], [ExpiryDate], [IsActive], [CreatedDate], [CreatedBy], [ModifiedDate], [ModifiedBy]) VALUES (2, N'AL', 1, CAST(N'2015-10-12 00:00:00.000' AS DateTime), CAST(N'2099-10-12 00:00:00.000' AS DateTime), 1, CAST(N'2015-10-12 00:00:00.000' AS DateTime), 1, CAST(N'2015-10-12 00:00:00.000' AS DateTime), NULL)
GO
INSERT [dbo].[TypeOfState] ([id], [StateCode], [IsGoodState], [EffectiveDate], [ExpiryDate], [IsActive], [CreatedDate], [CreatedBy], [ModifiedDate], [ModifiedBy]) VALUES (3, N'AR', 1, CAST(N'2015-10-12 00:00:00.000' AS DateTime), CAST(N'2099-10-12 00:00:00.000' AS DateTime), 1, CAST(N'2015-10-12 00:00:00.000' AS DateTime), 1, CAST(N'2015-10-12 00:00:00.000' AS DateTime), NULL)
GO
INSERT [dbo].[TypeOfState] ([id], [StateCode], [IsGoodState], [EffectiveDate], [ExpiryDate], [IsActive], [CreatedDate], [CreatedBy], [ModifiedDate], [ModifiedBy]) VALUES (4, N'AZ', 0, CAST(N'2015-10-12 00:00:00.000' AS DateTime), CAST(N'2099-10-12 00:00:00.000' AS DateTime), 1, CAST(N'2015-10-12 00:00:00.000' AS DateTime), 1, CAST(N'2015-10-12 00:00:00.000' AS DateTime), NULL)
GO
INSERT [dbo].[TypeOfState] ([id], [StateCode], [IsGoodState], [EffectiveDate], [ExpiryDate], [IsActive], [CreatedDate], [CreatedBy], [ModifiedDate], [ModifiedBy]) VALUES (5, N'CA', 1, CAST(N'2015-10-12 00:00:00.000' AS DateTime), CAST(N'2099-10-12 00:00:00.000' AS DateTime), 1, CAST(N'2015-10-12 00:00:00.000' AS DateTime), 1, CAST(N'2015-10-12 00:00:00.000' AS DateTime), NULL)
GO
INSERT [dbo].[TypeOfState] ([id], [StateCode], [IsGoodState], [EffectiveDate], [ExpiryDate], [IsActive], [CreatedDate], [CreatedBy], [ModifiedDate], [ModifiedBy]) VALUES (6, N'CO', 1, CAST(N'2015-10-12 00:00:00.000' AS DateTime), CAST(N'2099-10-12 00:00:00.000' AS DateTime), 1, CAST(N'2015-10-12 00:00:00.000' AS DateTime), 1, CAST(N'2015-10-12 00:00:00.000' AS DateTime), NULL)
GO
INSERT [dbo].[TypeOfState] ([id], [StateCode], [IsGoodState], [EffectiveDate], [ExpiryDate], [IsActive], [CreatedDate], [CreatedBy], [ModifiedDate], [ModifiedBy]) VALUES (7, N'CT', 0, CAST(N'2015-10-12 00:00:00.000' AS DateTime), CAST(N'2099-10-12 00:00:00.000' AS DateTime), 1, CAST(N'2015-10-12 00:00:00.000' AS DateTime), 1, CAST(N'2015-10-12 00:00:00.000' AS DateTime), NULL)
GO
INSERT [dbo].[TypeOfState] ([id], [StateCode], [IsGoodState], [EffectiveDate], [ExpiryDate], [IsActive], [CreatedDate], [CreatedBy], [ModifiedDate], [ModifiedBy]) VALUES (8, N'DC', 1, CAST(N'2015-10-12 00:00:00.000' AS DateTime), CAST(N'2099-10-12 00:00:00.000' AS DateTime), 1, CAST(N'2015-10-12 00:00:00.000' AS DateTime), 1, CAST(N'2015-10-12 00:00:00.000' AS DateTime), NULL)
GO
INSERT [dbo].[TypeOfState] ([id], [StateCode], [IsGoodState], [EffectiveDate], [ExpiryDate], [IsActive], [CreatedDate], [CreatedBy], [ModifiedDate], [ModifiedBy]) VALUES (9, N'DE', 1, CAST(N'2015-10-12 00:00:00.000' AS DateTime), CAST(N'2099-10-12 00:00:00.000' AS DateTime), 1, CAST(N'2015-10-12 00:00:00.000' AS DateTime), 1, CAST(N'2015-10-12 00:00:00.000' AS DateTime), NULL)
GO
INSERT [dbo].[TypeOfState] ([id], [StateCode], [IsGoodState], [EffectiveDate], [ExpiryDate], [IsActive], [CreatedDate], [CreatedBy], [ModifiedDate], [ModifiedBy]) VALUES (10, N'FL', 0, CAST(N'2015-10-12 00:00:00.000' AS DateTime), CAST(N'2099-10-12 00:00:00.000' AS DateTime), 1, CAST(N'2015-10-12 00:00:00.000' AS DateTime), 1, CAST(N'2015-10-12 00:00:00.000' AS DateTime), NULL)
GO
INSERT [dbo].[TypeOfState] ([id], [StateCode], [IsGoodState], [EffectiveDate], [ExpiryDate], [IsActive], [CreatedDate], [CreatedBy], [ModifiedDate], [ModifiedBy]) VALUES (11, N'GA', 1, CAST(N'2015-10-12 00:00:00.000' AS DateTime), CAST(N'2099-10-12 00:00:00.000' AS DateTime), 1, CAST(N'2015-10-12 00:00:00.000' AS DateTime), 1, CAST(N'2015-10-12 00:00:00.000' AS DateTime), NULL)
GO
INSERT [dbo].[TypeOfState] ([id], [StateCode], [IsGoodState], [EffectiveDate], [ExpiryDate], [IsActive], [CreatedDate], [CreatedBy], [ModifiedDate], [ModifiedBy]) VALUES (12, N'HI', 1, CAST(N'2015-10-12 00:00:00.000' AS DateTime), CAST(N'2099-10-12 00:00:00.000' AS DateTime), 1, CAST(N'2015-10-12 00:00:00.000' AS DateTime), 1, CAST(N'2015-10-12 00:00:00.000' AS DateTime), NULL)
GO
INSERT [dbo].[TypeOfState] ([id], [StateCode], [IsGoodState], [EffectiveDate], [ExpiryDate], [IsActive], [CreatedDate], [CreatedBy], [ModifiedDate], [ModifiedBy]) VALUES (13, N'IA', 0, CAST(N'2015-10-12 00:00:00.000' AS DateTime), CAST(N'2099-10-12 00:00:00.000' AS DateTime), 1, CAST(N'2015-10-12 00:00:00.000' AS DateTime), 1, CAST(N'2015-10-12 00:00:00.000' AS DateTime), NULL)
GO
INSERT [dbo].[TypeOfState] ([id], [StateCode], [IsGoodState], [EffectiveDate], [ExpiryDate], [IsActive], [CreatedDate], [CreatedBy], [ModifiedDate], [ModifiedBy]) VALUES (14, N'ID', 0, CAST(N'2015-10-12 00:00:00.000' AS DateTime), CAST(N'2099-10-12 00:00:00.000' AS DateTime), 1, CAST(N'2015-10-12 00:00:00.000' AS DateTime), 1, CAST(N'2015-10-12 00:00:00.000' AS DateTime), NULL)
GO
INSERT [dbo].[TypeOfState] ([id], [StateCode], [IsGoodState], [EffectiveDate], [ExpiryDate], [IsActive], [CreatedDate], [CreatedBy], [ModifiedDate], [ModifiedBy]) VALUES (15, N'IL', 0, CAST(N'2015-10-12 00:00:00.000' AS DateTime), CAST(N'2099-10-12 00:00:00.000' AS DateTime), 1, CAST(N'2015-10-12 00:00:00.000' AS DateTime), 1, CAST(N'2015-10-12 00:00:00.000' AS DateTime), NULL)
GO
INSERT [dbo].[TypeOfState] ([id], [StateCode], [IsGoodState], [EffectiveDate], [ExpiryDate], [IsActive], [CreatedDate], [CreatedBy], [ModifiedDate], [ModifiedBy]) VALUES (16, N'IN', 0, CAST(N'2015-10-12 00:00:00.000' AS DateTime), CAST(N'2099-10-12 00:00:00.000' AS DateTime), 1, CAST(N'2015-10-12 00:00:00.000' AS DateTime), 1, CAST(N'2015-10-12 00:00:00.000' AS DateTime), NULL)
GO
INSERT [dbo].[TypeOfState] ([id], [StateCode], [IsGoodState], [EffectiveDate], [ExpiryDate], [IsActive], [CreatedDate], [CreatedBy], [ModifiedDate], [ModifiedBy]) VALUES (17, N'KS', 1, CAST(N'2015-10-12 00:00:00.000' AS DateTime), CAST(N'2099-10-12 00:00:00.000' AS DateTime), 1, CAST(N'2015-10-12 00:00:00.000' AS DateTime), 1, CAST(N'2015-10-12 00:00:00.000' AS DateTime), NULL)
GO
INSERT [dbo].[TypeOfState] ([id], [StateCode], [IsGoodState], [EffectiveDate], [ExpiryDate], [IsActive], [CreatedDate], [CreatedBy], [ModifiedDate], [ModifiedBy]) VALUES (18, N'KY', 1, CAST(N'2015-10-12 00:00:00.000' AS DateTime), CAST(N'2099-10-12 00:00:00.000' AS DateTime), 1, CAST(N'2015-10-12 00:00:00.000' AS DateTime), 1, CAST(N'2015-10-12 00:00:00.000' AS DateTime), NULL)
GO
INSERT [dbo].[TypeOfState] ([id], [StateCode], [IsGoodState], [EffectiveDate], [ExpiryDate], [IsActive], [CreatedDate], [CreatedBy], [ModifiedDate], [ModifiedBy]) VALUES (19, N'LA', 1, CAST(N'2015-10-12 00:00:00.000' AS DateTime), CAST(N'2099-10-12 00:00:00.000' AS DateTime), 1, CAST(N'2015-10-12 00:00:00.000' AS DateTime), 1, CAST(N'2015-10-12 00:00:00.000' AS DateTime), NULL)
GO
INSERT [dbo].[TypeOfState] ([id], [StateCode], [IsGoodState], [EffectiveDate], [ExpiryDate], [IsActive], [CreatedDate], [CreatedBy], [ModifiedDate], [ModifiedBy]) VALUES (20, N'MA', 0, CAST(N'2015-10-12 00:00:00.000' AS DateTime), CAST(N'2099-10-12 00:00:00.000' AS DateTime), 1, CAST(N'2015-10-12 00:00:00.000' AS DateTime), 1, CAST(N'2015-10-12 00:00:00.000' AS DateTime), NULL)
GO
INSERT [dbo].[TypeOfState] ([id], [StateCode], [IsGoodState], [EffectiveDate], [ExpiryDate], [IsActive], [CreatedDate], [CreatedBy], [ModifiedDate], [ModifiedBy]) VALUES (21, N'MD', 0, CAST(N'2015-10-12 00:00:00.000' AS DateTime), CAST(N'2099-10-12 00:00:00.000' AS DateTime), 1, CAST(N'2015-10-12 00:00:00.000' AS DateTime), 1, CAST(N'2015-10-12 00:00:00.000' AS DateTime), NULL)
GO
INSERT [dbo].[TypeOfState] ([id], [StateCode], [IsGoodState], [EffectiveDate], [ExpiryDate], [IsActive], [CreatedDate], [CreatedBy], [ModifiedDate], [ModifiedBy]) VALUES (22, N'ME', 0, CAST(N'2015-10-12 00:00:00.000' AS DateTime), CAST(N'2099-10-12 00:00:00.000' AS DateTime), 1, CAST(N'2015-10-12 00:00:00.000' AS DateTime), 1, CAST(N'2015-10-12 00:00:00.000' AS DateTime), NULL)
GO
INSERT [dbo].[TypeOfState] ([id], [StateCode], [IsGoodState], [EffectiveDate], [ExpiryDate], [IsActive], [CreatedDate], [CreatedBy], [ModifiedDate], [ModifiedBy]) VALUES (23, N'MI', 1, CAST(N'2015-10-12 00:00:00.000' AS DateTime), CAST(N'2099-10-12 00:00:00.000' AS DateTime), 1, CAST(N'2015-10-12 00:00:00.000' AS DateTime), 1, CAST(N'2015-10-12 00:00:00.000' AS DateTime), NULL)
GO
INSERT [dbo].[TypeOfState] ([id], [StateCode], [IsGoodState], [EffectiveDate], [ExpiryDate], [IsActive], [CreatedDate], [CreatedBy], [ModifiedDate], [ModifiedBy]) VALUES (24, N'MN', 1, CAST(N'2015-10-12 00:00:00.000' AS DateTime), CAST(N'2099-10-12 00:00:00.000' AS DateTime), 1, CAST(N'2015-10-12 00:00:00.000' AS DateTime), 1, CAST(N'2015-10-12 00:00:00.000' AS DateTime), NULL)
GO
INSERT [dbo].[TypeOfState] ([id], [StateCode], [IsGoodState], [EffectiveDate], [ExpiryDate], [IsActive], [CreatedDate], [CreatedBy], [ModifiedDate], [ModifiedBy]) VALUES (25, N'MO', 1, CAST(N'2015-10-12 00:00:00.000' AS DateTime), CAST(N'2099-10-12 00:00:00.000' AS DateTime), 1, CAST(N'2015-10-12 00:00:00.000' AS DateTime), 1, CAST(N'2015-10-12 00:00:00.000' AS DateTime), NULL)
GO
INSERT [dbo].[TypeOfState] ([id], [StateCode], [IsGoodState], [EffectiveDate], [ExpiryDate], [IsActive], [CreatedDate], [CreatedBy], [ModifiedDate], [ModifiedBy]) VALUES (26, N'MS', 1, CAST(N'2015-10-12 00:00:00.000' AS DateTime), CAST(N'2099-10-12 00:00:00.000' AS DateTime), 1, CAST(N'2015-10-12 00:00:00.000' AS DateTime), 1, CAST(N'2015-10-12 00:00:00.000' AS DateTime), NULL)
GO
INSERT [dbo].[TypeOfState] ([id], [StateCode], [IsGoodState], [EffectiveDate], [ExpiryDate], [IsActive], [CreatedDate], [CreatedBy], [ModifiedDate], [ModifiedBy]) VALUES (27, N'MT', 1, CAST(N'2015-10-12 00:00:00.000' AS DateTime), CAST(N'2099-10-12 00:00:00.000' AS DateTime), 1, CAST(N'2015-10-12 00:00:00.000' AS DateTime), 1, CAST(N'2015-10-12 00:00:00.000' AS DateTime), NULL)
GO
INSERT [dbo].[TypeOfState] ([id], [StateCode], [IsGoodState], [EffectiveDate], [ExpiryDate], [IsActive], [CreatedDate], [CreatedBy], [ModifiedDate], [ModifiedBy]) VALUES (28, N'NC', 1, CAST(N'2015-10-12 00:00:00.000' AS DateTime), CAST(N'2099-10-12 00:00:00.000' AS DateTime), 1, CAST(N'2015-10-12 00:00:00.000' AS DateTime), 1, CAST(N'2015-10-12 00:00:00.000' AS DateTime), NULL)
GO
INSERT [dbo].[TypeOfState] ([id], [StateCode], [IsGoodState], [EffectiveDate], [ExpiryDate], [IsActive], [CreatedDate], [CreatedBy], [ModifiedDate], [ModifiedBy]) VALUES (29, N'NE', 1, CAST(N'2015-10-12 00:00:00.000' AS DateTime), CAST(N'2099-10-12 00:00:00.000' AS DateTime), 1, CAST(N'2015-10-12 00:00:00.000' AS DateTime), 1, CAST(N'2015-10-12 00:00:00.000' AS DateTime), NULL)
GO
INSERT [dbo].[TypeOfState] ([id], [StateCode], [IsGoodState], [EffectiveDate], [ExpiryDate], [IsActive], [CreatedDate], [CreatedBy], [ModifiedDate], [ModifiedBy]) VALUES (30, N'NH', 1, CAST(N'2015-10-12 00:00:00.000' AS DateTime), CAST(N'2099-10-12 00:00:00.000' AS DateTime), 1, CAST(N'2015-10-12 00:00:00.000' AS DateTime), 1, CAST(N'2015-10-12 00:00:00.000' AS DateTime), NULL)
GO
INSERT [dbo].[TypeOfState] ([id], [StateCode], [IsGoodState], [EffectiveDate], [ExpiryDate], [IsActive], [CreatedDate], [CreatedBy], [ModifiedDate], [ModifiedBy]) VALUES (31, N'NJ', 0, CAST(N'2015-10-12 00:00:00.000' AS DateTime), CAST(N'2099-10-12 00:00:00.000' AS DateTime), 1, CAST(N'2015-10-12 00:00:00.000' AS DateTime), 1, CAST(N'2015-10-12 00:00:00.000' AS DateTime), NULL)
GO
INSERT [dbo].[TypeOfState] ([id], [StateCode], [IsGoodState], [EffectiveDate], [ExpiryDate], [IsActive], [CreatedDate], [CreatedBy], [ModifiedDate], [ModifiedBy]) VALUES (32, N'NM', 1, CAST(N'2015-10-12 00:00:00.000' AS DateTime), CAST(N'2099-10-12 00:00:00.000' AS DateTime), 1, CAST(N'2015-10-12 00:00:00.000' AS DateTime), 1, CAST(N'2015-10-12 00:00:00.000' AS DateTime), NULL)
GO
INSERT [dbo].[TypeOfState] ([id], [StateCode], [IsGoodState], [EffectiveDate], [ExpiryDate], [IsActive], [CreatedDate], [CreatedBy], [ModifiedDate], [ModifiedBy]) VALUES (33, N'NV', 1, CAST(N'2015-10-12 00:00:00.000' AS DateTime), CAST(N'2099-10-12 00:00:00.000' AS DateTime), 1, CAST(N'2015-10-12 00:00:00.000' AS DateTime), 1, CAST(N'2015-10-12 00:00:00.000' AS DateTime), NULL)
GO
INSERT [dbo].[TypeOfState] ([id], [StateCode], [IsGoodState], [EffectiveDate], [ExpiryDate], [IsActive], [CreatedDate], [CreatedBy], [ModifiedDate], [ModifiedBy]) VALUES (34, N'NY', 0, CAST(N'2015-10-12 00:00:00.000' AS DateTime), CAST(N'2099-10-12 00:00:00.000' AS DateTime), 1, CAST(N'2015-10-12 00:00:00.000' AS DateTime), 1, CAST(N'2015-10-12 00:00:00.000' AS DateTime), NULL)
GO
INSERT [dbo].[TypeOfState] ([id], [StateCode], [IsGoodState], [EffectiveDate], [ExpiryDate], [IsActive], [CreatedDate], [CreatedBy], [ModifiedDate], [ModifiedBy]) VALUES (35, N'OK', 1, CAST(N'2015-10-12 00:00:00.000' AS DateTime), CAST(N'2099-10-12 00:00:00.000' AS DateTime), 1, CAST(N'2015-10-12 00:00:00.000' AS DateTime), 1, CAST(N'2015-10-12 00:00:00.000' AS DateTime), NULL)
GO
INSERT [dbo].[TypeOfState] ([id], [StateCode], [IsGoodState], [EffectiveDate], [ExpiryDate], [IsActive], [CreatedDate], [CreatedBy], [ModifiedDate], [ModifiedBy]) VALUES (36, N'OR', 0, CAST(N'2015-10-12 00:00:00.000' AS DateTime), CAST(N'2099-10-12 00:00:00.000' AS DateTime), 1, CAST(N'2015-10-12 00:00:00.000' AS DateTime), 1, CAST(N'2015-10-12 00:00:00.000' AS DateTime), NULL)
GO
INSERT [dbo].[TypeOfState] ([id], [StateCode], [IsGoodState], [EffectiveDate], [ExpiryDate], [IsActive], [CreatedDate], [CreatedBy], [ModifiedDate], [ModifiedBy]) VALUES (37, N'PA', 1, CAST(N'2015-10-12 00:00:00.000' AS DateTime), CAST(N'2099-10-12 00:00:00.000' AS DateTime), 1, CAST(N'2015-10-12 00:00:00.000' AS DateTime), 1, CAST(N'2015-10-12 00:00:00.000' AS DateTime), NULL)
GO
INSERT [dbo].[TypeOfState] ([id], [StateCode], [IsGoodState], [EffectiveDate], [ExpiryDate], [IsActive], [CreatedDate], [CreatedBy], [ModifiedDate], [ModifiedBy]) VALUES (38, N'RI', 1, CAST(N'2015-10-12 00:00:00.000' AS DateTime), CAST(N'2099-10-12 00:00:00.000' AS DateTime), 1, CAST(N'2015-10-12 00:00:00.000' AS DateTime), 1, CAST(N'2015-10-12 00:00:00.000' AS DateTime), NULL)
GO
INSERT [dbo].[TypeOfState] ([id], [StateCode], [IsGoodState], [EffectiveDate], [ExpiryDate], [IsActive], [CreatedDate], [CreatedBy], [ModifiedDate], [ModifiedBy]) VALUES (39, N'SC', 1, CAST(N'2015-10-12 00:00:00.000' AS DateTime), CAST(N'2099-10-12 00:00:00.000' AS DateTime), 1, CAST(N'2015-10-12 00:00:00.000' AS DateTime), 1, CAST(N'2015-10-12 00:00:00.000' AS DateTime), NULL)
GO
INSERT [dbo].[TypeOfState] ([id], [StateCode], [IsGoodState], [EffectiveDate], [ExpiryDate], [IsActive], [CreatedDate], [CreatedBy], [ModifiedDate], [ModifiedBy]) VALUES (40, N'SD', 1, CAST(N'2015-10-12 00:00:00.000' AS DateTime), CAST(N'2099-10-12 00:00:00.000' AS DateTime), 1, CAST(N'2015-10-12 00:00:00.000' AS DateTime), 1, CAST(N'2015-10-12 00:00:00.000' AS DateTime), NULL)
GO
INSERT [dbo].[TypeOfState] ([id], [StateCode], [IsGoodState], [EffectiveDate], [ExpiryDate], [IsActive], [CreatedDate], [CreatedBy], [ModifiedDate], [ModifiedBy]) VALUES (41, N'TN', 1, CAST(N'2015-10-12 00:00:00.000' AS DateTime), CAST(N'2099-10-12 00:00:00.000' AS DateTime), 1, CAST(N'2015-10-12 00:00:00.000' AS DateTime), 1, CAST(N'2015-10-12 00:00:00.000' AS DateTime), NULL)
GO
INSERT [dbo].[TypeOfState] ([id], [StateCode], [IsGoodState], [EffectiveDate], [ExpiryDate], [IsActive], [CreatedDate], [CreatedBy], [ModifiedDate], [ModifiedBy]) VALUES (42, N'TX', 1, CAST(N'2015-10-12 00:00:00.000' AS DateTime), CAST(N'2099-10-12 00:00:00.000' AS DateTime), 1, CAST(N'2015-10-12 00:00:00.000' AS DateTime), 1, CAST(N'2015-10-12 00:00:00.000' AS DateTime), NULL)
GO
INSERT [dbo].[TypeOfState] ([id], [StateCode], [IsGoodState], [EffectiveDate], [ExpiryDate], [IsActive], [CreatedDate], [CreatedBy], [ModifiedDate], [ModifiedBy]) VALUES (43, N'UT', 1, CAST(N'2015-10-12 00:00:00.000' AS DateTime), CAST(N'2099-10-12 00:00:00.000' AS DateTime), 1, CAST(N'2015-10-12 00:00:00.000' AS DateTime), 1, CAST(N'2015-10-12 00:00:00.000' AS DateTime), NULL)
GO
INSERT [dbo].[TypeOfState] ([id], [StateCode], [IsGoodState], [EffectiveDate], [ExpiryDate], [IsActive], [CreatedDate], [CreatedBy], [ModifiedDate], [ModifiedBy]) VALUES (44, N'VA', 1, CAST(N'2015-10-12 00:00:00.000' AS DateTime), CAST(N'2099-10-12 00:00:00.000' AS DateTime), 1, CAST(N'2015-10-12 00:00:00.000' AS DateTime), 1, CAST(N'2015-10-12 00:00:00.000' AS DateTime), NULL)
GO
INSERT [dbo].[TypeOfState] ([id], [StateCode], [IsGoodState], [EffectiveDate], [ExpiryDate], [IsActive], [CreatedDate], [CreatedBy], [ModifiedDate], [ModifiedBy]) VALUES (45, N'VT', 1, CAST(N'2015-10-12 00:00:00.000' AS DateTime), CAST(N'2099-10-12 00:00:00.000' AS DateTime), 1, CAST(N'2015-10-12 00:00:00.000' AS DateTime), 1, CAST(N'2015-10-12 00:00:00.000' AS DateTime), NULL)
GO
INSERT [dbo].[TypeOfState] ([id], [StateCode], [IsGoodState], [EffectiveDate], [ExpiryDate], [IsActive], [CreatedDate], [CreatedBy], [ModifiedDate], [ModifiedBy]) VALUES (46, N'WI', 0, CAST(N'2015-10-12 00:00:00.000' AS DateTime), CAST(N'2099-10-12 00:00:00.000' AS DateTime), 1, CAST(N'2015-10-12 00:00:00.000' AS DateTime), 1, CAST(N'2015-10-12 00:00:00.000' AS DateTime), NULL)
GO
INSERT [dbo].[TypeOfState] ([id], [StateCode], [IsGoodState], [EffectiveDate], [ExpiryDate], [IsActive], [CreatedDate], [CreatedBy], [ModifiedDate], [ModifiedBy]) VALUES (47, N'WV', 1, CAST(N'2015-10-12 00:00:00.000' AS DateTime), CAST(N'2099-10-12 00:00:00.000' AS DateTime), 1, CAST(N'2015-10-12 00:00:00.000' AS DateTime), 1, CAST(N'2015-10-12 00:00:00.000' AS DateTime), NULL)
GO
