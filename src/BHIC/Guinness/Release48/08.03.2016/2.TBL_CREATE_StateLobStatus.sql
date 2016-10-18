

/****** Object:  Table [dbo].[StateLobStatus]    Script Date: 3/8/2016 12:16:57 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[StateLobStatus](
	[Id] [int] NOT NULL,
	[StateLobId] [int] NOT NULL,
	[StatusId] [int] NOT NULL,
	[EffectiveFrom] [datetime] NOT NULL,
	[ExpiryOn] [datetime] NULL,
	[IsActive] [bit] NOT NULL,
	[CreatedDate] [datetime] NOT NULL,
	[CreatedBy] [int] NOT NULL,
	[ModifiedDate] [datetime] NOT NULL,
	[ModifiedBy] [int] NOT NULL,
 CONSTRAINT [PK_StateLobStatus] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

ALTER TABLE [dbo].[StateLobStatus]  WITH CHECK ADD  CONSTRAINT [FK_StateLobStatus_LobStatus] FOREIGN KEY([StatusId])
REFERENCES [dbo].[LobStatus] ([Id])
GO

ALTER TABLE [dbo].[StateLobStatus] CHECK CONSTRAINT [FK_StateLobStatus_LobStatus]
GO

ALTER TABLE [dbo].[StateLobStatus]  WITH CHECK ADD  CONSTRAINT [FK_StateLobStatus_StateLineOfBusinesses] FOREIGN KEY([StateLobId])
REFERENCES [dbo].[StateLineOfBusinesses] ([Id])
GO

ALTER TABLE [dbo].[StateLobStatus] CHECK CONSTRAINT [FK_StateLobStatus_StateLineOfBusinesses]
GO


