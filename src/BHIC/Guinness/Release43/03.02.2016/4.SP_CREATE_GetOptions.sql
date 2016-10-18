CREATE PROCEDURE [dbo].[GetOptions] 
(
@TableName Varchar(256),
@LineOfBusinessID INT
)
AS
BEGIN 
	IF @TableName='PolicyChange'
	BEGIN
		SELECT Id,Options FROM PolicyChangeOptions WHERE IsActive=1 AND LineOfBusinessID=@LineOfBusinessID
	END
	ELSE IF @TableName='PolicyCancel'
	BEGIN
		SELECT 0 as Id,'--Please select appropriate cancellation reason--' as Options 
		UNION
		SELECT Id,Options FROM PolicyCancelOptions WHERE IsActive=1 AND LineOfBusinessID=@LineOfBusinessID
	END
END