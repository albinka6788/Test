
/****** Object:  StoredProcedure [dbo].[GetZipCodeStateDetails]    Script Date: 11/6/2015 6:23:06 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- ================================================
/*
@Author      : Anuj Kumar Singh
@Description : This proc will fetch list of states 
@CreatedOn   : 2015-nov-6

@ExampleCall exec GetZipCodeStateDetails 

@Updation History : 
   @Update-1 : {date}, {by}, {description} 

*/
-- ================================================

CREATE PROCEDURE [dbo].[GetZipCodeStateDetails] 

AS 
BEGIN

	-- SET NOCOUNT ON added to prevent extra result sets from
	SET NOCOUNT OFF;
	
			SELECT T1.Id,T1.ZipCode,T2.Id as StateId,T2.StateCode FROM ZipCodeStates T1 
			INNER JOIN 
			StateMaster T2 ON T1.StateId = T2.Id WHERE T1.IsActive = 1 AND T2.IsActive = 1
		    
END

