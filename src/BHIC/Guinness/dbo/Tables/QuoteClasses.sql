CREATE TABLE [dbo].[QuoteClasses] (
    [Id]           INT           NOT NULL,
    [QuoteID]      INT           NOT NULL,
    [ClassCode]    VARCHAR (500) NOT NULL,
    [IsActive]     BIT           NOT NULL,
    [CreatedDate]  DATETIME      CONSTRAINT [DF_QuoteClasses_CreatedDate] DEFAULT (getdate()) NOT NULL,
    [CreatedBy]    INT           NOT NULL,
    [ModifiedDate] DATETIME      CONSTRAINT [DF_QuoteClasses_ModifiedDate] DEFAULT (getdate()) NOT NULL,
    [ModifiedBy]   INT           NOT NULL,
    CONSTRAINT [PK_QuoteClasses] PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [FK_QuoteClasses_Quote] FOREIGN KEY ([QuoteID]) REFERENCES [dbo].[Quote] ([Id])
);

