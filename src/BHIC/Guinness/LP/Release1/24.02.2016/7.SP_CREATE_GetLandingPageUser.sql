/****** Object:  StoredProcedure [dbo].[GetLandingPageUser]    Script Date: 24-02-2016 17:47:50 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
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


