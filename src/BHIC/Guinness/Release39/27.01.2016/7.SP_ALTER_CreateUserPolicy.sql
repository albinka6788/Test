USE [Guinness_DB]
GO
/****** Object:  StoredProcedure [dbo].[CreateUserPolicy]    Script Date: 27-01-2016 15:25:18 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- ================================================
/*
@Author      : Venkatesh
@Description : This proc will insert into OrganizationUserDetails,OrganizationAddress,Quote and Policy table.In case of
               failure in any table, the transaction will be rollback
			   If the user email address exist, organization user details and organization address will be update
			   Otherwise new record will be created.
@CreatedOn   : 2015-oct-28

@Updation History : 
   @Update-1 : {2016-Jan-27}, {Venkatesh}, { #1. Added validation to check quote and policy number exist. if exits transaction will rollback. } 
   
*/
-- ================================================

ALTER PROCEDURE [dbo].[CreateUserPolicy] 
(
	       @OrganizationName VARCHAR(200)
		  ,@EmailID          VARCHAR(150)
		  ,@Password         VARCHAR(256)
		  ,@TaxType          INT
		  ,@TaxId            VARCHAR(256) = NULL
		  ,@FirstName        VARCHAR(255)
		  ,@LastName         VARCHAR(255)
		  ,@PolicyCode       VARCHAR(50)
		  ,@PhoneNumber      BIGINT
		  ,@Address1            VARCHAR(200)
	      ,@Address2            VARCHAR(200)		= NULL
	      ,@Address3            VARCHAR(200)		= NULL
	      ,@City                VARCHAR(200)		= NULL
	      ,@County              VARCHAR(200)		= NULL
	      ,@StateCode           CHAR(2)
	      ,@ZipCode             INT
		  ,@QuoteNumber               VARCHAR(20)
		  ,@RequestDate  	          DATETIME			= GETDATE
	      ,@ExpiryDate                DATETIME			= NULL
	      ,@PremiumAmount             NUMERIC(18,2)
		  ,@PaymentoptionId			  INT				= NULL
		  ,@AgencyCode				  VARCHAR(20)		= NULL
		  ,@PolicyNumber		VARCHAR(20)
		  ,@EffectiveDate	DATETIME
		  ,@PolicyExpiryDate		DATETIME
	
)	
AS 	
BEGIN
BEGIN TRANSACTION T1

BEGIN TRY
	-- SET NOCOUNT ON added to prevent extra result sets from
	SET NOCOUNT OFF;

	--Coment : Here local variables declaration and initialization 
	BEGIN
	    --SELECT @OrganisationUserDetailID=ID from [OrganisationUserDetail] where EmailID=@EmailID AND IsActive=1;

		/*If @OrganisationUserDetailID>0
		BEGIN
			RETURN 'EmailID already exits';
		END
		SELECT @OrganisationUserDetailID=ID from [OrganisationUserDetail] where EmailID=@EmailID AND IsActive=0;
		*/
		DECLARE @OrganisationUserDetailID INT=0;
		SELECT @OrganisationUserDetailID=ID from [OrganisationUserDetail] where EmailID=@EmailID;
		
		DECLARE @OrganisationAddressID INT=0;
		SELECT @OrganisationAddressID=ID from [OraganisationAddress] where OrganizationID=@OrganisationUserDetailID;



		DECLARE @ReturnInsertedRowId BIGINT = 0;
		DECLARE @SEQUENCEOrganisationUserDetailId BIGINT = 0;
		SELECT @SEQUENCEOrganisationUserDetailId = (NEXT VALUE FOR [SEQUENCEOrganisationUserDetail]);

		DECLARE @SEQUENCEOraganisationAddressId BIGINT = 0;
		SELECT @SEQUENCEOraganisationAddressId = (NEXT VALUE FOR [SEQUENCEOraganisationAddress]);

		DECLARE @Quote_Identity BIGINT=0;
		SELECT @Quote_Identity = (NEXT VALUE FOR [SEQUENCEQuote]);

		DECLARE @Policy_Identity BIGINT=0;
		SELECT @Policy_Identity = (NEXT VALUE FOR SEQUENCEPolicy);

		DECLARE @Tin VARCHAR(256) =@TaxId;
		DECLARE @ssn VARCHAR(256)=NULL;
		IF @TaxType=2
		BEGIN
			SET @SSN = @TaxId;
			SET @Tin=NULL;
		END
	END

	--Comment : Here insert data and return id for further refernces
	BEGIN


	IF @OrganisationUserDetailID=0
	BEGIN
	INSERT INTO [dbo].[OrganisationUserDetail]
    (
		 [Id]
        ,[OrganizationName]
        ,[EmailID]
        ,[Password]
        ,[Tin]
        ,[Ssn]
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
		,1        
		,GETDATE()		
		,0		
		,GETDATE()	
		,0		
		,@FirstName       
		,@LastName        
		,@PolicyCode      
		,@PhoneNumber    
    )
END
	ELSE
	BEGIN
	SET @SEQUENCEOrganisationUserDetailId=@OrganisationUserDetailID;
	UPDATE [dbo].[OrganisationUserDetail] SET    
	    [OrganizationName]=@OrganizationName,
        [Password]=@Password,
        [Tin]=@Tin,
        [Ssn]=@Ssn,
        [IsActive]=1,
        [ModifiedDate]=GETDATE(),
        [ModifiedBy]=1,
        [FirstName]=@FirstName,
        [LastName]=@LastName,
        [PolicyCode]=@PolicyCode,
        [PhoneNumber]=@PhoneNumber
		WHERE ID=@OrganisationUserDetailID;

	/*
	Need to verify with Guru, whether we can update the address or for each quote new address should be created
	SET @SEQUENCEOraganisationAddressId=@OrganisationAddressID;

	UPDATE [dbo].[OraganisationAddress] SET
         [Address1]=@Address1
        ,[Address2]=@Address2
        ,[Address3]=@Address3
        ,[City]=@City
        ,[StateCode]=@StateCode
        ,[ZipCode]=@ZipCode
        ,[ModifiedDate]=GETDATE()
        ,[ModifiedBy]=1
		WHERE OrganizationID=@OrganisationUserDetailID
		*/
	END

	
	INSERT INTO [dbo].[OraganisationAddress]
        ([Id]
        ,[OrganizationID]
        ,[Address1]
        ,[Address2]
        ,[Address3]
        ,[City]
        ,[StateCode]
        ,[ZipCode]
        ,[CountryID]
        ,[IsCorporateAddress]
        ,[IsActive]
        ,[CreatedDate]
        ,[CreatedBy]
        ,[ModifiedDate]
        ,[ModifiedBy])
    VALUES
	(
         --NEXT VALUE FOR [SEQUENCEOraganisationAddress]
		 @SEQUENCEOraganisationAddressId
		,@SEQUENCEOrganisationUserDetailId  
    	,@Address1          
	 	,@Address2          
		,@Address3          
		,@City              
		,@StateCode         
		,@ZipCode           
		,1         
		,1
		,1          
		,GETDATE()       
		,1        
		,GETDATE()      
		,1  
	)
	
	--Comment : Here If record exists then rollback transaction otherwise insert new one
	IF NOT EXISTS(SELECT Id,QuoteNumber FROM Quote WHERE QuoteNumber = @QuoteNumber)
	BEGIN
	INSERT INTO [dbo].[Quote]
			  ([Id]
			  ,[QuoteNumber]
			  ,[OrganizationUserDetailID]
			  ,[OrganizationAddressID]
			  ,[LineOfBusinessId]
			  ,[ExternalSystemID]
			  ,[RequestDate]
			  ,[ExpiryDate]
			  ,[PremiumAmount]
			  ,[PaymentoptionId]
			  ,[AgencyCode]
			  ,[IsActive]
			  ,[CreatedDate]
			  ,[CreatedBy]
			  ,[ModifiedDate]
			  ,[ModifiedBy])
     
			  VALUES(
			   @Quote_Identity
			  ,@QuoteNumber
			  ,@SEQUENCEOrganisationUserDetailId
			  ,@SEQUENCEOraganisationAddressId   
			  ,2
			  ,1
			  ,@RequestDate
			  ,@ExpiryDate
			  ,@PremiumAmount
			  ,@PaymentoptionId
			  ,@AgencyCode
			  ,1
			  ,GETDATE()
			  ,1
			  ,GETDATE()
			  ,1
		   )
	   END
	   ELSE
	   BEGIN
		ROLLBACK TRANSACTION T1
		RETURN 0
	   END

	   --Comment : Here If record exists then rollback transaction otherwise insert new one
	IF NOT EXISTS(SELECT Id,PolicyNumber FROM Policy WHERE PolicyNumber = @PolicyNumber)
	BEGIN
		INSERT INTO Policy
		(Id
		,QuoteID
		,PolicyNumber
		,EffectiveDate
		,ExpiryDate
		,PremiumAmount
		,PaymentOptionID
		,IsActive
		,CreatedDate
		,CreatedBy
		,ModifiedDate
		,ModifiedBy) 
		VALUES(@Policy_Identity
		,@Quote_Identity
		,@PolicyNumber
		,@EffectiveDate
		,@PolicyExpiryDate
		,@PremiumAmount
		,@PaymentOptionID
		,1
		,GETDATE()
		,1
		,Getdate()
		,1);
		END
		ELSE
		BEGIN
		 ROLLBACK TRANSACTION T1
		 RETURN 0
		END
		

END
END TRY
BEGIN CATCH
   ROLLBACK TRANSACTION T1
   --RETURN Error_Message()
   RETURN 0
END CATCH
	
COMMIT TRANSACTION T1
  --Comment : Here finally return inserted row-id
	RETURN @Policy_Identity


END      
