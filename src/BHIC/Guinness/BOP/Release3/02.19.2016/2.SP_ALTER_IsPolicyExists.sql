/****** Object:  StoredProcedure [dbo].[IsPolicyExists]    Script Date: 19-02-2016 13:00:09 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

-- ================================================
/*
@Author      : Venkatesh
@Description : This procedure to validate whether the policy is already exists
@CreatedOn   : 2016-Jan-05 

@Updation History : 
   @Update-0 : 19-02-2016, Venkatesh, checking record exists by quote id 
   
@ExampleCall : 
EXEC IsPolicyExists '12345'

*/
-- ================================================

ALTER PROCEDURE [dbo].[IsPolicyExists]
(
	 @Option	INT, ---(1->Policy Number, 2->Quote Number)
	 @Value  VARCHAR(20)
)
AS 
BEGIN

	-- SET NOCOUNT ON added to prevent extra result sets from
	SET NOCOUNT OFF;

	--Comment : Here If record exists then return data
	IF @Option=2
		BEGIN
			SELECT Id FROM Policy WHERE QuoteID=(SELECT ID FROM QUOTE WHERE QuoteNumber=@Value)
		END
	ELSE
		BEGIN
	    	SELECT Id FROM Policy WHERE PolicyNumber=@Value
		END


END





