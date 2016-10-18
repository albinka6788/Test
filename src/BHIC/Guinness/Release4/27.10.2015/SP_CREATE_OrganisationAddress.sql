
/****** Object:  StoredProcedure [dbo].[CreateOraganisationAddress]    Script Date: 10/27/2015 6:32:18 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- ================================================
/*
@Author      : Anuj Kumar Singh
@Description : This proc will insert OrganisationAddress details into OrganisationAddress table
@CreatedOn   : 2015-oct-27

@Updation History : 
   @Update-1 : {date}, {by}, {description} 

*/
-- ================================================

CREATE PROCEDURE [dbo].[CreateOraganisationAddress] 
(
	       @OrganizationID      INT
	      ,@Address1            VARCHAR(200)
	      ,@Address2            VARCHAR(200)
	      ,@Address3            VARCHAR(200)
	      ,@City                VARCHAR(200)
	      ,@County              VARCHAR(200)
	      ,@StateCode           CHAR(2)
	      ,@ZipCode             INT
	      ,@CountryID           INT
	      ,@IsCorporateAddress  BIT
	      ,@ContactName         VARCHAR(200)
          ,@ContactNumber1      INT
          ,@ContactNumber2      INT
		  ,@Fax                 INT
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
             NEXT VALUE FOR [SEQUENCEOraganisationAddress]
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