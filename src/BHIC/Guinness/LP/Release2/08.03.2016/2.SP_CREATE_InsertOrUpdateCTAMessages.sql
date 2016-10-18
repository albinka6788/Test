/****** Object:  StoredProcedure [dbo].[InsertOrUpdateCTAMessages]    Script Date: 07-03-2016 13:34:36 ******/
DROP PROCEDURE [dbo].[InsertOrUpdateCTAMessages]
GO

/****** Object:  StoredProcedure [dbo].[InsertOrUpdateCTAMessages]    Script Date: 07-03-2016 13:34:36 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO



-- ================================================
/*
@Author      : Krishna
@Example call : exec [InsertOrUpdateCTAMessages] 10,"0d0a63a992fc44538adb249367fa6785","test1", 0
*/
-- ================================================
CREATE PROCEDURE [dbo].[InsertOrUpdateCTAMessages]
(
	@id BIGINT,
	@tokenId VARCHAR(32),
	@message VARCHAR(100),
	@returnId BIGINT OUTPUT	
)
AS 
	IF (@id=0) -- FOR INSERT OPERATION
		BEGIN
    		INSERT INTO dbo.CTAMessages([TokenId], [Message],[CreatedOn])
    		VALUES(@tokenId,@message,getDate())
			SELECT @returnId = SCOPE_IDENTITY();
		END
	ELSE
		BEGIN -- FOR UPDATE OPERATION
			UPDATE dbo.CTAMessages
			SET [Message]  = @message
			where Id=@id and TokenId=@tokenId
			SELECT @returnId = @id;
		ENd

GO


