CREATE TABLE [dbo].[StateMinimumPremium] (
    [Id]             INT             IDENTITY (1, 1) NOT NULL,
    [StateId]        INT             NULL,
    [MinimumPremium] NUMERIC (18, 2) NOT NULL,
    [EffectiveDate]  DATETIME        NOT NULL,
    [ExpiryDate]     DATETIME        NOT NULL,
    CONSTRAINT [PK_StateMinimumPremium] PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [FK__StateMini__State__22401542] FOREIGN KEY ([StateId]) REFERENCES [dbo].[StateMaster] ([Id])
);

