
/****** Object:  Table [dbo].[MandatoryDeductible]    Script Date: 27-10-2015 PM 08:56:48 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[MandatoryDeductible](
	[Id] [int] NOT NULL,
	[StateId] [int] NOT NULL,
	[IsActive] [bit] NOT NULL,
	[CreatedDate] [datetime] NULL,
	[CreatedBy] [int] NULL,
	[ModifiedDate] [datetime] NULL,
	[ModifiedBy] [int] NULL,
 CONSTRAINT [PK_MandatoryDeductible] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
ALTER TABLE [dbo].[MandatoryDeductible] ADD  CONSTRAINT [DF_MandatoryDeductible_CreatedDate]  DEFAULT (getdate()) FOR [CreatedDate]
GO
ALTER TABLE [dbo].[MandatoryDeductible] ADD  CONSTRAINT [DF_MandatoryDeductible_ModifiedDate]  DEFAULT (getdate()) FOR [ModifiedDate]
GO
ALTER TABLE [dbo].[MandatoryDeductible]  WITH CHECK ADD  CONSTRAINT [FK_MandatoryDeductible_StateMaster] FOREIGN KEY([StateId])
REFERENCES [dbo].[StateMaster] ([Id])
GO
ALTER TABLE [dbo].[MandatoryDeductible] CHECK CONSTRAINT [FK_MandatoryDeductible_StateMaster]
GO
