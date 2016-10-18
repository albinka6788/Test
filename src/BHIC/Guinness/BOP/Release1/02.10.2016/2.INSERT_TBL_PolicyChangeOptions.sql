/*Insert Policy Change Options for BOP*/

DECLARE @LineOfBusinessID				INT
DECLARE @CurrentSequenceValue			INT
DECLARE @MaxPolicyChangeOptionID        INT
DECLARE @sql							NVARCHAR(MAX)

SET @CurrentSequenceValue = (SELECT CONVERT(INT, CURRENT_VALUE) FROM SYS.sequences WHERE name = 'SEQUENCEPolicyChangeOptions')
PRINT @CurrentSequenceValue

SET @LineOfBusinessID = 2
SET @MaxPolicyChangeOptionID = (SELECT MAX(ID) FROM PolicyChangeOptions)

DELETE FROM PolicyChangeOptions WHERE LineOfBusinessID=@LineOfBusinessID;

IF (@MaxPolicyChangeOptionID >= @CurrentSequenceValue)
BEGIN
	PRINT @MaxPolicyChangeOptionID
	SET @MaxPolicyChangeOptionID = @MaxPolicyChangeOptionID + 1
	SET @sql = 'ALTER SEQUENCE SEQUENCEPolicyChangeOptions RESTART WITH ' + CONVERT(VARCHAR, @MaxPolicyChangeOptionID)
	EXEC (@sql)
	SET @CurrentSequenceValue = (SELECT CONVERT(INT, CURRENT_VALUE) FROM SYS.sequences WHERE name = 'SEQUENCEPolicyChangeOptions')
	PRINT @CurrentSequenceValue
END


INSERT [PolicyChangeOptions] ([Id], [LineOfBusinessID], [Options], [IsActive], [CreatedDate], [CreatedBy], [ModifiedDate], [ModifiedBy]) VALUES ((NEXT VALUE FOR [SEQUENCEPolicyChangeOptions]), @LineOfBusinessID, N'Physical Location Address', 1,getdate(), 1, NULL, NULL)
INSERT [PolicyChangeOptions] ([Id], [LineOfBusinessID], [Options], [IsActive], [CreatedDate], [CreatedBy], [ModifiedDate], [ModifiedBy]) VALUES ((NEXT VALUE FOR [SEQUENCEPolicyChangeOptions]), @LineOfBusinessID, N'Additional Insureds', 1, getdate(), 1, NULL, NULL)
INSERT [PolicyChangeOptions] ([Id], [LineOfBusinessID], [Options], [IsActive], [CreatedDate], [CreatedBy], [ModifiedDate], [ModifiedBy]) VALUES ((NEXT VALUE FOR [SEQUENCEPolicyChangeOptions]), @LineOfBusinessID, N'Coverages', 1, getdate(), 1, NULL, NULL)
INSERT [PolicyChangeOptions] ([Id], [LineOfBusinessID], [Options], [IsActive], [CreatedDate], [CreatedBy], [ModifiedDate], [ModifiedBy]) VALUES ((NEXT VALUE FOR [SEQUENCEPolicyChangeOptions]), @LineOfBusinessID, N'Adding/Deleting Location', 1, getdate(), 1, NULL, NULL)
INSERT [PolicyChangeOptions] ([Id], [LineOfBusinessID], [Options], [IsActive], [CreatedDate], [CreatedBy], [ModifiedDate], [ModifiedBy]) VALUES ((NEXT VALUE FOR [SEQUENCEPolicyChangeOptions]), @LineOfBusinessID, N'Limits/Deductibles', 1, getdate(), 1, NULL, NULL)
INSERT [PolicyChangeOptions] ([Id], [LineOfBusinessID], [Options], [IsActive], [CreatedDate], [CreatedBy], [ModifiedDate], [ModifiedBy]) VALUES ((NEXT VALUE FOR [SEQUENCEPolicyChangeOptions]), @LineOfBusinessID, N'Mailing Address', 1, getdate(), 1, NULL, NULL)
INSERT [PolicyChangeOptions] ([Id], [LineOfBusinessID], [Options], [IsActive], [CreatedDate], [CreatedBy], [ModifiedDate], [ModifiedBy]) VALUES ((NEXT VALUE FOR [SEQUENCEPolicyChangeOptions]), @LineOfBusinessID, N'Mortgagee/Loss Payee changes', 1, getdate(), 1, NULL, NULL)
INSERT [PolicyChangeOptions] ([Id], [LineOfBusinessID], [Options], [IsActive], [CreatedDate], [CreatedBy], [ModifiedDate], [ModifiedBy]) VALUES ((NEXT VALUE FOR [SEQUENCEPolicyChangeOptions]), @LineOfBusinessID, N'Other', 1, getdate(), 1, NULL, NULL)
INSERT [PolicyChangeOptions] ([Id], [LineOfBusinessID], [Options], [IsActive], [CreatedDate], [CreatedBy], [ModifiedDate], [ModifiedBy]) VALUES ((NEXT VALUE FOR [SEQUENCEPolicyChangeOptions]), @LineOfBusinessID, N'Named Insured', 1, getdate(), 1, NULL, NULL)
INSERT [PolicyChangeOptions] ([Id], [LineOfBusinessID], [Options], [IsActive], [CreatedDate], [CreatedBy], [ModifiedDate], [ModifiedBy]) VALUES ((NEXT VALUE FOR [SEQUENCEPolicyChangeOptions]), @LineOfBusinessID, N'Waiver Request', 1, getdate(), 1, NULL, NULL)

SET @CurrentSequenceValue = (SELECT CONVERT(INT, CURRENT_VALUE) FROM SYS.sequences WHERE name = 'SEQUENCEPolicyChangeOptions')
PRINT @CurrentSequenceValue

SELECT * FROM [PolicyChangeOptions]







