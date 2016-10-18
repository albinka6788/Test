﻿/****** Object:  Table [dbo].[CYBClassKeywords]    Script Date: 06-06-2016 21:31:43 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[CYBClassKeywords](
	[ClassKeyword] [varchar](100) NOT NULL
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
SET ANSI_PADDING ON

GO
/****** Object:  Index [CLIX_CYBClassKeywords]    Script Date: 06-06-2016 21:31:43 ******/
CREATE CLUSTERED INDEX [CLIX_CYBClassKeywords] ON [dbo].[CYBClassKeywords]
(
	[ClassKeyword] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO