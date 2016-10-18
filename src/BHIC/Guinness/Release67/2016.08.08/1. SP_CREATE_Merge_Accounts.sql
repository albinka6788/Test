SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

/* =============================================
   Author:		Amit Kumar
   Create date: 2016-Aug-04
   Description:	Merge Accounts


@Updation History : 
   @Update-1 : {date}, {by}, {description} 


   ================================================*/

CREATE PROCEDURE [dbo].[MergeAccounts]
(
@OlDEmailID varchar(250),	
@NewemailID varchar(250)
)
AS
BEGIN
Declare @Old_userID int
Declare @New_userID int
Declare @New_addressId int
DECLARE @UserIdUpdated BIGINT=0;
Begin Transaction t1
-- Old User Account from which policy to be removed and account to be deleted
select @Old_userID = Id from OrganisationUserDetail where EmailID = @OlDEmailID;
-- New User Account in which policy to be added 
select @New_userID = Id from OrganisationUserDetail where EmailID = @NewemailID;
-- New User Account AddressId to be fetched and updated
select @New_addressId = Id from OraganisationAddress where OrganizationID = @New_userID;

	BEGIN TRY	
			if(@Old_userID>0)
				Begin
					SET NOCOUNT OFF;				
					Update Quote set OrganizationUserDetailID = @New_userID, OrganizationAddressID = @New_addressId 
								where OrganizationUserDetailID = @Old_userID	
					select @UserIdUpdated = @New_userID;
				End			
	END TRY
    BEGIN CATCH
		ROLLBACK TRANSACTION T1
		RETURN Error_Message()   
	END CATCH	
	COMMIT TRANSACTION T1  	
	RETURN @UserIdUpdated
END  
GO


