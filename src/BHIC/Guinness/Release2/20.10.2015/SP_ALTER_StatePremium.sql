-- ================================================
/*
@Author      : Perm Pratap Singh Rajpurohit
@Description : This proc will return state with thier minimum payroll threshold limit based on supplied State
@CreatedOn   : 2015-Sep-29 

@Updation History : 
   @Update-1 : {date}, {by}, {description} 

@ExampleCall : EXEC GetStateMinimumPremium 'AK'

*/
-- ================================================

ALTER PROCEDURE GetStateMinimumPremium
(
	@StateCode CHAR(2)
)
AS 
BEGIN

	-- SET NOCOUNT ON added to prevent extra result sets from
	SET NOCOUNT ON;

	--SELECT StateCode,MinimumPremium As PremiumThreshold FROM StateMinimumPremium WHERE StateCode=@StateCode;
	SELECT T1.Id,T1.StateCode,MinimumPremium As PremiumThreshold FROM StateMaster T1, StateMinimumPremium T2 WHERE T1.Id=T2.StateId AND T1.StateCode=@StateCode;

END

