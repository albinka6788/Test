-- ================================================
/*
@Author      : Nishank Kumar Srivastava
@Description : This proc will return State Type based on State code provided
@CreatedOn   : 2015-Oct-16 

@Updation History : 
   @Update-1 : {date}, {by}, {description} 

@ExampleCall : EXEC GetStateType 'PA'

*/
-- ================================================

CREATE PROCEDURE [dbo].[GetStateType]
(
	@StateCode VARCHAR(9)
)
AS 
BEGIN

	-- SET NOCOUNT ON added to prevent extra result sets from
	SET NOCOUNT ON;

	SELECT IsGoodState AS StateType 
	FROM TypeOfState 
	WHERE StateCode=LTRIM(RTRIM(@StateCode ))
	AND CAST(ExpiryDate AS DATE) >= CAST(GETDATE() AS DATE);

END