CREATE TABLE [dbo].[StateMaster] (
    [Id]           INT          NOT NULL,
    [StateCode]    CHAR (2)     NOT NULL,
    [FullName]     VARCHAR (20) NULL,
    [IsActive]     BIT          NOT NULL,
    [CreatedDate]  DATETIME     CONSTRAINT [DF_StateMaster_CreatedDate] DEFAULT (getdate()) NULL,
    [CreatedBy]    INT          NULL,
    [ModifiedDate] DATETIME     CONSTRAINT [DF_StateMaster_ModifiedDate] DEFAULT (getdate()) NULL,
    [ModifiedBy]   INT          NULL,
    CONSTRAINT [PK_StateMaster] PRIMARY KEY CLUSTERED ([Id] ASC)
);

