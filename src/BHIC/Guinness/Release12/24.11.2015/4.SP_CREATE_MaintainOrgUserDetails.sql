
-- ================================================
/*
@Author      : Prem Pratap Singh
@Description : This proc will insert OraganisationUser detail if not exists otherwise will update details
@CreatedOn   : 2015-Nov-24

@Updation History : 
   @Update-0 : {date}, {by}, {description} 

*/
-- ================================================

CREATE PROCEDURE [dbo].[MaintainOraganisationUserDetail] 
(
	 @OrganizationName VARCHAR(200)
	,@EmailID          VARCHAR(150)
	,@Password         VARCHAR(256)
	,@Tin              VARCHAR(256)
	,@Ssn              VARCHAR(256)
	,@Fein             VARCHAR(256)
	,@IsActive         BIT    
	,@CreatedDate		 DATETIME
	,@CreatedBy		 INT
	,@ModifiedDate	 DATETIME
	,@ModifiedBy		 INT
	,@FirstName        VARCHAR(255)
	,@LastName         VARCHAR(255)
	,@PolicyCode       VARCHAR(50)
	,@PhoneNumber      BIGINT
)	
AS 	
BEGIN

	-- SET NOCOUNT ON added to prevent extra result sets from
	SET NOCOUNT OFF;

	--Coment : Here local variables declaration and initialization 
	BEGIN
		DECLARE @ReturnRowId BIGINT = 0;
		DECLARE @SEQUENCEOrganisationUserDetailId BIGINT = 0;		
	END

	--Comment : Here insert/update data and return id for further refernces
	BEGIN

	--Comment : Here If record exists then update otherwise insert new one
	IF NOT EXISTS(SELECT Id FROM OrganisationUserDetail WHERE EmailID=@EmailID)
	BEGIN
		--Comment : Here first get SEQUNCE id for table
		SELECT @SEQUENCEOrganisationUserDetailId = (NEXT VALUE FOR [SEQUENCEOrganisationUserDetail]);

		--Comment : Insert record into table
		INSERT INTO [dbo].[OrganisationUserDetail]
		(
			 [Id]
			,[OrganizationName]
			,[EmailID]
			,[Password]
			,[Tin]
			,[Ssn]
			,[Fein]
			,[IsActive]
			,[CreatedDate]
			,[CreatedBy]
			,[ModifiedDate]
			,[ModifiedBy]
			,[FirstName]
			,[LastName]
			,[PolicyCode]
			,[PhoneNumber]
		)
		VALUES
		(
			 @SEQUENCEOrganisationUserDetailId
			,@OrganizationName
			,@EmailID         
			,@Password        
			,@Tin             
			,@Ssn             
			,@Fein            
			,@IsActive        
			,@CreatedDate		
			,@CreatedBy		
			,@ModifiedDate	
			,@ModifiedBy		
			,@FirstName       
			,@LastName        
			,@PolicyCode      
			,@PhoneNumber    
		)
	END
	ELSE
	BEGIN
		--Comment : Here first get SEQUNCE id for table (Becoz this is update get it by EmailId)
		SELECT @SEQUENCEOrganisationUserDetailId = Id FROM OrganisationUserDetail WHERE EmailID=@EmailID;

		UPDATE OrganisationUserDetail SET 
		 OrganizationName  = @OrganizationName       
		,Password          = @Password        
		,Tin               = @Tin             
		,Ssn               = @Ssn             
		,Fein              = @Fein                 
		,ModifiedDate	   = @ModifiedDate	
		,ModifiedBy		   = @ModifiedBy		
		,FirstName         = @FirstName       
		,LastName          = @LastName            
		,PhoneNumber	   = @PhoneNumber 
		WHERE EmailID=@EmailID;
	END

	--Comment : Here set inserted row sequence id
	SET @ReturnRowId = @SEQUENCEOrganisationUserDetailId;

	END
	
	--Comment : Here finally return inserted row-id
	RETURN @ReturnRowId;

END      