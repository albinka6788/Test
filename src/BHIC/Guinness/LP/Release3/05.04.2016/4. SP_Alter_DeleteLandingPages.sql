
/****** Object:  StoredProcedure [dbo].[DeleteLandingPages]    Script Date: 14-03-2016 17:00:05 ******/
DROP PROCEDURE [dbo].[DeleteLandingPages]
GO

-- =============================================
-- Author:		Krishna
-- Create date: 01/18/2015
-- Description:	Use to Delete the Landing Pages
-- Use Exec DeleteLandingPages '1,2' , 1
-- =============================================
CREATE PROCEDURE [dbo].[DeleteLandingPages]
	@ids varchar(max),
	@updatedBy bigint
AS
declare @query varchar(max)
BEGIN
	SET NOCOUNT ON;
	SET @query = 'Update [dbo].[LandingPageTransaction] SET IsDeleted = 1  WHERE Id IN (' + @ids  +')'
	EXEC (@query)
	Print @query
END


GO


