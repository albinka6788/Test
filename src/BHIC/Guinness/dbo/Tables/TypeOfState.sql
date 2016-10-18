CREATE TABLE [dbo].[TypeOfState] (
    [id]            INT      NOT NULL,
    [StateId]       INT      NULL,
    [IsGoodState]   BIT      NOT NULL,
    [EffectiveDate] DATETIME NULL,
    [ExpiryDate]    DATETIME NULL,
    [IsActive]      BIT      NULL,
    [CreatedDate]   DATETIME CONSTRAINT [DF_TypeOfState_CreatedDate] DEFAULT (getdate()) NULL,
    [CreatedBy]     INT      NULL,
    [ModifiedDate]  DATETIME CONSTRAINT [DF_TypeOfState_ModifiedDate] DEFAULT (getdate()) NULL,
    [ModifiedBy]    INT      NULL,
    CONSTRAINT [PK_TypeOfState] PRIMARY KEY CLUSTERED ([id] ASC),
    CONSTRAINT [FK__TypeOfSta__State__1C873BEC] FOREIGN KEY ([StateId]) REFERENCES [dbo].[StateMaster] ([Id])
);

