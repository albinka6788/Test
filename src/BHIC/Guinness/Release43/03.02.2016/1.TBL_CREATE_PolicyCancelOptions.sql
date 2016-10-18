/****** Object:  Table [dbo].[PolicyCancelOptions]    Script Date: 02/02/2016 6:02:16 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

SET ANSI_PADDING ON
GO

CREATE TABLE [dbo].[PolicyCancelOptions](
	[Id] [int] NOT NULL,
	[LineOfBusinessID] [int] NULL,
	[Options] [varchar](255) NULL,
	[IsActive] [bit] NULL,
	[CreatedDate] [datetime] NULL,
	[CreatedBy] [int] NULL,
	[ModifiedDate] [datetime] NULL,
	[ModifiedBy] [int] NULL,
	[EffectiveDate] [datetime] NULL,
	[DiscontinuedDate] [datetime] NULL,
 CONSTRAINT [PK__PolicyCa__3213E83FB2CC69DF] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

SET ANSI_PADDING OFF
GO

ALTER TABLE [dbo].[PolicyCancelOptions]  WITH CHECK ADD  CONSTRAINT [FK_PolicyCancelOptions_LineOfBusiness] FOREIGN KEY([LineOfBusinessID])
REFERENCES [dbo].[LineOfBusiness] ([Id])
GO

ALTER TABLE [dbo].[PolicyCancelOptions] CHECK CONSTRAINT [FK_PolicyCancelOptions_LineOfBusiness]
GO
