CREATE TABLE [dbo].[OrganisationUserDetail] (
    [Id]               INT           NOT NULL,
    [OrganizationName] VARCHAR (200) NOT NULL,
    [EmailID]          VARCHAR (150) NOT NULL,
    [Password]         VARCHAR (256) NOT NULL,
    [Tin]              INT           NULL,
    [Ssn]              INT           NULL,
    [Fein]             INT           NULL,
    [IsActive]         BIT           NOT NULL,
    [CreatedDate]      DATETIME      CONSTRAINT [DF_OrganisationUserDetail_CreatedDate] DEFAULT (getdate()) NOT NULL,
    [CreatedBy]        INT           NOT NULL,
    [ModifiedDate]     DATETIME      CONSTRAINT [DF_OrganisationUserDetail_ModifiedDate] DEFAULT (getdate()) NOT NULL,
    [ModifiedBy]       INT           NOT NULL,
    CONSTRAINT [PK_TBS_User_Detail] PRIMARY KEY CLUSTERED ([Id] ASC)
);

