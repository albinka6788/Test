-- ================================================
/*
@Author      : Krishna
@Description : This proc will return id value else it will return 0 for failure execution.
@Createdon   : 2015/oct/14 
@Modifiedon	 : ----/---/--
@Example call : exec InsertOrUpdateOrganisationUserDetail 96,'Pk','Pk','Pk','Pk@gmail.com','test123',123,123,123,true,'2015-11-15',1,'2015-11-15',1,'CCMSS3',12312312
*/
-- ================================================
ALTER PROCEDURE [dbo].[InsertOrUpdateOrganisationUserDetail]
(
	@id INT,
	@fname VARCHAR(255),
	@lname VARCHAR(255),
	@organizationName VARCHAR(50),
	@email VARCHAR(150),
	@password VARCHAR(256),
	@tin	VARCHAR(256)= null,
	@ssn	VARCHAR(256)=null,
	@fein	VARCHAR(256)=null,
	@isActive BIT = 1,
	@createdDate DATETIME,
	@createdBy INT,
	@modifiedDate DATETIME,
	@modifiedBy INT,
	@policyCode VARCHAR(50),
	@phoneNumber BIGINT
)
AS 
	IF NOT EXISTS(SELECT Id,EmailID FROM dbo.organisationuserdetail  WHERE EMAILID=@email) -- FOR INSERT OPERATION
		BEGIN
    		INSERT INTO OrganisationUserDetail
    		(
    			Id, OrganizationName, EmailID, [Password], TIN, Ssn, Fein,
				IsActive, CreatedDate, CreatedBy, ModifiedDate, ModifiedBy,
				FirstName, LastName, PolicyCode,PhoneNumber
    		)
    		VALUES
    		(
    		    NEXT VALUE FOR SEQUENCEOrganisationUserDetail,
				@organizationName, @email, @password, @tin, @ssn, @fein,
				@isActive, @createdDate, @createdBy, @modifiedDate, @modifiedBy,
				@fname, @lname, @policyCode,@phoneNumber
    		)
    		select id from OrganisationUserDetail WHERE EMAILID=@email
		END
	ELSE
		BEGIN
			select Id =0
		END
