CREATE TABLE [dbo].[LineOfBusiness] (
    [Id]                 INT          NOT NULL,
    [Abbreviation]       VARCHAR (20) NOT NULL,
    [LineOfBusinessName] VARCHAR (50) NULL,
    [IsActive]           BIT          NOT NULL,
    [CreatedDate]        DATETIME     CONSTRAINT [DF_LineOfBusiness_CreatedDate] DEFAULT (getdate()) NULL,
    [CreatedBy]          INT          NULL,
    [ModifiedDate]       DATETIME     CONSTRAINT [DF_LineOfBusiness_ModifiedDate] DEFAULT (getdate()) NULL,
    [ModifiedBy]         INT          NULL,
    CONSTRAINT [PK_LineOfBusiness] PRIMARY KEY CLUSTERED ([Id] ASC)
);

