-- ================================================
/*
@Author      : Perm Pratap Singh Rajpurohit
@Description : This proc will add/update UserQuotes object to allow 1:M linking between 1QuoteId:ManyUserId
@CreatedOn   : 2016-Aug-10 

@Updation History : 
   @Update-0 : {date}, {by}, {description} 

@ExampleCall : 
EXEC MaintainUserQuotesLinking '14214',1,NULL,1,NULL,1,0

--Select Top 2 * From UserQuotes Order By Id Desc
--Select * From UserQuotes Where QuoteId = 14214

*/
-- ================================================

ALTER PROCEDURE [dbo].[MaintainUserQuotesLinking]
(
	 @QuoteId							BIGINT
	,@OrganizationUserDetailID			BIGINT
	,@CreatedDate						DATETIME		= GETDATE
	,@CreatedBy							INT				= 1
	,@ModifiedDate						DATETIME		= GETDATE
	,@ModifiedBy						INT				= 1
	,@UpdateOnly						BIT				= 0
)
AS 
BEGIN

	-- SET NOCOUNT ON added to prevent extra result sets from
	SET NOCOUNT OFF;

	--Comment : Here If record exists then update otherwise insert new one
	IF @UpdateOnly = 0 AND NOT EXISTS(SELECT QuoteId FROM UserQuotes WHERE QuoteId=@QuoteId AND OrganizationUserDetailID=@OrganizationUserDetailID) 
	BEGIN		
		INSERT INTO UserQuotes(Id,QuoteId,OrganizationUserDetailID,StartDate,CreatedDate,CreatedBy,ModifiedDate,ModifiedBy) VALUES
		((NEXT VALUE FOR SEQUENCEUserQuote),@QuoteId,@OrganizationUserDetailID,GETDATE(),GETDATE(),@CreatedBy,GETDATE(),@ModifiedBy);

		--PRINT 'INSERTED';
	END
	ELSE
	BEGIN
		UPDATE UserQuotes SET 
		 ModifiedDate=ISNULL(@ModifiedDate,GETDATE())
		,ModifiedBy=@ModifiedBy
		,OrganizationUserDetailID = @OrganizationUserDetailID 
		WHERE QuoteId=@QuoteId AND (OrganizationUserDetailID IS NULL OR OrganizationUserDetailID = 0);

		--PRINT 'UPDATED';
	END

END





