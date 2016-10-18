
/****** Object:  StoredProcedure [dbo].[GetPrimaryClassCodeData]    Script Date: 11/9/2015 6:00:42 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- ================================================
/*
@Author      : Nishank Kumar Srivastava
@Description : This proc will get all the data related to primary class code
@CreatedOn   : 2015-Nov-9 

@Updation History : 
   @Update-0 : {date}, {by}, {description} 
   
@ExampleCall : 
EXEC GetPrimaryClassCodeData

*/
-- ================================================

ALTER PROCEDURE [dbo].[GetPrimaryClassCodeData]
AS 
BEGIN

	-- SET NOCOUNT ON added to prevent extra result sets from
	SET NOCOUNT OFF;

	SELECT T2.StateCode,T1.ClassDescriptionId,T1.MinimumPayrollThreshold,T1.FriendlyName
	FROM MulticlassMinimumPayrollThreshold T1 
	LEFT OUTER JOIN StateMaster T2 ON T1.StateId = T2.Id

END


