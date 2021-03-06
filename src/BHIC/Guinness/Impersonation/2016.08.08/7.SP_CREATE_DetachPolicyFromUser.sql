/*
@Author      : Veera Reddy
@Description : This proc will De-Link a Policy from Owner or Impersonating User.
@Createdon   : 2016/Aug/08
@Updation History :          
   @update-1 : 
@Example call : exec DetachPolicyFromUser 'Pk@gmail.com','CCMSS3'
*/
-- ================================================
CREATE PROCEDURE [dbo].[DetachPolicyFromUser]
(
@Email Varchar(50),
@PolicyCode varchar(10)
)
AS 
BEGIN
	BEGIN TRY	
		-- Get OrganizationUserDetailID
		DECLARE @OrganizationUserDetailID Int = 0	
		DECLARE @UserRoleId Int = 0	
		DECLARE @ResponseMessage VARCHAR(500) ='OK'
		Select @OrganizationUserDetailID = Id,@UserRoleId =UserRoleId FROM organisationuserdetail WHERE EMAILID=@Email

		IF(@OrganizationUserDetailID > 0)
			BEGIN 
				IF(@UserRoleId>1)
					BEGIN
						-- Get QuoteId
						Declare @QuoteId Int = 0
						Select @QuoteId = QuoteId from Policy WHERE PolicyNumber = @PolicyCode
			
						IF(@QuoteId > 0)
						BEGIN
							Declare @UserQuoteId Int = 0
							Declare @UserQuoteEndDate DateTime
							Declare @NewEndDate DateTime = GETDATE()

							SELECT @UserQuoteId = Id, @UserQuoteEndDate = EndDate FROM UserQuotes 
							WHERE OrganizationUserDetailID = @OrganizationUserDetailID AND QuoteId = @quoteId
					
							-- Check if Policy is assigned to User
							IF(@userQuoteId > 0)
							BEGIN
								IF(@UserQuoteEndDate IS NULL Or @UserQuoteEndDate > @NewEndDate)
								BEGIN 
									UPDATE UserQuotes SET EndDate  = @NewEndDate WHERE OrganizationUserDetailID = @OrganizationUserDetailID AND QuoteId = @quoteId
									SET @ResponseMessage = 'Policy is detached from the User'
								END				 
								ELSE
								BEGIN
									SET @ResponseMessage = 'Policy already detached from the user on ' + convert(varchar(50),@UserQuoteEndDate,120)
								END								
							END
							ELSE
							BEGIN
								--SET @ResponseMessage = 'Invalid User-Policy mapping'
								RAISERROR(N'Invalid User(%s)-Policy(%s) mapping',16,1,@Email,@PolicyCode);
							END
						END
						ELSE
						BEGIN
							--SET @ResponseMessage = 'Invalid PolicyCode: ' + @PolicyCode;
							RAISERROR(N'Invalid PolicyCode : %s',16,1,@PolicyCode);
						END						
					END
				ELSE
					BEGIN
						--SET @ResponseMessage = 'Primary user can not be detached from a policy';
						RAISERROR(N'Primary user can not be detached from a policy.',16,1);
					END				
			END
		ELSE
			BEGIN
				--SET @ResponseMessage = 'Invalid User: ' + @Email ;
				RAISERROR(N'Invalid User : %s.',16,1,@Email);		
				SELECT @ResponseMessage
			END
		SELECT @ResponseMessage
	END TRY
	BEGIN CATCH
	THROW;
	END CATCH
END
