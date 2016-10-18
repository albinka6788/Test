CREATE TABLE [dbo].[UserExternalSystemDetail] (
    [Id]               INT           NOT NULL,
    [ExternalSystemID] INT           NOT NULL,
    [UserName]         VARCHAR (200) NOT NULL,
    [Password]         VARCHAR (256) NOT NULL,
    [OrganizationID]   INT           NOT NULL,
    [IsActive]         BIT           NOT NULL,
    [CreatedDate]      DATETIME      CONSTRAINT [DF_UserExternalSystemDetail_CreatedDate] DEFAULT (getdate()) NOT NULL,
    [CreatedBy]        INT           NOT NULL,
    [ModifiedDate]     DATETIME      CONSTRAINT [DF_UserExternalSystemDetail_ModifiedDate] DEFAULT (getdate()) NOT NULL,
    [ModifiedBy]       INT           NOT NULL,
    CONSTRAINT [PK_TBS_User_External_System_Detail] PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [FK_UserExternalSystemDetail_ExternalSystem] FOREIGN KEY ([ExternalSystemID]) REFERENCES [dbo].[ExternalSystem] ([Id])
);

