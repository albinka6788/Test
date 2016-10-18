
/****** Object:  StoredProcedure [dbo].[AddEditlandingPage]    Script Date: 14-03-2016 16:57:10 ******/
DROP PROCEDURE [dbo].[AddEditlandingPage]
GO

-- =============================================
-- Author:		Navneet Kumar
-- Create date: 01/18/2015
-- Description:	Use to add Landing Page information
-- =============================================
CREATE PROCEDURE [dbo].[AddEditlandingPage]
@lob varchar(3),
@state varchar(3),
@TransactionCounter bigint  =null,
--@logo varchar(200),
@title text = null,
@description text= null,
@btnText text = null,
@ctaBoxTitle text = null,
@header varchar(200) = null,
@footer varchar(200) = null,
@MainImage varchar(100),
@TemplateId int = null,
@Controller varchar(50) = null,
@ActionResult varchar(50) = null,
@TokenId varchar(32),
@Id bigint = null,
@CreatedBy bigint = 0,
@UpdatedBy bigint = 0,
@landingPageTransactionId BIGINT OUTPUT	
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	SET NOCOUNT ON;
	if(ISNULL(@Id,0)>0)
	begin
		UPDATE LandingPageTransaction 
		set lob=@lob,state=@state,TransactionCounter=@TransactionCounter,
		--logo=@logo,
		Title = @title,
		Description = @description,
		BTNText = @btnText,
		CTABoxTitle = @ctaBoxTitle,
		header=@header,footer=@footer,
		MainImage=@MainImage,TemplateId=@TemplateId,
		Controller=@Controller,ActionResult=@ActionResult,
		UpdatedBy=@UpdatedBy,
		UpdatedOn = GETDATE()
		where Id=@Id AND TokenId=@TokenId
		Select @landingPageTransactionId = @Id;
	end
	else
	begin
    -- Insert statements for procedure here
		Insert into LandingPageTransaction Select @lob,@state,@TransactionCounter,
		null,
		Title = @title,
		Description = @description,
		BTNText = @btnText,
		CTABoxTitle = @ctaBoxTitle,
		@header,@footer,@MainImage,@TemplateId,@Controller,@ActionResult,@TokenId,
	 	@CreatedBy, GETDATE(), @UpdatedBy, null,
		0
		Select @landingPageTransactionId = SCOPE_IDENTITY();
	end 
END 








GO


