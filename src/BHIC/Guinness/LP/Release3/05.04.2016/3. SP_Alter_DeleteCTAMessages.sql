
/****** Object:  StoredProcedure [dbo].[DeleteCTAMessages]    Script Date: 14-03-2016 16:58:47 ******/
DROP PROCEDURE [dbo].[DeleteCTAMessages]
GO

-- ================================================
/*
@Author      : Krishna
@Example call : exec [DeleteCTAMessages] "16,13","78852eda385d49cc882162a35cf0c29b"
*/
-- ================================================
CREATE PROCEDURE [dbo].[DeleteCTAMessages]
(
	@existsIds VARCHAR(max),
	@tokenId VARCHAR(32)
)
AS 
declare @query varchar(max)
BEGIN
    SET @query ='DELETE FROM dbo.CTAMessages WHERE Id NOT IN' 
	SET @query = @query  + '('+ @existsIds + ') AND TokenId=''' + @tokenId + ''''
	EXEC (@query)
	Print @query
END


GO


