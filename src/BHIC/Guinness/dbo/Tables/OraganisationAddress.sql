﻿CREATE TABLE [dbo].[OraganisationAddress] (
    [Id]                 INT           NOT NULL,
    [OrganizationID]     INT           NOT NULL,
    [Address1]           VARCHAR (200) NOT NULL,
    [Address2]           VARCHAR (200) NOT NULL,
    [Address3]           VARCHAR (200) NOT NULL,
    [City]               VARCHAR (200) NOT NULL,
    [County]             VARCHAR (200) NOT NULL,
    [StateCode]          CHAR (2)      NOT NULL,
    [ZipCode]            INT           NOT NULL,
    [CountryID]          INT           NOT NULL,
    [IsCorporateAddress] BIT           CONSTRAINT [DF_OraganisationAddress_IsCorporateAddress] DEFAULT ((1)) NOT NULL,
    [ContactName]        VARCHAR (200) NULL,
    [ContactNumber1]     INT           NULL,
    [ContactNumber2]     INT           NULL,
    [Fax]                INT           NULL,
    [IsActive]           BIT           NOT NULL,
    [CreatedDate]        DATETIME      CONSTRAINT [DF_OraganisationAddress_CreatedDate] DEFAULT (getdate()) NOT NULL,
    [CreatedBy]          INT           NOT NULL,
    [ModifiedDate]       DATETIME      CONSTRAINT [DF_OraganisationAddress_ModifiedDate] DEFAULT (getdate()) NOT NULL,
    [ModifiedBy]         INT           NOT NULL,
    CONSTRAINT [PK_TBS_User_Address] PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [FK_TBS_User_Address_UserID] FOREIGN KEY ([OrganizationID]) REFERENCES [dbo].[OrganisationUserDetail] ([Id])
);
