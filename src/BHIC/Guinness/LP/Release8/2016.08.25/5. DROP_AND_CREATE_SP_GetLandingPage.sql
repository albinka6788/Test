
/****** Object:  StoredProcedure [dbo].[GetLandingPage]    Script Date: 29-08-2016 14:33:48 ******/
DROP PROCEDURE [dbo].[GetLandingPage]
GO

/****** Object:  StoredProcedure [dbo].[GetLandingPage]    Script Date: 29-08-2016 14:33:48 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Author:		Navneet Kumar
-- Create date: 01/18/2015
-- Description:	Use to Get Landing Page Detail
-- Use Exec GetLandingPage 'e4df1bf5574846cba1495c9e6900c3f6'
-- =============================================
CREATE PROCEDURE [dbo].[GetLandingPage]
	@AdId varchar(max)
AS

BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;
	SELECT 
		LPT.Id,
		LPT.Lob,
		LPT.State,
		LPT.Transactioncounter,
		LPT.PageName, 
		LPT.Heading, 
		LPT.SubHeading, 
		LPT.ProductName, 
		LPT.ProductHighlight, 
		LPT.Btntext,
		LPT.CalloutText,
		LPT.MainImage, 
		LPT.TokenId,
		LPT.CreatedOn, 
		TEMP.Id as TemplateId,
		'Content/Template' + CONVERT(varchar,Temp.Id) + '.css' as TemplateCss,
		TEMP.Logo,
		isnull(LPT.IsDeployed,0) as IsDeployed 
	FROM LANDINGPAGETRANSACTION LPT INNER JOIN DBO.TBLTEMPLATES TEMP ON LPT.TEMPLATEID=TEMP.ID
	WHERE LPT.TokenId= @AdId AND IsDeleted = 0

	SELECT CTAM.Id, CTAM.TokenId, CTAM.[Message], CTAM.CreatedOn
	FROM CTAMessages CTAM INNER JOIN DBO.LANDINGPAGETRANSACTION LPT ON CTAM.TokenId=LPT.TokenId AND CTAM.TokenId= @AdId
	
END

GO


