CREATE TABLE [dbo].[ControlMaster] (
    [ID]           INT           NOT NULL,
    [ScreenID]     INT           NOT NULL,
    [Name]         VARCHAR (200) NOT NULL,
    [IsActive]     BIT           NOT NULL,
    [CreatedDate]  DATETIME      CONSTRAINT [DF_ControlMaster_CreatedDate] DEFAULT (getdate()) NOT NULL,
    [CreatedBy]    INT           NOT NULL,
    [ModifiedDate] DATETIME      CONSTRAINT [DF_ControlMaster_ModifiedDate] DEFAULT (getdate()) NOT NULL,
    [ModifiedBy]   INT           NOT NULL,
    CONSTRAINT [PK_ControlMaster] PRIMARY KEY CLUSTERED ([ID] ASC)
);

