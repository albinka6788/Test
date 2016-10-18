/****** Object:  StoredProcedure [dbo].[InsertCYBClassKeywords]    Script Date: 06-06-2016 21:37:21 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO


CREATE PROCEDURE [dbo].[InsertCYBClassKeywords]
(@ClassKeyword varchar(100))
AS
 BEGIN
	-- Check if the Description already exists
	If(NOT EXISTS(Select * from [dbo].[CYBClassKeywords] where ClassKeyword = @ClassKeyword))
		BEGIN		
			Insert into [dbo].[CYBClassKeywords] (ClassKeyword) values (@ClassKeyword)
		  END
 END


GO
