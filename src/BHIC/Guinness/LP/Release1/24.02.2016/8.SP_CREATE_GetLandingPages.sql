/****** Object:  StoredProcedure [dbo].[GetLandingPages]    Script Date: 24-02-2016 17:47:36 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Author:		Navneet Kumar
-- Create date: 01/18/2015
-- Description:	Use to Get Landing Pages by search
-- Use Exec GetLandingPage 'd2552d60b92b4435829734ff87cae0bf'
-- =============================================
CREATE PROCEDURE [dbo].[GetLandingPages]
	@filter varchar(32) = null
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	Select * from LandingPageTransaction where (isnull(@filter,'')='' or state=@filter) ORDER BY CreatedOn DESC
END


GO


