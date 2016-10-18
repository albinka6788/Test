
/****** Object:  Table [dbo].[MulticlassMinimumPayrollThreshold]    Script Date: 11/9/2015 11:59:14 AM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

SET ANSI_PADDING ON
GO

CREATE TABLE [dbo].[MulticlassMinimumPayrollThreshold](
	[Id] [int] NOT NULL,
	[StateId] [int] NULL,
	[ClassDescriptionId] [int] NOT NULL,
	[MinimumPayrollThreshold] [decimal](10, 2) NOT NULL,
	[FriendlyName] [varchar](100) NULL,
	[IsActive] [bit] NOT NULL,
	[CreatedDate] [datetime] NULL,
	[CreatedBy] [int] NULL,
	[ModifiedDate] [datetime] NULL,
	[ModifiedBy] [int] NULL,
 CONSTRAINT [PK_MinimumPayrollThreshold] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

SET ANSI_PADDING OFF
GO

ALTER TABLE [dbo].[MulticlassMinimumPayrollThreshold] ADD  CONSTRAINT [DF_MinimumPayrollThreshold_CreatedDate]  DEFAULT (getdate()) FOR [CreatedDate]
GO

ALTER TABLE [dbo].[MulticlassMinimumPayrollThreshold] ADD  CONSTRAINT [DF_MinimumPayrollThreshold_ModifiedDate]  DEFAULT (getdate()) FOR [ModifiedDate]
GO

ALTER TABLE [dbo].[MulticlassMinimumPayrollThreshold]  WITH CHECK ADD  CONSTRAINT [FK_MinimumPayrollThreshold_StateMaster] FOREIGN KEY([StateId])
REFERENCES [dbo].[StateMaster] ([Id])
GO

ALTER TABLE [dbo].[MulticlassMinimumPayrollThreshold] CHECK CONSTRAINT [FK_MinimumPayrollThreshold_StateMaster]
GO


