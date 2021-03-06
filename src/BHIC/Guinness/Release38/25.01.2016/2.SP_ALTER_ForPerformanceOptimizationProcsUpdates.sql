/****** Object:  StoredProcedure [dbo].[CreatePolicy]    Script Date: 01/22/2016 5:27:57 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

-- ================================================
/*
@Author      : Anuj Kumar Singh
@Description : This proc will insert policy details into Policy table
@CreatedOn   : 2015-oct-14 

@Updation History : 
   @Update-1 : {date}, {by}, {description} 

*/
-- ================================================

ALTER PROCEDURE [dbo].[CreatePolicy] 
(
	  @QuoteNumber      VARCHAR(20)
	 ,@PolicyNumber		VARCHAR(20)
	 ,@EffectiveDate	DATETIME
	 ,@ExpiryDate		DATETIME
	 ,@PremiumAmount	NUMERIC(18,2)
	 ,@PaymentOptionID	INT
	 ,@IsActive			BIT
	 ,@CreatedDate		DATETIME
	 ,@CreatedBy		INT
	 ,@ModifiedDate		DATETIME
	 ,@ModifiedBy		INT
	 ,@policy_identity  INT    OUTPUT
)
AS 
BEGIN

	-- SET NOCOUNT ON added to prevent extra result sets from
	SET NOCOUNT OFF;

	--Comment : Insert record into table

		SELECT @policy_identity = (NEXT VALUE FOR SEQUENCEPolicy);

		DECLARE @QuoteID INT;
		SELECT @QuoteID=Id FROM Quote WITH (NOLOCK) WHERE QuoteNumber=@QuoteNumber;

		INSERT INTO Policy(Id,QuoteID,PolicyNumber,EffectiveDate,ExpiryDate,PremiumAmount,PaymentOptionID,IsActive,CreatedDate,CreatedBy,ModifiedDate,ModifiedBy) 
		VALUES(@policy_identity,@QuoteID,@PolicyNumber,@EffectiveDate,@ExpiryDate,@PremiumAmount,@PaymentOptionID,@IsActive,@CreatedDate,@CreatedBy,@ModifiedDate,@ModifiedBy);
		
END





GO
/****** Object:  StoredProcedure [dbo].[GetFeinXModFactor]    Script Date: 01/22/2016 5:27:57 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- ================================================
/*
@Author      : Perm Pratap Singh Rajpurohit
@Description : This proc will return premium-factor/xMod-factor for supplied Fein
@CreatedOn   : 2015-Sep-29 

@Updation History : 
   @Update-1 : {date}, {by}, {description} 

@ExampleCall : EXEC GetFeinXModFactor '123456789'

*/
-- ================================================

ALTER PROCEDURE [dbo].[GetFeinXModFactor]
(
	@FeinNumber VARCHAR(9)
)
AS 
BEGIN

	-- SET NOCOUNT ON added to prevent extra result sets from
	SET NOCOUNT ON;

	SELECT Fein,PremiumFactor AS XModValue FROM FeinPremiumFactor WHERE Fein=@FeinNumber AND CAST(ExpiryDate AS DATE) >= CAST(GETDATE() AS DATE);

END

GO
/****** Object:  StoredProcedure [dbo].[GetReferralThresholdClaims]    Script Date: 01/22/2016 5:27:57 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- ================================================
/*
@Author      : Perm Pratap Singh Rajpurohit
@Description : This proc will return referral threshold claims value for supplied StateCode, ClassCode, Annual Payroll
@CreatedOn   : 2015-Sep-30 

@Updation History : 
   @Update-1 : {date}, {by}, {description} 

@ExampleCall : EXEC GetReferralThresholdClaims 'AK','8046','27300'

*/
-- ================================================

ALTER PROCEDURE [dbo].[GetReferralThresholdClaims]
(
	 @StateCode			CHAR(2)
	,@ClassCode			VARCHAR(10)
	,@AnnualPayroll		NUMERIC(18,0)
)
AS 
BEGIN

	-- SET NOCOUNT ON added to prevent extra result sets from
	SET NOCOUNT ON;

	--Comment : Here local variable declaration & initialization
	BEGIN

		DECLARE @OrgActivityType			INT = 0;
		DECLARE @ReferralThresholdClaims	INT = 0;
		DECLARE @UnlimitedTotalPayrollMax	NUMERIC(18,0) = 99999999;

	END

	--Comment : Here STEP - 1 First get OrganizationActivityType based on StateCode, ClassCode
	SELECT @OrgActivityType = T2.OrganizationActivityType FROM StateMaster T1 WITH (NOLOCK), FrequencyClass T2 WITH (NOLOCK) WHERE T1.Id=T2.StateId AND T2.ClassCode=@ClassCode AND T2.IsActive=1 AND T1.StateCode=@StateCode;

	--Comment : Here STEP - 2 According business logic if this StateCode+ClassCode combination doesn't have any data then set it to 2 as Default 
	IF ISNULL(@OrgActivityType,0) = 0
	BEGIN
		SET @OrgActivityType = 2;
	END

	--Comment : Here STEP - 3 Based on fetched OrganizationActivityType get ReferralThresholdClaims value for given AnnualPayroll
	IF @OrgActivityType > 0
	BEGIN
				
		SELECT @ReferralThresholdClaims = ThresholdNumberOfClaims FROM FrequencyClaims WITH (NOLOCK) WHERE (@AnnualPayroll BETWEEN TotalPayrollMin AND ISNULL(TotalPayrollMax,@UnlimitedTotalPayrollMax)) AND IsActive=1 And OrganizationActivityType=@OrgActivityType;
		
		--PRINT 'Reference Column Type : '+ CAST(@OrgActivityType AS VARCHAR);
		--PRINT 'Allowed Referral Threshold Claims : '+ CAST(@ReferralThresholdClaims AS VARCHAR);

		--Comment : Here return this value to calle method
		SELECT ISNULL(@ReferralThresholdClaims,0) AS ReferralThresholdClaims;

	END

END


GO
/****** Object:  StoredProcedure [dbo].[GetStateMandatoryDeductible]    Script Date: 01/22/2016 5:27:57 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

-- ================================================
/*
@Author      : Perm Pratap Singh Rajpurohit
@Description : This proc will return MandatoryDeductible offering for hard states based on supplied State
@CreatedOn   : 2015-Oct-27 

@Updation History : 
   @Update-1 : {date}, {by}, {description} 

@ExampleCall : EXEC GetStateMandatoryDeductible 'AL'

*/
-- ================================================

ALTER PROCEDURE [dbo].[GetStateMandatoryDeductible]
(
	@StateCode CHAR(2)
)
AS 
BEGIN

	-- SET NOCOUNT ON added to prevent extra result sets from
	SET NOCOUNT ON;

	--SELECT StateCode,MinimumPremium As PremiumThreshold FROM StateMinimumPremium WHERE StateCode=@StateCode;
	SELECT T1.Id,T1.StateCode,T2.StateId FROM StateMaster T1 WITH (NOLOCK), MandatoryDeductible T2 WITH (NOLOCK) WHERE T1.Id=T2.StateId AND T2.IsActive=1 AND T1.StateCode=@StateCode;

END


GO
/****** Object:  StoredProcedure [dbo].[GetStateMinimumPremium]    Script Date: 01/22/2016 5:27:57 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- ================================================
/*
@Author      : Perm Pratap Singh Rajpurohit
@Description : This proc will return state with thier minimum payroll threshold limit based on supplied State
@CreatedOn   : 2015-Sep-29 

@Updation History : 
   @Update-1 : {date}, {by}, {description} 

@ExampleCall : EXEC GetStateMinimumPremium 'AK'

*/
-- ================================================

ALTER PROCEDURE [dbo].[GetStateMinimumPremium]
(
	@StateCode CHAR(2)
)
AS 
BEGIN

	-- SET NOCOUNT ON added to prevent extra result sets from
	SET NOCOUNT ON;

	--SELECT StateCode,MinimumPremium As PremiumThreshold FROM StateMinimumPremium WHERE StateCode=@StateCode;
	SELECT T1.Id,T1.StateCode,MinimumPremium As PremiumThreshold FROM StateMaster T1 WITH (NOLOCK), StateMinimumPremium T2 WITH (NOLOCK) WHERE T1.Id=T2.StateId AND T1.StateCode=@StateCode;

END


GO
/****** Object:  StoredProcedure [dbo].[MaintainOraganisationAddress]    Script Date: 01/22/2016 5:27:57 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- ================================================
/*
@Author      : Prem Pratap Singh
@Description : This proc will insert OraganisationAddress detail if not exists otherwise will update details
@CreatedOn   : 2015-Nov-24

@Updation History : 
   @Update-0 : {date}, {by}, {description} 

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
	IF NOT EXISTS(SELECT Id FROM OraganisationAddress WITH (NOLOCK) WHERE OrganizationID=@OrganizationID)
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
		SELECT @SEQUENCEOraganisationAddressId = Id FROM OraganisationAddress WITH (NOLOCK) WHERE OrganizationID=@OrganizationID;

		UPDATE OraganisationAddress SET 
    	 Address1           = @Address1          
	 	,Address2           = @Address2          
		,Address3           = @Address3          
		,City               = @City              
		,County             = @County            
    	,StateCode          = @StateCode         
		,ZipCode            = @ZipCode           
		,CountryID          = @CountryID         
		,IsCorporateAddress = @IsCorporateAddress
		,ContactName        = @ContactName       
		,ContactNumber1     = @ContactNumber1    
		,ContactNumber2     = @ContactNumber2    
		,Fax                = @Fax                        
		,ModifiedDate       = @ModifiedDate      
		,ModifiedBy			= @ModifiedBy 
		WHERE OrganizationID=@OrganizationID
	END

	--Comment : Here set inserted row sequence id
	SET @ReturnRowId = @SEQUENCEOraganisationAddressId;

	END

	--Comment : Here finally return inserted row-id
	RETURN @ReturnRowId;

END      
GO
/****** Object:  StoredProcedure [dbo].[UpdateOraganisationUserDetail]    Script Date: 01/22/2016 5:27:57 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- ================================================      
/*      
@Author      : Sreeram       
@Description : This procedure will update the FirstName,LastName,Phone in OrganisationUserDetail table
			   based on Email
@CreatedOn   : 2015-Nov-20       
      
@Updation History :       
   @Update-1 : 2015-Nov-21, Sreeram, Updated  
   @Update-2 : 2015-Dec-2, Sreeram, Updated
@ExampleCall : 
EXEC UpdateOraganisationUserDetail 'Abc@a.com', 'FirstName', 'LastName',123413   

*/      
-- ================================================      
  
ALTER PROCEDURE [dbo].[UpdateOraganisationUserDetail]
(      
  @Email VARCHAR(150),  
  @Firstname VARCHAR(150), 
  @Lastname VARCHAR(150), 
  @Phone  BIGINT   
)      
AS       
BEGIN      
      
 -- SET NOCOUNT ON added to prevent extra result sets from      
SET NOCOUNT OFF;      
    
     UPDATE OrganisationUserDetail	 
	 SET  FirstName =@Firstname,LastName = @Lastname,PhoneNumber = @Phone
	 WHERE EmailID=@Email; 
       
END 




GO
/****** Object:  StoredProcedure [dbo].[UpdateQuoteOrganizationAddressID]    Script Date: 01/22/2016 5:27:57 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- ================================================
/*
@Author      : Perm Pratap Singh Rajpurohit
@Description : This proc will update reference OrganizationUserId for supplied QuoteId
@CreatedOn   : 2015-Nov-17 

@Updation History : 
   @Update-0 : {date}, {by}, {description} 

@ExampleCall : 
EXEC UpdateQuoteOrganizationAddressID 19551,12,'cybuat50@cyb.com','2016-01-22',1 
EXEC UpdateQuoteOrganizationAddressID 16444,'prem@gmail.com'

*/
-- ================================================

ALTER PROCEDURE [dbo].[UpdateQuoteOrganizationAddressID]
(
	 @QuoteNumber				VARCHAR(20)
	,@OrganizationAddressID		INT				= NULL
	,@EmailId					VARCHAR(150)	= NULL
	,@ModifiedDate				DATETIME		= NULL
	,@ModifiedBy				INT				= NULL
)
AS 
BEGIN

	-- SET NOCOUNT ON added to prevent extra result sets from
	SET NOCOUNT OFF;

	--Comment : Here Update based on following scenario's
	--1. In case UID available then update it directly
	--2. Otherwise in case of EmailId available then get associated Id from "OrganisationUserDetail" and then let's update it in dependent table "Quote"
	
	IF(Isnull(@OrganizationAddressID,0)=0)
	BEGIN
	SELECT @OrganizationAddressID = T2.Id FROM OrganisationUserDetail T1 WITH (NOLOCK),OraganisationAddress T2 WITH (NOLOCK) WHERE T1.Id=T2.OrganizationID AND T1.EmailID=@EmailId
	END

	BEGIN
		UPDATE Quote SET 
		 ModifiedDate=ISNULL(@ModifiedDate,GETDATE())
		,ModifiedBy=ISNULL(@ModifiedBy,1)
		,OrganizationAddressID=@OrganizationAddressID
		WHERE QuoteNumber=@QuoteNumber
	END

END

GO
/****** Object:  StoredProcedure [dbo].[GetPrimaryClassCodeData]    Script Date: 01/22/2016 5:50:30 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- ================================================
/*
@Author      : Nishank Kumar Srivastava
@Description : This proc will get all the data related to primary class code
@CreatedOn   : 2015-Nov-9 

@Updation History : 
   @Update-0 : {date}, {by}, {description} 

   
@ExampleCall : 
EXEC GetPrimaryClassCodeData

*/
-- ================================================

ALTER PROCEDURE [dbo].[GetPrimaryClassCodeData]
AS 
BEGIN

	-- SET NOCOUNT ON added to prevent extra result sets from
	SET NOCOUNT OFF;

	SELECT T2.StateCode,T1.ClassDescriptionId,T1.MinimumPayrollThreshold,T1.FriendlyName
	FROM MulticlassMinimumPayrollThreshold T1 
	LEFT OUTER JOIN StateMaster T2 WITH (NOLOCK) ON T1.StateId = T2.Id

END


Go

/****** Object:  StoredProcedure [dbo].[UpdateQuoteOrganizationUserId]    Script Date: 01/22/2016 6:08:12 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- ================================================
/*
@Author      : Perm Pratap Singh Rajpurohit
@Description : This proc will update reference OrganizationUserId for supplied QuoteId
@CreatedOn   : 2015-Nov-04 

@Updation History : 
   @Update-0 : {date}, {by}, {description} 
   @Update-1 : {2015-Nov-16}, {Prem}, {Made changes to allow update data based on either UID or EmailId} 

@ExampleCall : 
EXEC UpdateQuoteOrganizationUserId 16444,14
EXEC UpdateQuoteOrganizationUserId 16444,'prem@gmail.com'

*/
-- ================================================

ALTER PROCEDURE [dbo].[UpdateQuoteOrganizationUserId]
(
	 @QuoteNumber				VARCHAR(20)
	,@OrgnizationUserId			INT				= NULL
	,@EmailId					VARCHAR(150)	= NULL
	,@ModifiedDate				DATETIME		= GETDATE
	,@ModifiedBy				INT				= 1
)
AS 
BEGIN

	-- SET NOCOUNT ON added to prevent extra result sets from
	SET NOCOUNT OFF;

	--Comment : Here Update based on following scenario's
	--1. In case UID available then update it directly
	--2. Otherwise in case of EmailId available then get associated Id from "OrganisationUserDetail" and then let's update it in dependent table "Quote"
	
	IF (ISNULL(@OrgnizationUserId,0)=0)
	BEGIN
		SELECT @OrgnizationUserId= Id FROM OrganisationUserDetail WITH (NOLOCK) WHERE EmailID=@EmailId;
	END

	BEGIN
		UPDATE Quote SET 
		 ModifiedDate=@ModifiedDate
		,ModifiedBy=@ModifiedBy
		,OrganizationUserDetailID=@OrgnizationUserId
		WHERE QuoteNumber=@QuoteNumber
	END

END


