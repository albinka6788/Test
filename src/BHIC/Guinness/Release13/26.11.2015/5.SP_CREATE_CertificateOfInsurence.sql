/****** Object:  StoredProcedure [dbo].[CreateCertificateOfInsurance]    Script Date: 20-11-2015 PM 10:39:14 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

-- ================================================
/*
@Author      : Albin K A
@Description : This proc will insert requests for certificates
@CreatedOn   : 2015-Nov-20

@Updation History : 
   @Update-1 : {date}, {by}, {description} 

*/
-- ================================================

CREATE PROCEDURE [dbo].[CreateCertificateOfInsurance] 
(	 
	 @PolicyNumber VARCHAR(20),
	 @CertificateRequestId VARCHAR(20),
	 @EmailId VARCHAR(50)
)
AS 
BEGIN

	-- SET NOCOUNT ON added to prevent extra result sets from
	SET NOCOUNT OFF

DECLARE @PolicyId INT
DECLARE @UserId INT

  SELECT @UserId=  Id FROM OrganisationUserDetail WHERE EmailID = @EmailId
  SELECT @PolicyId=  Id FROM Policy WHERE PolicyNumber = @PolicyNumber

--Comment : Insert record into table
		INSERT INTO [dbo].[CertificateOfInsurance]
			  ([Id]
			  ,[PolicyId]
			  ,[CertificateRequestId]			  
			  ,[IsActive]
			  ,[CreatedDate]
			  ,[CreatedBy]
			  ,[ModifiedDate]
			  ,[ModifiedBy])
     
			 VALUES(
			NEXT VALUE FOR [SEQUENCECertificateOfInsurance]
			  ,@PolicyId
			  ,@CertificateRequestId			  
			  ,1
			  ,GETDATE()
			  ,@UserId
			  ,GETDATE()
			  ,@UserId)
END	





GO


