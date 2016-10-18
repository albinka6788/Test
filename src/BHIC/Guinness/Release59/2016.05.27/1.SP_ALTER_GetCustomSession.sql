/****** Object:  StoredProcedure [dbo].[GetApplicationCustomSession]    Script Date: 5/27/2016 3:45:33 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

-- ================================================
/*
@Author      : Perm Pratap Singh Rajpurohit
@Description : This proc will get application[Guinness] custom session for supplied QuoteId which will be used to show data on UI
@CreatedOn   : 2015-Oct-23 

@Updation History : 
   @Update-0 : {date}, {by}, {description} 
   @Update-1 : {2015-DEC-31}, {Prem}, {Erlier quote-id based session state fetch was running now its rectified to validate Guest/Register user quotes } 
   @Update-2 : {2016-MAY-26}, {Prem}, {As per GURU's suggestion made the changes } 
   @Update-3 : {2016-MAY-27}, {Guru}, { Fixed issue related to conversion of Quote Number } 

@ExampleCall : 
EXEC GetApplicationCustomSession 48306, 1

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

		SELECT QuoteId, SessionData
		FROM PurchaePathCustomSession PPCS
		LEFT JOIN Quote Q
			ON CONVERT(varchar, PPCS.QuoteId) = Q.QuoteNumber
		WHERE PPCS.QuoteId = @QuoteId
			AND ISNULL(Q.OrganizationUserDetailID,1) = @UserId

		--SELECT QuoteId,SessionData FROM PurchaePathCustomSession WHERE QuoteId=@QuoteId AND CreatedBy = @UserId  --Old Line
	END

END




