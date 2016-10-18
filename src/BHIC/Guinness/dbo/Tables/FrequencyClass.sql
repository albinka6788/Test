CREATE TABLE [dbo].[FrequencyClass] (
    [Id]                       INT          NOT NULL,
    [StateCode]                CHAR (2)     NOT NULL,
    [ClassCode]                VARCHAR (10) NOT NULL,
    [OrganizationActivityType] SMALLINT     NOT NULL,
    [EffectiveDate]            DATETIME     CONSTRAINT [DF_FrequencyClass_EffectiveDate] DEFAULT (getdate()) NOT NULL,
    [ExpiryDate]               DATETIME     NOT NULL,
    [IsActive]                 BIT          NOT NULL,
    [CreatedDate]              DATETIME     CONSTRAINT [DF_FrequencyClass_CreatedDate] DEFAULT (getdate()) NOT NULL,
    [CreatedBy]                INT          NOT NULL,
    [ModifiedDate]             DATETIME     CONSTRAINT [DF_FrequencyClass_ModifiedDate] DEFAULT (getdate()) NULL,
    [ModifiedBy]               INT          NULL,
    CONSTRAINT [PK_FrequencyClass] PRIMARY KEY CLUSTERED ([Id] ASC)
);

