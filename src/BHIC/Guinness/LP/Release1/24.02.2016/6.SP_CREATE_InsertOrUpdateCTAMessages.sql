
/****** Object:  StoredProcedure [dbo].[InsertOrUpdateCTAMessages]    Script Date: 24-02-2016 17:48:14 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO


-- ================================================
/*
@Author      : Krishna
@Example call : exec [InsertOrUpdateCTAMessages] 1,"test","test"
*/
-- ================================================
CREATE PROCEDURE [dbo].[InsertOrUpdateCTAMessages]
(
	@id BIGINT,
	@tokenId VARCHAR(32),
	@message VARCHAR(100)
)
AS 
	IF (@id=0) -- FOR INSERT OPERATION
		BEGIN
    		INSERT INTO dbo.CTAMessages
    		(
    			[TokenId], [Message],[CreatedOn]
    		)
    		VALUES
    		(
				@tokenId,@message,getDate()
    		)
		END
	ELSE
		BEGIN -- FOR UPDATE OPERATION
			UPDATE dbo.CTAMessages
			SET [Message]  = @message
			where Id=@id and TokenId=@tokenId
		END



GO


