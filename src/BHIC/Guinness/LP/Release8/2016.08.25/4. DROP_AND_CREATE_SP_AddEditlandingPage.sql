/****** Object:  StoredProcedure [dbo].[AddEditlandingPage]    Script Date: 29-08-2016 14:31:56 ******/
DROP PROCEDURE [dbo].[AddEditlandingPage]
GO

/****** Object:  StoredProcedure [dbo].[AddEditlandingPage]    Script Date: 29-08-2016 14:31:56 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO



-- =============================================
-- Author:		Krishna
-- Create date: 18/08/2016
-- Description:	Use to add Landing Page information
-- =============================================
CREATE PROCEDURE [dbo].[AddEditlandingPage]
@lob varchar(3),
@state varchar(3),
@transactionCounter bigint  =null,
--@logo varchar(200),
@pageName text = null,
@heading text = null,
@subheading text= null,
@productName text = null,
@productHighlight text= null,
@btnText text = null,
@calloutText text = null,
@mainImage varchar(100),
@templateId int = null,
@controller varchar(50) = null,
@actionResult varchar(50) = null,
@tokenId varchar(32),
@id bigint = null,
@createdBy bigint = 0,
@updatedBy bigint = 0,
@isDeployed bit=0,
@landingPageTransactionId BIGINT OUTPUT	
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	SET NOCOUNT ON;
	if(ISNULL(@Id,0)>0)
	begin
		UPDATE LandingPageTransaction 
		set lob=@lob,state=@state,
		TransactionCounter=@transactionCounter,
		PageName = @pageName,
		Heading = @heading,
		SubHeading = @subheading,
		ProductName = @productName,
		ProductHighlight = @productHighlight,
		BTNText = @btnText,
		CalloutText = @calloutText,
		MainImage=@mainImage,
		TemplateId=@templateId,
		Controller=@controller,
		ActionResult=@actionResult,
		UpdatedBy=@updatedBy,
		UpdatedOn = GETDATE(),
		IsDeployed=@isDeployed
		where Id=@id AND TokenId=@tokenId
		Select @landingPageTransactionId = @id;
	end
	else
	begin
    -- Insert statements for procedure here
		Insert into LandingPageTransaction 
		values(@lob,@state,@transactionCounter,Null,@pageName,@heading,@subheading,
		@productName,@productHighlight,@btnText, @calloutText,
		@mainImage,@templateId,@controller,@actionResult,@tokenId,
	 	@createdBy, GETDATE(), @updatedBy, null,
		0,@isDeployed)
		Select @landingPageTransactionId = SCOPE_IDENTITY();
	end 
END 



GO


