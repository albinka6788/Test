
/****** Object:  StoredProcedure [dbo].[GetAllCTAMessagesByTokenId]    Script Date: 14-03-2016 17:00:30 ******/
DROP PROCEDURE [dbo].[GetAllCTAMessagesByTokenId]
GO

-- =============================================
-- Author:		Krishna
-- Create date: 01/18/2015
-- Description:	Use to Get Landing Pages by search
-- Use Exec GetAllCTAMessagesByTokenId '0d0a63a992fc44538adb249367fa6785'
-- =============================================
CREATE PROCEDURE [dbo].[GetAllCTAMessagesByTokenId]
	@tokenId varchar(32)
AS
BEGIN
	SET NOCOUNT ON;
	Select Id from [dbo].[CTAMessages] where TokenId =@tokenId
END

GO


