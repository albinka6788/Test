
/****** Object:  Table [dbo].[PolicyChangeOptions]    Script Date: 27-10-2015 PM 08:11:50 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[PolicyChangeOptions](
	[Id] [int] NOT NULL,
	[LineOfBusinessID] [int] NULL,
	[Options] [varchar](255) NULL,
	[IsActive] [bit] NULL,
	[CreatedDate] [datetime] NULL,
	[CreatedBy] [int] NULL,
	[ModifiedDate] [datetime] NULL,
	[ModifiedBy] [int] NULL,
 CONSTRAINT [PK__PolicyCh__3213E83FB2CC69DF] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO

ALTER TABLE [dbo].[PolicyChangeOptions]  WITH CHECK ADD  CONSTRAINT [FK_PolicyChangeOptions_LineOfBusiness] FOREIGN KEY([LineOfBusinessID])
REFERENCES [dbo].[LineOfBusiness] ([Id])
GO
ALTER TABLE [dbo].[PolicyChangeOptions] CHECK CONSTRAINT [FK_PolicyChangeOptions_LineOfBusiness]
GO
