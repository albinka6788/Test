
/****** Object:  StoredProcedure [dbo].[GetStateType]    Script Date: 11/2/2015 2:46:50 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- ================================================
/*
@Author      : Nishank Kumar Srivastava
@Description : This proc will return State Type based on State code provided
@CreatedOn   : 2015-Oct-16 

@Updation History : 
   @Update-1 : {date}, {by}, {description} 

@ExampleCall : EXEC GetStateType 

*/
-- ================================================

ALTER PROCEDURE [dbo].[GetStateType]

AS 
BEGIN

	-- SET NOCOUNT ON added to prevent extra result sets from
	SET NOCOUNT ON;

	SELECT TS.IsGoodState AS StateType,SM.StateCode
	FROM TypeOfState TS
	INNER JOIN StateMaster SM
	ON SM.Id = TS.StateId
	WHERE GETDATE() BETWEEN TS.EffectiveDate AND TS.ExpiryDate
	AND CAST(ExpiryDate AS DATE) >= CAST(GETDATE() AS DATE);

END