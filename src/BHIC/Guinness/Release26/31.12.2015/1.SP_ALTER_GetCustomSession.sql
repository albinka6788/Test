-- ================================================
/*
@Author      : Perm Pratap Singh Rajpurohit
@Description : This proc will get application[Guinness] custom session for supplied QuoteId which will be used to show data on UI
@CreatedOn   : 2015-Oct-23 

@Updation History : 
   @Update-0 : {date}, {by}, {description} 
   @Update-1 : {31-DEC-2015}, {Prem}, {Erlier quote-id based session state fetch was running now its rectified to validate Guest/Register user quotes } 

@ExampleCall : 
EXEC GetApplicationCustomSession '14214'

--Select * From PurchaePathCustomSession

*/
-- ================================================

ALTER PROCEDURE [dbo].[GetApplicationCustomSession]
(
	 @QuoteId			BIGINT,
	 @UserId			INT
)
AS 
BEGIN

	-- SET NOCOUNT ON added to prevent extra result sets from
	SET NOCOUNT OFF;

	--Comment : Here If record exists then return data
	BEGIN
		SELECT QuoteId,SessionData FROM PurchaePathCustomSession WHERE QuoteId=@QuoteId AND CreatedBy = @UserId
	END

END




