
DROP INDEX [non_clustered_Fein_FeinPremiumFactor] ON  [dbo].[FeinPremiumFactor] 
go

ALTER TABLE [dbo].[FeinPremiumFactor] ALTER COLUMN Fein VARCHAR(256)
go

/****** Object:  Index [non_clustered_Fein_FeinPremiumFactor]    Script Date: 4/22/2016 7:44:59 AM ******/
CREATE NONCLUSTERED INDEX [non_clustered_Fein_FeinPremiumFactor] ON [dbo].[FeinPremiumFactor]
(
	[Fein] ASC,
	[ExpiryDate] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, FILLFACTOR = 80) ON [PRIMARY]
GO


