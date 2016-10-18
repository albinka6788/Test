
ALTER TABLE [dbo].[OrganisationUserDetail] 
	  ADD AccountLockedDateTime DateTime NOT NULL DEFAULT((GETDATE())),
		  LoginAttempt int NOT NULL DEFAULT ((0))