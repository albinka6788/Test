
-- ================================================
/*
@Author		 : Krishna
@Description : This proc will add/update CTA messages
@CreatedOn   : 2015-Oct-23 

@Updation History : 
   @Update-0 : {date}, {by}, {description} 
   @Update-1 : {2016-MAY-13}, {Navneet}, {Increased message size form 100 to 200.} 
   
@ExampleCall :
EXEC [InsertOrUpdateCTAMessages] 10,'0d0a63a992fc44538adb249367fa6785','test1', 0

*/
-- ================================================
ALTER PROCEDURE [dbo].[InsertOrUpdateCTAMessages]
(
	@id BIGINT,
	@tokenId VARCHAR(32),
	@message VARCHAR(200) = null,
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



