
/****** Object:  StoredProcedure [dbo].[InsertTemplates]    Script Date: 14-03-2016 17:05:19 ******/
DROP PROCEDURE [dbo].[InsertTemplates]
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


