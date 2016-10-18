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

CREATE PROCEDURE GetStateMinimumPremium
(
	@StateCode CHAR(2)
)
AS 
BEGIN

	-- SET NOCOUNT ON added to prevent extra result sets from
	SET NOCOUNT ON;

	SELECT StateCode,MinimumPremium As PremiumThreshold FROM StateMinimumPremium WHERE StateCode=@StateCode;

END
