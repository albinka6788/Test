CREATE TABLE [dbo].[ControlChronological] (
    [ID]                    BIGINT        NOT NULL,
    [ScreenChronologicalID] BIGINT        NOT NULL,
    [ControlID]             INT           NOT NULL,
    [OldValue]              VARCHAR (MAX) NULL,
    [NewValue]              VARCHAR (MAX) NULL,
    [CreatedBy]             INT           NULL,
    [CreatedDate]           DATETIME      CONSTRAINT [DF_Table_1_CreatedDate] DEFAULT (getdate()) NOT NULL,
    CONSTRAINT [PK_ControlHistory_1] PRIMARY KEY CLUSTERED ([ID] ASC, [ScreenChronologicalID] ASC, [ControlID] ASC)
);

