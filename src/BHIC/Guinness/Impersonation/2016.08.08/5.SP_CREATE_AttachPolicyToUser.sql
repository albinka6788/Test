/*
@Author      : Sreeram
@Description : This proc will Link or Re-link a Policy to the Owner or Impersonating User.
@Createdon   : 2016/Aug/08
@Updation History :          
   @update-1 : Veera Reddy, Implemented conditions in Procedure.
   @update-2 :  Amit - Added convertion and conditions in Procedure.
   @update-3 : Updated CreatedBy and UpdatedBy to 0 as it is system updated.
@Example call : exec AttachPolicyToUser 'Pk@gmail.com','CCMSS3','2016-08-06 01:34:20.000','2016-08-06 03:34:20.753'
*/
-- ================================================
CREATE PROCEDURE [dbo].[AttachPolicyToUser]
(
@Email Varchar(100),
@PolicyCode Varchar(50),
@StartDate DateTime = NULL,
@EndDate DateTime = NULL
)
AS 
BEGIN	
	BEGIN TRY	
		IF(@StartDate IS NULL)
		BEGIN
		   SET @StartDate = GETDATE()
		END
		-- Get OrganizationUserDetailID
		DECLARE @OrganizationUserDetailID Int = 0	
		DECLARE @UserRoleId Int = 0	
		DECLARE @ResponseMessage VARCHAR(500) ='OK'
		Select @OrganizationUserDetailID = Id, @UserRoleId = UserRoleId FROM organisationuserdetail WHERE EMAILID = @Email

		IF(@OrganizationUserDetailID > 0)		
			BEGIN 
			    IF(@UserRoleId>1)
					BEGIN
						-- Get QuoteId
						DECLARE @QuoteId Int = 0
						Select @QuoteId = QuoteId from Policy WHERE PolicyNumber = @PolicyCode
			
						IF(@QuoteId > 0)
							BEGIN
							DECLARE @UserQuoteId Int = 0
							DECLARE @UserQuoteStartDate DateTime
							DECLARE @UserQuoteEndDate  DateTime
							DECLARE @PolicyOwnerId  Int = 0	

							--select @quoteId;
							SELECT @UserQuoteId= Id, @UserQuoteStartDate = StartDate, @UserQuoteEndDate = EndDate FROM UserQuotes 
							WHERE OrganizationUserDetailID = @OrganizationUserDetailID AND QuoteId = @quoteId
							
							--select @PolicyOwnerId;
							SELECT @PolicyOwnerId= O.Id from organisationuserdetail O inner join UserQuotes Q on O.Id = Q.OrganizationUserDetailID 
									where O.UserRoleId=1 and Q.QuoteId=@quoteId;
							
							-- Expire already attached policies for which primary user is different than the user of current policy which is being attached 
							UPDATE UserQuotes SET EndDate = GETDATE() WHERE OrganizationUserDetailID = @OrganizationUserDetailID 
							AND QuoteId not in (SELECT QuoteId from UserQuotes where OrganizationUserDetailID = @PolicyOwnerId);

							IF(@UserQuoteId > 0)			
								BEGIN	
							
									IF (@UserQuoteEndDate IS NULL)
									BEGIN 				
										SET @ResponseMessage = 'Policy is already attached to the user with no end date.'
										--RAISERROR(N'Policy : %s is already attached to the user :%s with no end date.',16,1,@PolicyCode,@Email);
									END
									ELSE IF(@EndDate IS NULL)
									BEGIN
										UPDATE USERQUOTES SET EndDate = @EndDate WHERE Id = @UserQuoteId
										SET @ResponseMessage = 'Policy is now attached to the User with no Expiration Date'
										--RAISERROR(N'Policy : %s is now attached to the user :%s with no end date.',16,1,@PolicyCode,@Email);
									END
									ELSE
									BEGIN
										UPDATE USERQUOTES SET EndDate = @EndDate WHERE Id = @UserQuoteId
										SET @ResponseMessage = 'Policy is now attached to the User till ' + convert(varchar(50),@EndDate,120)										
									END
								END
							ELSE
								BEGIN
									-- INSERT INTO UserQuote
									INSERT INTO UserQuotes
									(
									Id,
									OrganizationUserDetailID, QuoteId,
									StartDate, EndDate,	CreatedDate, CreatedBy,	ModifiedDate, ModifiedBy
									)
									VALUES
									(
									NEXT VALUE FOR SEQUENCEUserQuote,
									@organizationUserDetailID, @quoteId,
									@StartDate, @EndDate, GetDate(), 0, GetDate(), 0
									);
									SET @ResponseMessage = 'Policy is now attached to the User';
								END
							END
						ELSE
							BEGIN
							--SET @ResponseMessage = 'Invalid PolicyCode'
							RAISERROR(N'Invalid PolicyCode : %s',16,1,@PolicyCode);
						END							
					END
					ELSE
					BEGIN
						--SET @ResponseMessage = 'Primary user can not be attached to a policy';
						RAISERROR(N'Primary user can not be attached to a policy.',16,1);
				END
			END	
		ELSE
			BEGIN
				--SET @ResponseMessage = 'Invalid User'
				RAISERROR(N'Invalid User : %s.',16,1,@Email);				
			END
		SELECT @ResponseMessage
	END TRY
	BEGIN CATCH				
		THROW; 
	END CATCH	
END