
-- =============================================
-- Author:		Navneet Kumar
-- Create date: 04/15/2016
-- Description:	Use to Get Master data for Landing Page
-- Use Exec EXEC [GetAllLandingPageMasterData]
-- =============================================
-- EXEC [GetAllLandingPageMasterData]
/* History 
   Navneet - 04/15/2016 Added Active and Inactive status
*/

ALTER PROCEDURE [dbo].[GetAllLandingPageMasterData]

AS 
BEGIN

	-- SET NOCOUNT ON added to prevent extra result sets from
	SET NOCOUNT ON;


	SELECT id as Value , Abbreviation as [Text] FROM LineOfBusiness

	SELECT Id as Value, StateCode as [Text] from dbo.StateMaster WHERE IsActive = 1 ORDER BY StateCode

	SELECT Id as Value, Template as [Text] from dbo.tblTemplates where Status='Active' ORDER BY Id asc

END



