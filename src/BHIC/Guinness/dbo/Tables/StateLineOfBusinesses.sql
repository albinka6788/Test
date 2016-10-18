CREATE TABLE [dbo].[StateLineOfBusinesses] (
    [Id]               INT      NOT NULL,
    [StateId]          INT      NOT NULL,
    [LineOfBusinessId] INT      NOT NULL,
    [IsActive]         BIT      NOT NULL,
    [CreatedDate]      DATETIME CONSTRAINT [DF_StateLineOfBusinesses_CreatedDate] DEFAULT (getdate()) NULL,
    [CreatedBy]        INT      NULL,
    [ModifiedDate]     DATETIME CONSTRAINT [DF_StateLineOfBusinesses_ModifiedDate] DEFAULT (getdate()) NULL,
    [ModifiedBy]       INT      NULL,
    CONSTRAINT [PK_StateLineOfBusinesses] PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [FK_StateLineOfBusinesses_LineOfBusiness] FOREIGN KEY ([LineOfBusinessId]) REFERENCES [dbo].[LineOfBusiness] ([Id]),
    CONSTRAINT [FK_StateLineOfBusinesses_StateMaster] FOREIGN KEY ([StateId]) REFERENCES [dbo].[StateMaster] ([Id])
);

