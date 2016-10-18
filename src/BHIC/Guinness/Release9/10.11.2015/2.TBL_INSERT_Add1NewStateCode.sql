
--Select * From [StateMaster]

INSERT INTO [dbo].[StateMaster]
           ([Id]
           ,[StateCode]
           ,[FullName]
           ,[IsActive]
           ,[CreatedDate]
           ,[CreatedBy]
           ,[ModifiedDate]
           ,[ModifiedBy])
     VALUES
           (NEXT VALUE FOR SEQUENCEStateMaster
           ,'ND'
           ,NULL
           ,1
           ,GETDATE()
           ,1
           ,GETDATE()
           ,NULL)
GO


