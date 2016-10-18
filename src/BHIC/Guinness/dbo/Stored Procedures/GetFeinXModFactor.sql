-- ================================================
/*
@Author      : Perm Pratap Singh Rajpurohit
@Description : This proc will return premium-factor/xMod-factor for supplied Fein
@CreatedOn   : 2015-Sep-29 

@Updation History : 
   @Update-1 : {date}, {by}, {description} 

@ExampleCall : EXEC GetFeinXModFactor '123456789'

*/
-- ================================================

CREATE PROCEDURE GetFeinXModFactor
(
	@FeinNumber VARCHAR(9)
)
AS 
BEGIN

	-- SET NOCOUNT ON added to prevent extra result sets from
	SET NOCOUNT ON;

	SELECT Fein,PremiumFactor AS XModValue FROM FeinPremiumFactor WHERE Fein=@FeinNumber AND CAST(ExpiryDate AS DATE) >= CAST(GETDATE() AS DATE);

END
