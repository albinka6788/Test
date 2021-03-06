/****** Object:  StoredProcedure [dbo].[CreateOraganisationUserDetail]    Script Date: 10/28/2015 3:04:05 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- ================================================
/*
@Author      : Anuj Kumar Singh
@Description : This proc will insert OraganisationUser Detail  into OraganisationUserDetail table
@CreatedOn   : 2015-oct-28

@Updation History : 
   @Update-1 : {date}, {by}, {description} 

*/
-- ================================================

CREATE PROCEDURE [dbo].[CreateOraganisationUserDetail] 
(
	       @OrganizationName VARCHAR(200)
		  ,@EmailID          VARCHAR(150)
		  ,@Password         VARCHAR(256)
		  ,@Tin              INT
		  ,@Ssn              INT
		  ,@Fein             INT
		  ,@IsActive         BIT    
		  ,@CreatedDate		 DATETIME
		  ,@CreatedBy		 INT
		  ,@ModifiedDate	 DATETIME
		  ,@ModifiedBy		 INT
		  ,@FirstName        VARCHAR(255)
		  ,@LastName         VARCHAR(255)
		  ,@PolicyCode       VARCHAR(50)
		  ,@PhoneNumber      INT
	  )	
	  AS 	
	  BEGIN

	-- SET NOCOUNT ON added to prevent extra result sets from
	SET NOCOUNT OFF;

	--Comment : Insert record into table

		INSERT INTO [dbo].[OrganisationUserDetail]
           ([Id]
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
           ,[PhoneNumber])
     VALUES
	 (
	       NEXT VALUE FOR [SEQUENCEOrganisationUserDetail]
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