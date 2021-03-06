
-- ================================================
/*
@Author      : Perm Pratap Singh Rajpurohit
@Description : This proc will get user credentials details based on EmailId
@CreatedOn   : 2015-Nov-20 

@Updation History : 
   @Update-0 : {date}, {by}, {description} 

@ExampleCall : 
EXEC GetUserCredentialDetails 'prem7@gmail.com'

--Select * From OrganisationUserDetail

*/
-- ================================================

CREATE PROCEDURE [dbo].[GetUserCredentialDetails]
(
	 @EmailId	VARCHAR(150)
)
AS 
BEGIN

	-- SET NOCOUNT ON added to prevent extra result sets from
	SET NOCOUNT OFF;

	--Comment : Here If record exists then return data
	BEGIN
		SELECT Id,EmailId,[Password] As Password FROM OrganisationUserDetail WHERE EmailId=@EmailId
	END

END




