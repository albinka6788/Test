
/****** Object:  StoredProcedure [dbo].[GetLineOfBusiness]    Script Date: 11/2/2015 12:18:28 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- ================================================
/*
@Author      : Anuj Kumar Singh
@Description : This proc will fetch record based on state code and abbreviation
@CreatedOn   : 2015-oct-27 

@ExampleCall exec GetLineOfBusiness 

@Updation History : 
   @Update-1 : {date}, {by}, {description} 

*/
-- ================================================

ALTER PROCEDURE [dbo].[GetLineOfBusiness] 

AS 
BEGIN

	-- SET NOCOUNT ON added to prevent extra result sets from
	SET NOCOUNT OFF;

	SELECT T3.Id as LobId,T3.Abbreviation,T2.StateCode,T1.Id FROM StateLineOfBusinesses T1
	INNER JOIN StateMaster T2 ON T1.StateId=T2.Id
	INNER JOIN LineOfBusiness T3 ON T1.LineOfBusinessId=T3.Id
	WHERE T1.IsActive=1 AND T2.IsActive=1 AND T3.IsActive=1

END




