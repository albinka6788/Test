CREATE TABLE [dbo].[Policy] (
    [Id]              INT             NOT NULL,
    [QuoteID]         INT             NOT NULL,
    [PolicyNumber]    VARCHAR (20)    NOT NULL,
    [EffectiveDate]   DATETIME        NOT NULL,
    [ExpiryDate]      DATETIME        NOT NULL,
    [PremiumAmount]   NUMERIC (18, 2) NOT NULL,
    [PaymentOptionID] INT             NOT NULL,
    [IsActive]        BIT             NOT NULL,
    [CreatedDate]     DATETIME        CONSTRAINT [DF_Policy_CreatedDate] DEFAULT (getdate()) NOT NULL,
    [CreatedBy]       INT             NOT NULL,
    [ModifiedDate]    DATETIME        CONSTRAINT [DF_Policy_ModifiedDate] DEFAULT (getdate()) NOT NULL,
    [ModifiedBy]      INT             NOT NULL,
    CONSTRAINT [PK_Policy] PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [FK_Policy_Quote] FOREIGN KEY ([QuoteID]) REFERENCES [dbo].[Quote] ([Id])
);

