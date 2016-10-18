/*Insert Commercial Auto in line of business*/

DECLARE @LineOfBusinessID				INT
DECLARE @MaxStateLineOfBusinessesID		INT
DECLARE @CurrentSequenceValue			INT
DECLARE @sql							NVARCHAR(MAX)

SET @CurrentSequenceValue = (SELECT CONVERT(INT, CURRENT_VALUE) FROM SYS.sequences WHERE name = 'SEQUENCEStateLineOfBusinesses')
PRINT @CurrentSequenceValue

SET @LineOfBusinessID = 3
SET @MaxStateLineOfBusinessesID = (SELECT MAX(ID) FROM StateLineOfBusinesses)

IF (@MaxStateLineOfBusinessesID >= @CurrentSequenceValue)
BEGIN
	PRINT @MaxStateLineOfBusinessesID
	SET @MaxStateLineOfBusinessesID = @MaxStateLineOfBusinessesID + 1
	SET @sql = 'ALTER SEQUENCE SEQUENCEStateLineOfBusinesses RESTART WITH ' + CONVERT(VARCHAR, @MaxStateLineOfBusinessesID)
	EXEC (@sql)
	SET @CurrentSequenceValue = (SELECT CONVERT(INT, CURRENT_VALUE) FROM SYS.sequences WHERE name = 'SEQUENCEStateLineOfBusinesses')
	PRINT @CurrentSequenceValue
END

IF NOT EXISTS (SELECT 1 FROM [LineOfBusiness] WHERE ID = @LineOfBusinessID) 
BEGIN
	PRINT 'Adding line of business'
	INSERT [dbo].[LineOfBusiness] ([Id], [Abbreviation], [LineOfBusinessName], [IsActive], [CreatedDate], [CreatedBy], [ModifiedDate], [ModifiedBy]) 
	VALUES (@LineOfBusinessID, N'CA', N'Commercial Auto', 1, GETDATE(), 1, GETDATE(), NULL)
END

INSERT [dbo].[StateLineOfBusinesses] 
	([Id], [StateId], [LineOfBusinessId], [IsActive], [CreatedDate], [CreatedBy], [ModifiedDate], [ModifiedBy]) 
SELECT (NEXT VALUE FOR [SEQUENCEStateLineOfBusinesses]), st.ID, @LineOfBusinessID, 1, getdate(), 1, getdate(), 1
FROM StateMaster st
WHERE st.StateCode NOT IN ('AK', 'HI', 'KY', 'MA', 'MI', 'MS', 'NC', 'NH')
	AND st.Id NOT IN (SELECT stb.StateId FROM StateLineOfBusinesses stb WHERE stb.LineOfBusinessId = @LineOfBusinessID)

SET @CurrentSequenceValue = (SELECT CONVERT(INT, CURRENT_VALUE) FROM SYS.sequences WHERE name = 'SEQUENCEStateLineOfBusinesses')
PRINT @CurrentSequenceValue

SELECT * FROM StateLineOfBusinesses

