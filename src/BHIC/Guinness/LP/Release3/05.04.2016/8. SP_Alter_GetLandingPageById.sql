
/****** Object:  StoredProcedure [dbo].[GetLandingPageById]    Script Date: 14-03-2016 17:02:20 ******/
DROP PROCEDURE [dbo].[GetLandingPageById]
GO


-- =============================================
-- Author:		Navneet Kumar
-- Create date: 01/18/2015
-- Description:	Use to Get Landing Page Detail by Id for Edit
-- Use Exec GetLandingPage '12'
-- =============================================
Create PROCEDURE [dbo].[GetLandingPageById]
	@Id varchar(32)
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	Select * from LandingPageTransaction where Id=@Id AND IsDeleted = 0
END


GO


