if not exists(select * from PolicyCancelOptions where Options='No longer have employees' and LineOfBusinessID=2)Insert into PolicyCancelOptions(id,LineOfBusinessID,Options,IsActive,CreatedDate,CreatedBy) values(NEXT VALUE FOR SEQUENCEPolicyCancelOptions,2,'No longer have employees',1,GETDATE(),1)
GO
if not exists(select * from PolicyCancelOptions where Options='Sold the business' and LineOfBusinessID=2)Insert into PolicyCancelOptions(id,LineOfBusinessID,Options,IsActive,CreatedDate,CreatedBy) values(NEXT VALUE FOR SEQUENCEPolicyCancelOptions,2,'Sold the business',1,GETDATE(),1)
GO
if not exists(select * from PolicyCancelOptions where Options='Closed the business' and LineOfBusinessID=2)Insert into PolicyCancelOptions(id,LineOfBusinessID,Options,IsActive,CreatedDate,CreatedBy) values(NEXT VALUE FOR SEQUENCEPolicyCancelOptions,2,'Closed the business',1,GETDATE(),1)
GO
if not exists(select * from PolicyCancelOptions where Options='Moved my coverage to a different insurance company' and LineOfBusinessID=2)Insert into PolicyCancelOptions(id,LineOfBusinessID,Options,IsActive,CreatedDate,CreatedBy) values(NEXT VALUE FOR SEQUENCEPolicyCancelOptions,2,'Moved my coverage to a different insurance company',1,GETDATE(),1)
GO
if not exists(select * from PolicyCancelOptions where Options='Cannot provide the limits and/or coverage options that I need' and LineOfBusinessID=2)Insert into PolicyCancelOptions(id,LineOfBusinessID,Options,IsActive,CreatedDate,CreatedBy) values(NEXT VALUE FOR SEQUENCEPolicyCancelOptions,2,'Cannot provide the limits and/or coverage options that I need',1,GETDATE(),1)
GO
if not exists(select * from PolicyCancelOptions where Options='Other' and LineOfBusinessID=2)Insert into PolicyCancelOptions(id,LineOfBusinessID,Options,IsActive,CreatedDate,CreatedBy) values(NEXT VALUE FOR SEQUENCEPolicyCancelOptions,2,'Other',1,GETDATE(),1)
GO