
/****** Object:  StoredProcedure [dbo].[GetAllLandingPageMasterData]    Script Date: 14-03-2016 17:00:49 ******/
DROP PROCEDURE [dbo].[GetAllLandingPageMasterData]
GO

-- EXEC [GetAllLandingPageMasterData]

CREATE PROCEDURE [dbo].[GetAllLandingPageMasterData]

AS 
BEGIN

	-- SET NOCOUNT ON added to prevent extra result sets from
	SET NOCOUNT ON;


	SELECT id as Value , Abbreviation as [Text] FROM LineOfBusiness

	SELECT Id as Value, StateCode as [Text] from dbo.StateMaster WHERE IsActive = 1 ORDER BY StateCode

	SELECT Id as Value, Template as [Text] from dbo.tblTemplates  ORDER BY Id asc

END



GO


