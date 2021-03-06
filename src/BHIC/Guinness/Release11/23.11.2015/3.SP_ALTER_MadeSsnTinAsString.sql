-- ================================================
/*
@Author      : Anuj Kumar Singh
@Description : This proc will insert OraganisationUser Detail  into OraganisationUserDetail table
@CreatedOn   : 2015-oct-28

@Updation History : 
   @Update-0 : {date}, {by}, {description} 
   @Update-1 : {2015-Nov-17}, {Prem}, {Added one retrun parameter to insert dependent/reference data with this retruned FK_ID} 

*/
-- ================================================

ALTER PROCEDURE [dbo].[CreateOraganisationUserDetail] 
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
		DECLARE @ReturnInsertedRowId BIGINT = 0;
		DECLARE @SEQUENCEOrganisationUserDetailId BIGINT = 0;

		SELECT @SEQUENCEOrganisationUserDetailId = (NEXT VALUE FOR [SEQUENCEOrganisationUserDetail]);
	END

	--Comment : Here insert data and return id for further refernces
	BEGIN

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

	--Comment : Here set inserted row sequence id
	SET @ReturnInsertedRowId = @SEQUENCEOrganisationUserDetailId;

	END
	
	--Comment : Here finally return inserted row-id
	RETURN @ReturnInsertedRowId;

END      