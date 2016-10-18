
/****** Object:  StoredProcedure [dbo].[GetLandingPageUser]    Script Date: 14-03-2016 17:03:34 ******/
DROP PROCEDURE [dbo].[GetLandingPageUser]
GO


-- example exec [dbo].[GetLandingPageUser] "admin","admin"

CREATE PROCEDURE [dbo].[GetLandingPageUser]
	@username varchar(max),
	@password varchar(max)
AS
DECLARE @UserId bigint=0;
BEGIN
	
	SET NOCOUNT ON;
	SELECT * FROM [dbo].[tblLandingPageUsers] WHERE Username =@username and [password]= @password

END


GO


