
--DROP index unique_quotenumber ON QUOTE
--DROP index unique_Policynumber ON Policy

BEGIN

BEGIN--Comment : Here add index on quote Table

DECLARE @Id INT = (SELECT ISNULL(MAX(Id),0) FROM Quote); 

DECLARE @SQL NVARCHAR(MAX) = CONCAT('CREATE UNIQUE INDEX unique_quotenumber ON QUOTE(quotenumber) WHERE Id > ',@Id);
--PRINT @SQL

EXEC sp_executeSQL @SQL;

END

BEGIN--Comment : Here add index on Policy Table

SELECT @Id = ISNULL(MAX(Id),0) FROM Policy; 

SET @SQL = CONCAT('CREATE UNIQUE INDEX unique_Policynumber ON POLICY(Policynumber) WHERE Id > ',@Id);
--PRINT @SQL

EXEC sp_executeSQL @SQL;

END

END