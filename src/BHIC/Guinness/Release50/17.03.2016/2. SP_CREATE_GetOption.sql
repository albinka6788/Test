/****** Object:  StoredProcedure [dbo].[GetOptions]    Script Date: 17-03-2016 4:35:53 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[GetOption] 
(
@TableName Varchar(256),
@LineOfBusinessID INT,
@id int
)
AS
BEGIN 
	IF @TableName='PolicyChange'
	BEGIN
		SELECT Id,Options FROM PolicyChangeOptions WHERE IsActive=1 AND LineOfBusinessID=@LineOfBusinessID and Id=@id
	END
	ELSE IF @TableName='PolicyCancel'
	BEGIN
		SELECT Id,Options FROM PolicyCancelOptions WHERE IsActive=1 AND LineOfBusinessID=@LineOfBusinessID and Id=@id
	END
END