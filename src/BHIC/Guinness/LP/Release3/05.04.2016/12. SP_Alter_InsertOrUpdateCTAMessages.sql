
/****** Object:  StoredProcedure [dbo].[InsertOrUpdateCTAMessages]    Script Date: 14-03-2016 17:04:40 ******/
DROP PROCEDURE [dbo].[InsertOrUpdateCTAMessages]
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
	@message VARCHAR(100) = null,
	@createdBy BIGINT = 0,
	@updatedBy BIGINT = 0,
	@returnId BIGINT OUTPUT	
)
AS 
	IF (@id=0) -- FOR INSERT OPERATION
		BEGIN
    		INSERT INTO dbo.CTAMessages([TokenId], [Message],[CreatedBy],[CreatedOn])
    		VALUES(@tokenId,@message,@createdBy,GETDATE())
			SELECT @returnId = SCOPE_IDENTITY();
		END
	ELSE
		BEGIN -- FOR UPDATE OPERATION
			UPDATE dbo.CTAMessages
			SET [Message]  = @message,
			[UpdatedBy] = @updatedBy,
			[UpdatedOn]= GETDATE()
			where Id=@id and TokenId=@tokenId
			SELECT @returnId = @id;
		ENd



GO


