CREATE TABLE [dbo].[Quote] (
    [Id]                       INT             NOT NULL,
    [OrganizationUserDetailID] INT             NOT NULL,
    [OrganizationAddressID]    INT             NOT NULL,
    [QuoteNumber]              VARCHAR (20)    NOT NULL,
    [QuotePolicyTypeID]        INT             NOT NULL,
    [ExternalSystemID]         INT             NOT NULL,
    [RequestDate]              DATETIME        NOT NULL,
    [ExpiryDate]               DATETIME        NOT NULL,
    [PremiumAmount]            NUMERIC (18, 2) NOT NULL,
    [IsActive]                 BIT             NOT NULL,
    [CreatedDate]              DATETIME        CONSTRAINT [DF_Quote_CreatedDate] DEFAULT (getdate()) NOT NULL,
    [CreatedBy]                INT             NOT NULL,
    [ModifiedDate]             DATETIME        CONSTRAINT [DF_Quote_ModifiedDate] DEFAULT (getdate()) NOT NULL,
    [ModifiedBy]               INT             NOT NULL,
    CONSTRAINT [PK_Quote] PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [FK_Quote_ExternalSystem] FOREIGN KEY ([ExternalSystemID]) REFERENCES [dbo].[ExternalSystem] ([Id]),
    CONSTRAINT [FK_Quote_OraganisationAddress] FOREIGN KEY ([OrganizationAddressID]) REFERENCES [dbo].[OraganisationAddress] ([Id]),
    CONSTRAINT [FK_Quote_OrganisationUserDetail] FOREIGN KEY ([OrganizationUserDetailID]) REFERENCES [dbo].[OrganisationUserDetail] ([Id]),
    CONSTRAINT [FK_Quote_QuotePolicyType] FOREIGN KEY ([QuotePolicyTypeID]) REFERENCES [dbo].[QuotePolicyType] ([Id])
);

