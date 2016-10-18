-- ================================================
/*
@Author      : Gurupreet Singh
@Description : This proc will return PolicyChangeOptions based on supplied LOB-Id
@CreatedOn   : 2015-Oct-27 

@Updation History : 
   @Update-1 : {date}, {by}, {description} 

@ExampleCall : EXEC GetPolicyChangeOptions 1

*/
-- ================================================

CREATE PROCEDURE [dbo].[GetPolicyChangeOptions] 
(
	@LineOfBusinessID INT
)
AS
BEGIN 
	SELECT Id,Options FROM PolicyChangeOptions WHERE IsActive=1 AND LineOfBusinessID=@LineOfBusinessID
END
GO


