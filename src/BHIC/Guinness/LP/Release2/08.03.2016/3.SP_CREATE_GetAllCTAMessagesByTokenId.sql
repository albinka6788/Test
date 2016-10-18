
GO

/****** Object:  StoredProcedure [dbo].[GetAllCTAMessagesByTokenId]    Script Date: 09-03-2016 12:22:13 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
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
