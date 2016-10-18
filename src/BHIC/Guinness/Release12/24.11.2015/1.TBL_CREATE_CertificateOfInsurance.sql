/****** Object:  Table [dbo].[CertificateOfInsurance]    Script Date: 19-11-2015 PM 04:08:07 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

SET ANSI_PADDING ON
GO

CREATE TABLE [dbo].[CertificateOfInsurance](
	[Id] [INT] NOT NULL,
	[PolicyId] [INT] NOT NULL,
	[CertificateRequestId] [VARCHAR](20) NOT NULL,
	[IsActive] [BIT] NOT NULL,
	[CreatedDate] [DATETIME] NOT NULL,
	[CreatedBy] [INT] NOT NULL,
	[ModifiedDate] [DATETIME] NOT NULL,
	[ModifiedBy] [INT] NOT NULL,
 CONSTRAINT [PK_CertificateOfInsurance] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

SET ANSI_PADDING OFF
GO

ALTER TABLE [dbo].[CertificateOfInsurance] ADD  CONSTRAINT [DF_CertificateOfInsurance_CreatedDate]  DEFAULT (GETDATE()) FOR [CreatedDate]
GO

ALTER TABLE [dbo].[CertificateOfInsurance] ADD  CONSTRAINT [DF_CertificateOfInsurance_ModifiedDate]  DEFAULT (GETDATE()) FOR [ModifiedDate]
GO

ALTER TABLE [dbo].[CertificateOfInsurance]  WITH CHECK ADD  CONSTRAINT [FK_CertificateOfInsurance_Policy] FOREIGN KEY([PolicyId])
REFERENCES [dbo].[Policy] ([Id])
GO

ALTER TABLE [dbo].[CertificateOfInsurance] CHECK CONSTRAINT [FK_CertificateOfInsurance_Policy]
GO


