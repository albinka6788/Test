CREATE TABLE [dbo].[QuotePolicyType] (
    [Id]          INT           NOT NULL,
    [Name]        VARCHAR (50)  NOT NULL,
    [Description] VARCHAR (200) NOT NULL,
    CONSTRAINT [PK_QuotePolicyType] PRIMARY KEY CLUSTERED ([Id] ASC)
);

