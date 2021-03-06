
-- ================================================
/*
@Author      : Prem Pratap Singh
@Description : This proc will insert OraganisationAddress detail if not exists otherwise will update details
@CreatedOn   : 2015-Nov-24

@Updation History : 
   @update-1 : 2016-Apr-14, Gurpreet, Updation not required here for user Address in the proc

*/
-- ================================================

ALTER PROCEDURE [dbo].[MaintainOraganisationAddress] 
(
	 @OrganizationID      INT
	,@Address1            VARCHAR(200)
	,@Address2            VARCHAR(200)		= NULL
	,@Address3            VARCHAR(200)		= NULL
	,@City                VARCHAR(200)		= NULL
	,@County              VARCHAR(200)		= NULL
	,@StateCode           CHAR(2)
	,@ZipCode             INT
	,@CountryID           INT					= NULL
	,@IsCorporateAddress  BIT
	,@ContactName         VARCHAR(200)
    ,@ContactNumber1      BIGINT
    ,@ContactNumber2      BIGINT				= NULL
	,@Fax                 BIGINT				= NULL
	,@IsActive            BIT    
	,@CreatedDate         DATETIME
	,@CreatedBy           INT
	,@ModifiedDate        DATETIME
	,@ModifiedBy          INT

)
AS 	

BEGIN

	-- SET NOCOUNT ON added to prevent extra result sets from
	SET NOCOUNT OFF;

	--Coment : Here local variables declaration and initialization 
	BEGIN
		DECLARE @ReturnRowId BIGINT = 0;
		DECLARE @SEQUENCEOraganisationAddressId BIGINT = 0;

		SELECT @SEQUENCEOraganisationAddressId = (NEXT VALUE FOR [SEQUENCEOraganisationAddress]);
	END

	--Comment : Here insert data and return id for further refernces
	BEGIN

	--Comment : Here If record exists then update otherwise insert new one
	IF NOT EXISTS(SELECT Id FROM OraganisationAddress WHERE OrganizationID=@OrganizationID)
	BEGIN

		--Comment : Here first get SEQUNCE id for table
		SELECT @SEQUENCEOraganisationAddressId = (NEXT VALUE FOR [SEQUENCEOraganisationAddress]);	

		--Comment : Insert record into table
		INSERT INTO [dbo].[OraganisationAddress]
			([Id]
			,[OrganizationID]
			,[Address1]
			,[Address2]
			,[Address3]
			,[City]
			,[County]
			,[StateCode]
			,[ZipCode]
			,[CountryID]
			,[IsCorporateAddress]
			,[ContactName]
			,[ContactNumber1]
			,[ContactNumber2]
			,[Fax]
			,[IsActive]
			,[CreatedDate]
			,[CreatedBy]
			,[ModifiedDate]
			,[ModifiedBy])
		VALUES
		(
			 --NEXT VALUE FOR [SEQUENCEOraganisationAddress]
			 @SEQUENCEOraganisationAddressId
			,@OrganizationID  
    		,@Address1          
	 		,@Address2          
			,@Address3          
			,@City              
			,@County            
    		,@StateCode         
			,@ZipCode           
			,@CountryID         
			,@IsCorporateAddress
			,@ContactName       
			,@ContactNumber1    
			,@ContactNumber2    
			,@Fax               
			,@IsActive          
			,@CreatedDate       
			,@CreatedBy         
			,@ModifiedDate      
			,@ModifiedBy  
		)

	END
	ELSE
	BEGIN
		--Comment : Here first get SEQUNCE id for table (Becoz this is update get it by EmailId)
		SELECT @SEQUENCEOraganisationAddressId = Id FROM OraganisationAddress WHERE OrganizationID=@OrganizationID;

		UPDATE OraganisationAddress SET 
		--Address1            = @Address1          
		--,Address2           = @Address2          
		--,Address3           = @Address3          
		--,City               = @City              
		--,County             = @County            
		--,StateCode          = @StateCode         
		--,ZipCode            = @ZipCode           
		--,CountryID          = @CountryID         
		--,IsCorporateAddress = @IsCorporateAddress
		--,ContactName        = @ContactName       
		--,ContactNumber1     = @ContactNumber1    
		--,ContactNumber2     = @ContactNumber2    
		--,Fax                = @Fax                        
		ModifiedDate       = @ModifiedDate      
		,ModifiedBy			= @ModifiedBy 
		WHERE OrganizationID=@OrganizationID
	END

	--Comment : Here set inserted row sequence id
	SET @ReturnRowId = @SEQUENCEOraganisationAddressId;

	END

	--Comment : Here finally return inserted row-id
	RETURN @ReturnRowId;

END      