GO
/****** Object:  StoredProcedure [dbo].[GetInActiveUsersAndQuote]    Script Date: 07-04-2016 09:04:18 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
/*=============================================
 Author:		<Venkatesh>
 Create date: <31-Mar-2016>
 Description:	<Get Quotes for the inactive user and save for later mail not triggered. This procedure created
               for the task GUIN-76

exec [GetInActiveUsersAndQuote]
 =============================================*/
CREATE PROCEDURE [dbo].[GetInActiveUsersAndQuote] 
AS
BEGIN 
	select OrgUser.ID OrganizationUserDetailID, Qt.[QuoteNumber],OrgUser.EmailID
	from [dbo].[Quote] Qt 
	inner join [dbo].[OrganisationUserDetail] OrgUser on Qt.organizationuserdetailid=OrgUser.Id
	where OrgUser.IsActive=0 and OrgUser.IsSaveForLaterMailSent=0
END