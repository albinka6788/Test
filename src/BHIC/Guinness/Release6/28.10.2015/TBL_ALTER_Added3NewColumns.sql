
-- Alter script for OrganisationUserDetail
ALTER TABLE OrganisationUserDetail ADD FirstName VARCHAR(255)
ALTER TABLE OrganisationUserDetail ADD LastName VARCHAR(255)

ALTER TABLE [dbo].[OrganisationUserDetail] ADD PolicyCode VARCHAR(50)
ALTER TABLE [dbo].[OrganisationUserDetail] ADD PhoneNumber INT