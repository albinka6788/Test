-- ================================================
/*
@Author      : Perm Pratap Singh Rajpurohit
@Description : This proc will get application[Guinness] custom session for supplied QuoteId which will be used to show data on UI
@CreatedOn   : 2015-Oct-23 

@Updation History : 
   @Update-0 : {date}, {by}, {description} 
   @Update-1 : {2015-DEC-31}, {Prem}, {Erlier quote-id based session state fetch was running now its rectified to validate Guest/Register user quotes } 
   @Update-2 : {2016-MAY-26}, {Prem}, {As per GURU's suggestion made the changes } 


@updated By Krishna,

  1) Added isdeleted condition to handled expired saved quotes link
  2) Handled QuoteId with Policynumber 1:1 condition i.e with respect one Quote Id one Policy should exists

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
			SELECT PPCS.QuoteId, SessionData
			FROM PurchaePathCustomSession PPCS
			LEFT JOIN Quote Q ON PPCS.QuoteId = Q.QuoteNumber 
			LEFT JOIN Policy P ON P.QuoteId = Q.Id
			WHERE PPCS.QuoteId = @QuoteId
				AND ISNULL(Q.OrganizationUserDetailID,1) = @UserId 
				AND ISNULL(Q.isDeleted, 0) = 0
				AND P.id IS NULL
		END
END



