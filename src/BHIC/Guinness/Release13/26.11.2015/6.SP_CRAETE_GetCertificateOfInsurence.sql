/****** Object:  StoredProcedure [dbo].[GetCertificateofInsurance]    Script Date: 20-11-2015 PM 10:39:41 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

 /*=============================================
 Author:		<Albin K A>
 Create date: <20-Nov-2015>
 Description:	<selecting Certificates based on PolicyNumber>
exec GetCertificateofInsurance ''
 =============================================*/
CREATE PROCEDURE [dbo].[GetCertificateofInsurance]
(
@PolicyNumber VARCHAR(20)
)
AS
BEGIN
SELECT [CertificateRequestId]
      ,[PolicyId]     
	  FROM [dbo].[CertificateOfInsurance]
	  WHERE policyId IN(SELECT DISTINCT PolicyId FROM Policy WHERE PolicyNumber = @PolicyNumber)
	   
END
GO


