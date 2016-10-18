CREATE TABLE [dbo].[PolicyPaymentDetail] (
    [Id]               INT             NOT NULL,
    [PolicyID]         INT             NOT NULL,
    [CreditCardNumber] VARCHAR (256)   NOT NULL,
    [CardExpiryDate]   DATETIME        NOT NULL,
    [TransactionCode]  VARCHAR (200)   NOT NULL,
    [AmountPaid]       NUMERIC (18, 2) NOT NULL,
    [IsActive]         BIT             NOT NULL,
    [CreatedDate]      DATETIME        CONSTRAINT [DF_PolicyPaymentDetail_CreatedDate] DEFAULT (getdate()) NOT NULL,
    [CreatedBy]        INT             NOT NULL,
    [ModifiedDate]     DATETIME        CONSTRAINT [DF_PolicyPaymentDetail_ModifiedDate] DEFAULT (getdate()) NOT NULL,
    [ModifiedBy]       INT             NOT NULL,
    CONSTRAINT [PK_PolicyPaymentDetail] PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [FK_PolicyPaymentDetail_Policy] FOREIGN KEY ([PolicyID]) REFERENCES [dbo].[Policy] ([Id])
);

