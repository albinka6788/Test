-- ================================================
/*
@Author      : Perm Pratap Singh Rajpurohit
@Description : This proc will add/update premium-factor/xMod-factor for supplied Fein
@CreatedOn   : 2015-Sep-30 

@Updation History : 
   @Update-0 : {date}, {by}, {description} 
   @Update-1 : {2015-10-13}, {Prem}, {Removed identity insert with DBA created sequence number for primary key column} 

@ExampleCall : 
EXEC UpdateFeinXModFactor '123456789', '0.75', '2015-12-31'

*/
-- ================================================

CREATE PROCEDURE UpdateFeinXModFactor
(
	 @FeinNumber		VARCHAR(9)
	,@XModValue			FLOAT
	,@ExpiryDate		DATETIME
)
AS 
BEGIN

	-- SET NOCOUNT ON added to prevent extra result sets from
	SET NOCOUNT OFF;

	--Comment : Here If record exists then update otherwise insert new one
	IF NOT EXISTS(SELECT Fein,PremiumFactor FROM FeinPremiumFactor WHERE Fein=@FeinNumber)
	BEGIN		
		INSERT INTO FeinPremiumFactor(Id,Fein,PremiumFactor,EffectiveDate,ExpiryDate) VALUES((NEXT VALUE FOR SEQUENCEFeinPremiumFactor),@FeinNumber,@XModValue,GETDATE(),@ExpiryDate);
	END
	ELSE
	BEGIN
		UPDATE FeinPremiumFactor SET Fein = @FeinNumber, PremiumFactor = @XModValue, EffectiveDate = GETDATE()+1, ExpiryDate = @ExpiryDate  WHERE Fein=@FeinNumber;
	END

END


