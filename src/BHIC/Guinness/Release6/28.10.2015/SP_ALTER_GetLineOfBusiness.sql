
/****** Object:  StoredProcedure [dbo].[GetLineOfBusiness]    Script Date: 10/28/2015 10:45:19 PM ******/
DROP PROCEDURE [dbo].[GetLineOfBusiness]
GO

/****** Object:  StoredProcedure [dbo].[GetLineOfBusiness]    Script Date: 10/28/2015 10:45:19 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

-- ================================================
/*
@Author      : Anuj Kumar Singh
@Description : This proc will fetch record based on state code and abbreviation
@CreatedOn   : 2015-oct-27 

@Updation History : 
   @Update-1 : {date}, {by}, {description} 

*/
-- ================================================

CREATE PROCEDURE [dbo].[GetLineOfBusiness] 
(
  	  @StateCode VARCHAR(20)
)
AS 
BEGIN

	-- SET NOCOUNT ON added to prevent extra result sets from
	SET NOCOUNT OFF;

	SELECT T3.Id as LobId,T3.Abbreviation,T2.StateCode,T1.Id FROM StateLineOfBusinesses T1
	INNER JOIN StateMaster T2 ON T1.StateId=T2.Id
	INNER JOIN LineOfBusiness T3 ON T1.LineOfBusinessId=T3.Id
	WHERE T2.StateCode Like @StateCode AND T1.IsActive=1 AND T2.IsActive=1 AND T3.IsActive=1
END




GO


