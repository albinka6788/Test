
-- =============================================
-- Author:		Navneet Kumar
-- Create date: 01/18/2015
-- Description:	Use to Get Landing Page Detail
-- Use Exec GetLandingPage '0d0a63a992fc44538adb249367fa6785'
/* Change History
{
	Navneet - 04/07/2016 - Added IsDeployed for LP deployment
}
*/
-- =============================================
ALTER PROCEDURE [dbo].[GetLandingPage]
	@AdId varchar(max)
AS

BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;
	SELECT 
		LPT.ID,
		LPT.LOB,
		LPT.STATE,
		LPT.TRANSACTIONCOUNTER,
		LPT.Title, 
		LPT.Description, 
		LPT.BTNText,
		LPT.CTABoxTitle,
		LPT.MAINIMAGE, 
		LPT.TOKENID,
		LPT.CREATEDON, 
		TEMP.Id as TemplateId,
		'Content/Template' + CONVERT(varchar,Temp.Id) + '.css' as TemplateCss,
		TEMP.LOGO,
		isnull(LPT.IsDeployed,0) as IsDeployed 
	FROM LANDINGPAGETRANSACTION LPT INNER JOIN DBO.TBLTEMPLATES TEMP ON LPT.TEMPLATEID=TEMP.ID
	WHERE LPT.TokenId= @AdId AND IsDeleted = 0

	SELECT CTAM.Id, CTAM.TokenId, CTAM.[Message], CTAM.CreatedOn
	FROM CTAMessages CTAM INNER JOIN DBO.LANDINGPAGETRANSACTION LPT ON CTAM.TokenId=LPT.TokenId AND CTAM.TokenId= @AdId
	
END








