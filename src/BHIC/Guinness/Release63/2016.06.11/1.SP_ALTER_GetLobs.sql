-- ================================================
/*
@Author      : Anuj Kumar Singh
@Description : This proc will fetch record based on state code and abbreviation
@CreatedOn   : 2015-oct-27 

@ExampleCall exec GetLineOfBusiness 

@Updation History : 
   @Update-0 : {date}, {by}, {description} 
   @Update-1 : {2016-MAY-11}, {Prem}, {Now we have also require LOB full name to show on UI} 

*/
-- ================================================

ALTER PROCEDURE [dbo].[GetLineOfBusiness] 

AS 
BEGIN

	-- SET NOCOUNT ON added to prevent extra result sets from
	SET NOCOUNT OFF;

	SELECT T3.Id as LobId,T3.Abbreviation,T2.StateCode,T1.Id,T3.LineOfBusinessName LobFullName FROM StateLineOfBusinesses T1
	INNER JOIN StateMaster T2 ON T1.StateId=T2.Id
	INNER JOIN LineOfBusiness T3 ON T1.LineOfBusinessId=T3.Id
	WHERE T1.IsActive=1 AND T2.IsActive=1 AND T3.IsActive=1
END




