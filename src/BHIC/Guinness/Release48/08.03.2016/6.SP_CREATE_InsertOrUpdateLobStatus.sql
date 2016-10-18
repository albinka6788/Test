/****** Object:  StoredProcedure [dbo].[InsertOrUpdateLobStatus]    Script Date: 3/8/2016 12:18:35 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

-- ================================================      
/*      
@Author      : Anuj Kumar Singh       
@Description : This procedure will update IsEmailverified in OrganisationUserDetail table
			   based on Email
@CreatedOn   : 2016-Mar-07       
@Updation History :       
   @Update-1 : 2016-Jan-6, Anuj, created
@ExampleCall : 
EXEC InsertOrUpdateLobStatus 
      
*/      
-- ================================================      
  
CREATE PROCEDURE [dbo].[InsertOrUpdateLobStatus]
(
@StateCode VARCHAR(2),
@LineOfBusinessName VARCHAR(50),
@Status VARCHAR(50),
@EffectiveFrom DATETIME,
@ExpiryOn DATETIME,
@IsActive BIT,
@CreatedDate DATETIME,
@CreatedBy INT,
@ModifiedDate DATETIME,
@ModifiedBy INT
)
AS       
BEGIN      
      
 -- SET NOCOUNT ON added to prevent extra result sets from      
SET NOCOUNT OFF;      
    
DECLARE @STATELOBID INT;
DECLARE @LOBSTATUSID INT;

SELECT  @STATELOBID =SLB.ID FROM StateLineOfBusinesses SLB INNER JOIN StateMaster SM ON SLB.StateId = SM.Id
INNER JOIN LineOfBusiness LB ON SLB.LineOfBusinessId = LB.Id
WHERE SM.StateCode=@StateCode AND LB.LineOfBusinessName = @LineOfBusinessName AND SM.IsActive =1 AND LB.IsActive=1 AND SLB.IsActive=1

SELECT @LOBSTATUSID=LobStatus.Id FROM LobStatus WHERE Status=@Status AND IsActive=1


IF(((@STATELOBID IS NOT NULL) OR (LEN(@STATELOBID) > 0)) AND ((@LOBSTATUSID IS NOT NULL) OR (LEN(@LOBSTATUSID) > 0)))

DECLARE @ReturnInsertedRowId BIGINT = 0;
DECLARE @SEQUENCEStateLobStatus BIGINT = 0;

SELECT @SEQUENCEStateLobStatus = (NEXT VALUE FOR [SEQUENCEStateLobStatus]);

INSERT INTO StateLobStatus 
VALUES(
 @SEQUENCEStateLobStatus
,@STATELOBID
,@LOBSTATUSID
,CONVERT(DATE,@EffectiveFrom)
,CONVERT(DATE,@ExpiryOn)
,@IsActive
,@CreatedDate
,@CreatedBy
,@ModifiedDate
,@ModifiedBy
)
  --Comment : Here set inserted row sequence id
	SET @ReturnInsertedRowId = @SEQUENCEStateLobStatus;

END 

--Comment : Here finally return inserted row-id
	RETURN @ReturnInsertedRowId;

GO


