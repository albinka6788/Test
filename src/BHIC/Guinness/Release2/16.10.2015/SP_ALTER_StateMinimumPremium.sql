
ALTER TABLE StateMinimumPremium 
ADD StateId INTEGER NULL
FOREIGN KEY(StateId) REFERENCES StateMaster(Id);
GO

Update T1 SET T1.StateId=T2.Id FROM StateMinimumPremium T1, StateMaster T2 Where T1.StateCode=T2.StateCode
GO

ALTER TABLE StateMinimumPremium 
DROP COLUMN StateCode
GO

