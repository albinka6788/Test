CREATE TABLE [dbo].[ScreenChronological] (
    [ID]          INT          NOT NULL,
    [ScreenID]    INT          NOT NULL,
    [UserIP]      VARCHAR (40) NOT NULL,
    [BrowserName] VARCHAR (50) NOT NULL,
    [CreatedBy]   INT          NULL,
    [CreatedDate] DATETIME     CONSTRAINT [DF_ScreenChronData_CreatedDate] DEFAULT (getdate()) NOT NULL,
    CONSTRAINT [PK_ScreenChronData] PRIMARY KEY CLUSTERED ([ID] ASC, [ScreenID] ASC)
);

