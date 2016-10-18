-- ================================================
/*
@Author      : Perm Pratap Singh Rajpurohit
@Description : This proc will get application[Guinness] custom session for supplied QuoteId which will be used to show data on UI
@CreatedOn   : 2015-Oct-23 

@Updation History : 
   @Update-0 : {date}, {by}, {description} 

@ExampleCall : 
EXEC GetApplicationCustomSession '14849'

--Select * From PurchaePathCustomSession

*/
-- ================================================

CREATE PROCEDURE [dbo].[GetApplicationCustomSession]
(
	 @QuoteId			BIGINT
)
AS 
BEGIN

	-- SET NOCOUNT ON added to prevent extra result sets from
	SET NOCOUNT OFF;

	--Comment : Here If record exists then return data
	BEGIN
		SELECT QuoteId,SessionData FROM PurchaePathCustomSession WHERE QuoteId=@QuoteId
	END

END




GO


