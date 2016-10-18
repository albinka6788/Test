
/****** Object:  StoredProcedure [dbo].[InsertTemplates]    Script Date: 24-02-2016 17:48:32 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[InsertTemplates]
(
	@template VARCHAR(max),
	@logo VARCHAR(max)
)
AS 
	IF((SELECT Count(*) from dbo.tblTemplates where template =@template) = 0 )
	BEGIN
		INSERT INTO dbo.tblTemplates([Template],[Logo],[Status])
		VALUES ( @template,@logo,'Active')
	END
	ELSE
	BEGIN
		UPDATE dbo.tblTemplates
		SET Logo = @logo 
		WHERE Template = @template
	END 

GO
