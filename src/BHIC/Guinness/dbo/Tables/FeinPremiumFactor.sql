CREATE TABLE [dbo].[FeinPremiumFactor] (
    [Id]            INT             NOT NULL,
    [Fein]          INT             NOT NULL,
    [PremiumFactor] NUMERIC (18, 2) NOT NULL,
    [EffectiveDate] DATETIME        CONSTRAINT [DF_FeinPremiumFactor_EffectiveDate] DEFAULT (getdate()) NOT NULL,
    [ExpiryDate]    DATETIME        NOT NULL,
    CONSTRAINT [PK_FeinPremiumFactor] PRIMARY KEY CLUSTERED ([Id] ASC)
);

