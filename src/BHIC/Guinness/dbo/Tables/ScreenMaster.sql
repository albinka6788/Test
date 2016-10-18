CREATE TABLE [dbo].[ScreenMaster] (
    [ID]           INT           NOT NULL,
    [Name]         VARCHAR (200) NOT NULL,
    [IsActive]     BIT           NOT NULL,
    [CreatedDate]  DATETIME      CONSTRAINT [DF_ScreenMaster_CreatedDate] DEFAULT (getdate()) NOT NULL,
    [CreatedBy]    INT           NOT NULL,
    [ModifiedDate] DATETIME      CONSTRAINT [DF_ScreenMaster_ModifiedDate] DEFAULT (getdate()) NOT NULL,
    [ModifiedBy]   INT           NOT NULL,
    CONSTRAINT [PK_ScreenMaster] PRIMARY KEY CLUSTERED ([ID] ASC)
);

