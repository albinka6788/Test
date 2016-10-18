CREATE TABLE [dbo].[FrequencyClaims] (
    [TotalPayrollMin]          NUMERIC (18) NOT NULL,
    [TotalPayrollMax]          NUMERIC (18) NULL,
    [ThresholdNumberOfClaims]  SMALLINT     NOT NULL,
    [OrganizationActivityType] SMALLINT     NOT NULL,
    [IsActive]                 BIT          NOT NULL,
    [CreatedDate]              DATETIME     CONSTRAINT [DF_FrequencyClaims_CreatedDate] DEFAULT (getdate()) NOT NULL,
    [CreatedBy]                INT          NOT NULL,
    [ModifiedDate]             DATETIME     CONSTRAINT [DF_FrequencyClaims_ModifiedDate] DEFAULT (getdate()) NULL,
    [ModifiedBy]               INT          NULL
);

