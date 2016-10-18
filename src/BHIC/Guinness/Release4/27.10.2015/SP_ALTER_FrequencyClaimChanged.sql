-- ================================================
/*
@Author      : Perm Pratap Singh Rajpurohit
@Description : This proc will return referral threshold claims value for supplied StateCode, ClassCode, Annual Payroll
@CreatedOn   : 2015-Sep-30 

@Updation History : 
   @Update-1 : {date}, {by}, {description} 

@ExampleCall : EXEC GetReferralThresholdClaims 'AK','8046','27300'

*/
-- ================================================

ALTER PROCEDURE GetReferralThresholdClaims
(
	 @StateCode			CHAR(2)
	,@ClassCode			VARCHAR(10)
	,@AnnualPayroll		NUMERIC(18,0)
)
AS 
BEGIN

	-- SET NOCOUNT ON added to prevent extra result sets from
	SET NOCOUNT ON;

	--Comment : Here local variable declaration & initialization
	BEGIN

		DECLARE @OrgActivityType			INT = 0;
		DECLARE @ReferralThresholdClaims	INT = 0;
		DECLARE @UnlimitedTotalPayrollMax	NUMERIC(18,0) = 99999999;

	END

	--Comment : Here STEP - 1 First get OrganizationActivityType based on StateCode, ClassCode
	SELECT @OrgActivityType = ISNULL(T2.OrganizationActivityType,0) FROM StateMaster T1, FrequencyClass T2 WHERE T1.Id=T2.StateId AND ClassCode=@ClassCode AND T2.IsActive=1 AND T1.StateCode=@StateCode;

	--Comment : Here STEP - 2 According business logic if this StateCode+ClassCode combination doesn't have any data then set it to 2 as Default 
	IF @OrgActivityType = 0
	BEGIN
		SET @OrgActivityType = 2;
	END

	--Comment : Here STEP - 3 Based on fetched OrganizationActivityType get ReferralThresholdClaims value for given AnnualPayroll
	IF @OrgActivityType > 0
	BEGIN
				
		SELECT @ReferralThresholdClaims = ISNULL(ThresholdNumberOfClaims,0) FROM FrequencyClaims WHERE (@AnnualPayroll BETWEEN TotalPayrollMin AND ISNULL(TotalPayrollMax,@UnlimitedTotalPayrollMax)) AND IsActive=1 And OrganizationActivityType=@OrgActivityType;
		
		--PRINT 'Reference Column Type : '+ CAST(@OrgActivityType AS VARCHAR);
		--PRINT 'Allowed Referral Threshold Claims : '+ CAST(@ReferralThresholdClaims AS VARCHAR);

		--Comment : Here return this value to calle method
		SELECT @ReferralThresholdClaims AS ReferralThresholdClaims;

	END

END

