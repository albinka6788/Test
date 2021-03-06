 /*=============================================
 Author:		<Albin K A>
 Create date: <20-Nov-2015>
 Description:	<selecting Certificates based on PolicyNumber>
exec GetCertificateofInsurance ''
 =============================================*/
ALTER PROCEDURE [dbo].[GetCertificateofInsurance]
(
@PolicyNumber Varchar(20)
)
AS
BEGIN
select [CertificateRequestId]
      ,[PolicyId]     
	  from [dbo].[CertificateOfInsurance]
	  where policyId in(select distinct Id from Policy where PolicyNumber = @PolicyNumber)
	   
END