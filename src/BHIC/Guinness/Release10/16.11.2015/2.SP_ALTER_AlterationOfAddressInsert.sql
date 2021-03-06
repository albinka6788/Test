
-- ================================================
/*
@Author      : Anuj Kumar Singh
@Description : This proc will insert OrganisationAddress details into OrganisationAddress table
@CreatedOn   : 2015-oct-27

@Updation History : 
   @Update-0 : {date}, {by}, {description} 
   @Update-1 : {2015-Nov-16}, {Prem}, {Made some non-mandatory fieds as NULLABLE} 

*/
-- ================================================

ALTER PROCEDURE [dbo].[CreateOraganisationAddress] 
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
		DECLARE @ReturnInsertedRowId BIGINT = 0;
		DECLARE @SEQUENCEOraganisationAddressId BIGINT = 0;

		SELECT @SEQUENCEOraganisationAddressId = (NEXT VALUE FOR [SEQUENCEOraganisationAddress]);
	END

	--Comment : Here insert data and return id for further refernces
	BEGIN

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

	--Comment : Here set inserted row sequence id
	SET @ReturnInsertedRowId = @SEQUENCEOraganisationAddressId;

	END

	--Comment : Here finally return inserted row-id
	RETURN @ReturnInsertedRowId;

END      