-- ================================================
/*
@Author      : Perm Pratap Singh Rajpurohit
@Description : This proc will return MandatoryDeductible offering for hard states based on supplied State
@CreatedOn   : 2015-Oct-27 

@Updation History : 
   @Update-1 : {date}, {by}, {description} 

@ExampleCall : EXEC GetStateMandatoryDeductible 'AL'

*/
-- ================================================

CREATE PROCEDURE GetStateMandatoryDeductible
(
	@StateCode CHAR(2)
)
AS 
BEGIN

	-- SET NOCOUNT ON added to prevent extra result sets from
	SET NOCOUNT ON;

	--SELECT StateCode,MinimumPremium As PremiumThreshold FROM StateMinimumPremium WHERE StateCode=@StateCode;
	SELECT T1.Id,T1.StateCode,T2.StateId As PremiumThreshold FROM StateMaster T1, MandatoryDeductible T2 WHERE T1.Id=T2.StateId AND T2.IsActive=1 AND T1.StateCode=@StateCode;

END

