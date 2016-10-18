
/****** Object:  Table [dbo].[ZipCodeStates]    Script Date: 11/6/2015 11:37:13 AM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

SET ANSI_PADDING ON
GO

CREATE TABLE [dbo].[ZipCodeStates](
	[Id] [int] NOT NULL,
	[ZipCode] [varchar](10) NOT NULL,
	[StateId] [int] NOT NULL,
	[IsActive] [bit] NULL,
	[CreatedDate] [datetime] NULL,
	[CreatedBy] [int] NULL,
	[ModifiedDate] [datetime] NULL,
	[ModifiedBy] [int] NULL,
 CONSTRAINT [PK_ZipCodeStates] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

SET ANSI_PADDING OFF
GO

ALTER TABLE [dbo].[ZipCodeStates] ADD  CONSTRAINT [DF_ZipCodeStates_CreatedDate]  DEFAULT (getdate()) FOR [CreatedDate]
GO

ALTER TABLE [dbo].[ZipCodeStates] ADD  CONSTRAINT [DF_ZipCodeStates_ModifiedDate]  DEFAULT (getdate()) FOR [ModifiedDate]
GO

ALTER TABLE [dbo].[ZipCodeStates]  WITH CHECK ADD  CONSTRAINT [FK_ZipCodeStates_StateMaster] FOREIGN KEY([StateId])
REFERENCES [dbo].[StateMaster] ([Id])
GO

ALTER TABLE [dbo].[ZipCodeStates] CHECK CONSTRAINT [FK_ZipCodeStates_StateMaster]
GO


