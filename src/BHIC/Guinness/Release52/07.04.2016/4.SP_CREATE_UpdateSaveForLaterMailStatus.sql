GO
/****** Object:  StoredProcedure [dbo].[UpdateSaveForLaterMailStatus]    Script Date: 07-04-2016 09:05:38 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
/*=============================================
 Author:		<Venkatesh>
 Create date: <31-Mar-2016>
 Description:  Update save for later mail status once the mail sent. This procedure created
               for the task GUIN-76

exec UpdateSaveForLaterMailStatus 2
 =============================================*/
CREATE PROCEDURE [dbo].[UpdateSaveForLaterMailStatus] 
(
 @OrganizationUserDetailID int,
 @returnId BIGINT OUTPUT	
)
AS
BEGIN 
    UPDATE [OrganisationUserDetail] SET IsSaveForLaterMailSent=1 WHERE ID=@OrganizationUserDetailID;
	SELECT @returnId = @OrganizationUserDetailID;
END