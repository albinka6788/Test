
ALTER TABLE FrequencyClass 
ADD StateId INTEGER NULL
FOREIGN KEY(StateId) REFERENCES StateMaster(Id)
GO

Update T1 SET T1.StateId=T2.Id FROM FrequencyClass T1, StateMaster T2 Where T1.StateCode=T2.StateCode
GO

ALTER TABLE FrequencyClass 
DROP COLUMN StateCode
GO

ALTER TABLE FrequencyClass 
ALTER COLUMN StateId INTEGER NOT NULL
GO
