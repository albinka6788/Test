/****** Object:  StoredProcedure [dbo].[GetlobStatusForState]    Script Date: 3/8/2016 12:17:43 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO


-- =============================================
-- Author:		Anuj Kumar Singh
-- Create date: 03/04/2016
-- Description:	Use to Get all available lob status
-- Use Exec GetlobStatusForState 
-- =============================================
CREATE PROCEDURE [dbo].[GetlobStatusForState]
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	SELECT st.Id,st.StateLobId,st.StatusId,ls.Status,st.EffectiveFrom,st.ExpiryOn 
	FROM StateLobStatus st 
	INNER JOIN LobStatus ls 
	ON st.StatusId= ls.Id

    WHERE st.EffectiveFrom <= getdate() 
	AND st.ExpiryOn >= getdate() 
	AND st.IsActive=1 and ls.IsActive=1

END

GO


