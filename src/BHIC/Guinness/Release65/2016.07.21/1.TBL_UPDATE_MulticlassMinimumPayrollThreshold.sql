
--BackUp current minimum-payroll-threshold table
SELECT * INTO MulticlassMinimumPayrollThreshold_20160719 FROM MulticlassMinimumPayrollThreshold

--Select * From MulticlassMinimumPayrollThreshold_20160719

--Truncate current table records
Truncate Table MulticlassMinimumPayrollThreshold

--RESET Table Sequence 
ALTER SEQUENCE SEQUENCEMulticlassMinimumPayrollThreshold RESTART WITH 1

--Select * From MulticlassMinimumPayrollThreshold

--INSERT INTO MulticlassMinimumPayrollThreshold 
--SELECT (NEXT VALUE FOR SEQUENCEMulticlassMinimumPayrollThreshold) Id,StateMaster.Id StateId, XThreshhold.ClassDescriptionId, XThreshhold.MinimumPayrollThreshold, XThreshhold.FriendlyName,
--1 IsActive,GETDATE() CreatedDate,1 CreatedBy,GETDATE() ModifiedDate,1 ModifiedBy
--FROM XThreshhold LEFT OUTER JOIN StateMaster ON StateMaster.StateCode = XThreshhold.StateId