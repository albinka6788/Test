INSERT INTO [dbo].[OrganisationUserDetail]
           ([Id]
           ,[OrganizationName]
           ,[EmailID]
           ,[Password]
           ,[Tin]
           ,[Ssn]
           ,[Fein]
           ,[IsActive]
           ,[CreatedDate]
           ,[CreatedBy]
           ,[ModifiedDate]
           ,[ModifiedBy]
           ,[FirstName]
           ,[LastName]
           ,[PolicyCode]
           ,[PhoneNumber]
           ,[IsEmailVerified]
           ,[ForgotPwdRequestedDateTime]
           ,[AccountLockedDateTime]
           ,[LoginAttempt])
     VALUES
           (1
           ,''
           ,'admin@cyb.com'
           ,''
           ,null
           ,null
		   ,null
           ,0
           ,GETDATE()
           ,1
           ,GETDATE()
           ,1
           ,null
           ,null
           ,null
           ,null
           ,0
           ,null
           ,null
           ,0)
GO


