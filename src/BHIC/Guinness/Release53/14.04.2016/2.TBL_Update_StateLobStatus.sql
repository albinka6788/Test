UPDATE StateLobStatus 
SET StatusId = (SELECT Id FROM LobStatus WHERE Status = 'Not Available'),
ModifiedDate = GETDATE(),
ModifiedBy = 1
WHERE StateLobId IN (
SELECT slb.Id 
FROM [StateLineOfBusinesses] slb
INNER JOIN STATEMASTER sm
 ON slb.StateId = sm.Id
INNER JOIN LineOfBusiness lb
 ON slb.LineOfBusinessId = lb.Id
WHERE lb.abbreviation = 'BOP')

UPDATE StateLobStatus 
SET StatusId = (SELECT Id FROM LobStatus WHERE Status = 'Available'),
ModifiedDate = GETDATE(),
ModifiedBy = 1
WHERE StateLobId IN (
SELECT slb.Id 
FROM [StateLineOfBusinesses] slb
INNER JOIN STATEMASTER sm
 ON slb.StateId = sm.Id
INNER JOIN LineOfBusiness lb
 ON slb.LineOfBusinessId = lb.Id
WHERE sm.StateCode IN ('AZ', 'CA', 'CO', 'CT', 'GA', 'IA', 'IL', 'KS', 'MI', 'MN', 'MO', 'MS', 'NC', 'NE', 'NH', 'NJ', 'NV', 'OH', 'PA', 'SC', 'TN', 'TX')
 AND lb.abbreviation = 'BOP')

UPDATE StateLobStatus 
SET StatusId = (SELECT Id FROM LobStatus WHERE Status = 'Coming Soon'),
ModifiedDate = GETDATE(),
ModifiedBy = 1
WHERE StateLobId IN (
SELECT slb.Id 
FROM [StateLineOfBusinesses] slb
INNER JOIN STATEMASTER sm
 ON slb.StateId = sm.Id
INNER JOIN LineOfBusiness lb
 ON slb.LineOfBusinessId = lb.Id
WHERE sm.StateCode IN ('WA', 'NY', 'MA', 'VA', 'FL', 'MD')
 AND lb.abbreviation = 'BOP')

