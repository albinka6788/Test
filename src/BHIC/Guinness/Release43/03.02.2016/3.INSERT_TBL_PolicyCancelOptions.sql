if not exists(select * from PolicyCancelOptions where Options='No longer have employees')Insert into PolicyCancelOptions(id,LineOfBusinessID,Options,IsActive,CreatedDate,CreatedBy) values(NEXT VALUE FOR SEQUENCEPolicyCancelOptions,1,'No longer have employees',1,GETDATE(),1)
GO
if not exists(select * from PolicyCancelOptions where Options='Sold the business')Insert into PolicyCancelOptions(id,LineOfBusinessID,Options,IsActive,CreatedDate,CreatedBy) values(NEXT VALUE FOR SEQUENCEPolicyCancelOptions,1,'Sold the business',1,GETDATE(),1)
GO
if not exists(select * from PolicyCancelOptions where Options='Closed the business')Insert into PolicyCancelOptions(id,LineOfBusinessID,Options,IsActive,CreatedDate,CreatedBy) values(NEXT VALUE FOR SEQUENCEPolicyCancelOptions,1,'Closed the business',1,GETDATE(),1)
GO
if not exists(select * from PolicyCancelOptions where Options='Moved my coverage to a different insurance company')Insert into PolicyCancelOptions(id,LineOfBusinessID,Options,IsActive,CreatedDate,CreatedBy) values(NEXT VALUE FOR SEQUENCEPolicyCancelOptions,1,'Moved my coverage to a different insurance company',1,GETDATE(),1)
GO
if not exists(select * from PolicyCancelOptions where Options='Cannot provide the limits and/or coverage options that I need')Insert into PolicyCancelOptions(id,LineOfBusinessID,Options,IsActive,CreatedDate,CreatedBy) values(NEXT VALUE FOR SEQUENCEPolicyCancelOptions,1,'Cannot provide the limits and/or coverage options that I need',1,GETDATE(),1)
GO
if not exists(select * from PolicyCancelOptions where Options='Other')Insert into PolicyCancelOptions(id,LineOfBusinessID,Options,IsActive,CreatedDate,CreatedBy) values(NEXT VALUE FOR SEQUENCEPolicyCancelOptions,1,'Other',1,GETDATE(),1)
GO
