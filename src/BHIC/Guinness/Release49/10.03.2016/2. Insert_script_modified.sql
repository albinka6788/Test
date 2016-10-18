DELETE FROM StateLobStatus

ALTER SEQUENCE SequenceStateLobStatus RESTART WITH 1

INSERT INTO StateLobStatus SELECT (NEXT VALUE FOR SequenceStateLobStatus), slb.Id, (SELECT Id FROM LobStatus WHERE Status = 'Not Available'), GETDATE(), DATEADD(YYYY, 5, GETDATE()), 1, GETDATE(), 1, GETDATE(), 1
FROM [StateLineOfBusinesses] slb
INNER JOIN STATEMASTER sm
 ON slb.StateId = sm.Id
INNER JOIN LineOfBusiness lb
 ON slb.LineOfBusinessId = lb.Id
WHERE sm.StateCode = 'AL'
 AND lb.abbreviation = 'BOP'
INSERT INTO StateLobStatus SELECT (NEXT VALUE FOR SequenceStateLobStatus), slb.Id, (SELECT Id FROM LobStatus WHERE Status = 'Not Available'), GETDATE(), DATEADD(YYYY, 5, GETDATE()), 1, GETDATE(), 1, GETDATE(), 1
FROM [StateLineOfBusinesses] slb
INNER JOIN STATEMASTER sm
 ON slb.StateId = sm.Id
INNER JOIN LineOfBusiness lb
 ON slb.LineOfBusinessId = lb.Id
WHERE sm.StateCode = 'AK'
 AND lb.abbreviation = 'BOP'
INSERT INTO StateLobStatus SELECT (NEXT VALUE FOR SequenceStateLobStatus), slb.Id, (SELECT Id FROM LobStatus WHERE Status = 'Available'), GETDATE(), DATEADD(YYYY, 5, GETDATE()), 1, GETDATE(), 1, GETDATE(), 1
FROM [StateLineOfBusinesses] slb
INNER JOIN STATEMASTER sm
 ON slb.StateId = sm.Id
INNER JOIN LineOfBusiness lb
 ON slb.LineOfBusinessId = lb.Id
WHERE sm.StateCode = 'AZ'
 AND lb.abbreviation = 'BOP'
INSERT INTO StateLobStatus SELECT (NEXT VALUE FOR SequenceStateLobStatus), slb.Id, (SELECT Id FROM LobStatus WHERE Status = 'Not Available'), GETDATE(), DATEADD(YYYY, 5, GETDATE()), 1, GETDATE(), 1, GETDATE(), 1
FROM [StateLineOfBusinesses] slb
INNER JOIN STATEMASTER sm
 ON slb.StateId = sm.Id
INNER JOIN LineOfBusiness lb
 ON slb.LineOfBusinessId = lb.Id
WHERE sm.StateCode = 'AR'
 AND lb.abbreviation = 'BOP'
INSERT INTO StateLobStatus SELECT (NEXT VALUE FOR SequenceStateLobStatus), slb.Id, (SELECT Id FROM LobStatus WHERE Status = 'Available'), GETDATE(), DATEADD(YYYY, 5, GETDATE()), 1, GETDATE(), 1, GETDATE(), 1
FROM [StateLineOfBusinesses] slb
INNER JOIN STATEMASTER sm
 ON slb.StateId = sm.Id
INNER JOIN LineOfBusiness lb
 ON slb.LineOfBusinessId = lb.Id
WHERE sm.StateCode = 'CA'
 AND lb.abbreviation = 'BOP'
INSERT INTO StateLobStatus SELECT (NEXT VALUE FOR SequenceStateLobStatus), slb.Id, (SELECT Id FROM LobStatus WHERE Status = 'Available'), GETDATE(), DATEADD(YYYY, 5, GETDATE()), 1, GETDATE(), 1, GETDATE(), 1
FROM [StateLineOfBusinesses] slb
INNER JOIN STATEMASTER sm
 ON slb.StateId = sm.Id
INNER JOIN LineOfBusiness lb
 ON slb.LineOfBusinessId = lb.Id
WHERE sm.StateCode = 'CO'
 AND lb.abbreviation = 'BOP'
INSERT INTO StateLobStatus SELECT (NEXT VALUE FOR SequenceStateLobStatus), slb.Id, (SELECT Id FROM LobStatus WHERE Status = 'Available'), GETDATE(), DATEADD(YYYY, 5, GETDATE()), 1, GETDATE(), 1, GETDATE(), 1
FROM [StateLineOfBusinesses] slb
INNER JOIN STATEMASTER sm
 ON slb.StateId = sm.Id
INNER JOIN LineOfBusiness lb
 ON slb.LineOfBusinessId = lb.Id
WHERE sm.StateCode = 'CT'
 AND lb.abbreviation = 'BOP'
INSERT INTO StateLobStatus SELECT (NEXT VALUE FOR SequenceStateLobStatus), slb.Id, (SELECT Id FROM LobStatus WHERE Status = 'Not Available'), GETDATE(), DATEADD(YYYY, 5, GETDATE()), 1, GETDATE(), 1, GETDATE(), 1
FROM [StateLineOfBusinesses] slb
INNER JOIN STATEMASTER sm
 ON slb.StateId = sm.Id
INNER JOIN LineOfBusiness lb
 ON slb.LineOfBusinessId = lb.Id
WHERE sm.StateCode = 'DE'
 AND lb.abbreviation = 'BOP'
INSERT INTO StateLobStatus SELECT (NEXT VALUE FOR SequenceStateLobStatus), slb.Id, (SELECT Id FROM LobStatus WHERE Status = 'Not Available'), GETDATE(), DATEADD(YYYY, 5, GETDATE()), 1, GETDATE(), 1, GETDATE(), 1
FROM [StateLineOfBusinesses] slb
INNER JOIN STATEMASTER sm
 ON slb.StateId = sm.Id
INNER JOIN LineOfBusiness lb
 ON slb.LineOfBusinessId = lb.Id
WHERE sm.StateCode = 'DC'
 AND lb.abbreviation = 'BOP'
INSERT INTO StateLobStatus SELECT (NEXT VALUE FOR SequenceStateLobStatus), slb.Id, (SELECT Id FROM LobStatus WHERE Status = 'Coming Soon'), GETDATE(), DATEADD(YYYY, 5, GETDATE()), 1, GETDATE(), 1, GETDATE(), 1
FROM [StateLineOfBusinesses] slb
INNER JOIN STATEMASTER sm
 ON slb.StateId = sm.Id
INNER JOIN LineOfBusiness lb
 ON slb.LineOfBusinessId = lb.Id
WHERE sm.StateCode = 'FL'
 AND lb.abbreviation = 'BOP'
INSERT INTO StateLobStatus SELECT (NEXT VALUE FOR SequenceStateLobStatus), slb.Id, (SELECT Id FROM LobStatus WHERE Status = 'Available'), GETDATE(), DATEADD(YYYY, 5, GETDATE()), 1, GETDATE(), 1, GETDATE(), 1
FROM [StateLineOfBusinesses] slb
INNER JOIN STATEMASTER sm
 ON slb.StateId = sm.Id
INNER JOIN LineOfBusiness lb
 ON slb.LineOfBusinessId = lb.Id
WHERE sm.StateCode = 'GA'
 AND lb.abbreviation = 'BOP'
INSERT INTO StateLobStatus SELECT (NEXT VALUE FOR SequenceStateLobStatus), slb.Id, (SELECT Id FROM LobStatus WHERE Status = 'Not Available'), GETDATE(), DATEADD(YYYY, 5, GETDATE()), 1, GETDATE(), 1, GETDATE(), 1
FROM [StateLineOfBusinesses] slb
INNER JOIN STATEMASTER sm
 ON slb.StateId = sm.Id
INNER JOIN LineOfBusiness lb
 ON slb.LineOfBusinessId = lb.Id
WHERE sm.StateCode = 'HI'
 AND lb.abbreviation = 'BOP'
INSERT INTO StateLobStatus SELECT (NEXT VALUE FOR SequenceStateLobStatus), slb.Id, (SELECT Id FROM LobStatus WHERE Status = 'Not Available'), GETDATE(), DATEADD(YYYY, 5, GETDATE()), 1, GETDATE(), 1, GETDATE(), 1
FROM [StateLineOfBusinesses] slb
INNER JOIN STATEMASTER sm
 ON slb.StateId = sm.Id
INNER JOIN LineOfBusiness lb
 ON slb.LineOfBusinessId = lb.Id
WHERE sm.StateCode = 'ID'
 AND lb.abbreviation = 'BOP'
INSERT INTO StateLobStatus SELECT (NEXT VALUE FOR SequenceStateLobStatus), slb.Id, (SELECT Id FROM LobStatus WHERE Status = 'Available'), GETDATE(), DATEADD(YYYY, 5, GETDATE()), 1, GETDATE(), 1, GETDATE(), 1
FROM [StateLineOfBusinesses] slb
INNER JOIN STATEMASTER sm
 ON slb.StateId = sm.Id
INNER JOIN LineOfBusiness lb
 ON slb.LineOfBusinessId = lb.Id
WHERE sm.StateCode = 'IL'
 AND lb.abbreviation = 'BOP'
INSERT INTO StateLobStatus SELECT (NEXT VALUE FOR SequenceStateLobStatus), slb.Id, (SELECT Id FROM LobStatus WHERE Status = 'Not Available'), GETDATE(), DATEADD(YYYY, 5, GETDATE()), 1, GETDATE(), 1, GETDATE(), 1
FROM [StateLineOfBusinesses] slb
INNER JOIN STATEMASTER sm
 ON slb.StateId = sm.Id
INNER JOIN LineOfBusiness lb
 ON slb.LineOfBusinessId = lb.Id
WHERE sm.StateCode = 'IN'
 AND lb.abbreviation = 'BOP'
INSERT INTO StateLobStatus SELECT (NEXT VALUE FOR SequenceStateLobStatus), slb.Id, (SELECT Id FROM LobStatus WHERE Status = 'Available'), GETDATE(), DATEADD(YYYY, 5, GETDATE()), 1, GETDATE(), 1, GETDATE(), 1
FROM [StateLineOfBusinesses] slb
INNER JOIN STATEMASTER sm
 ON slb.StateId = sm.Id
INNER JOIN LineOfBusiness lb
 ON slb.LineOfBusinessId = lb.Id
WHERE sm.StateCode = 'IA'
 AND lb.abbreviation = 'BOP'
INSERT INTO StateLobStatus SELECT (NEXT VALUE FOR SequenceStateLobStatus), slb.Id, (SELECT Id FROM LobStatus WHERE Status = 'Available'), GETDATE(), DATEADD(YYYY, 5, GETDATE()), 1, GETDATE(), 1, GETDATE(), 1
FROM [StateLineOfBusinesses] slb
INNER JOIN STATEMASTER sm
 ON slb.StateId = sm.Id
INNER JOIN LineOfBusiness lb
 ON slb.LineOfBusinessId = lb.Id
WHERE sm.StateCode = 'KS'
 AND lb.abbreviation = 'BOP'
INSERT INTO StateLobStatus SELECT (NEXT VALUE FOR SequenceStateLobStatus), slb.Id, (SELECT Id FROM LobStatus WHERE Status = 'Not Available'), GETDATE(), DATEADD(YYYY, 5, GETDATE()), 1, GETDATE(), 1, GETDATE(), 1
FROM [StateLineOfBusinesses] slb
INNER JOIN STATEMASTER sm
 ON slb.StateId = sm.Id
INNER JOIN LineOfBusiness lb
 ON slb.LineOfBusinessId = lb.Id
WHERE sm.StateCode = 'KY'
 AND lb.abbreviation = 'BOP'
INSERT INTO StateLobStatus SELECT (NEXT VALUE FOR SequenceStateLobStatus), slb.Id, (SELECT Id FROM LobStatus WHERE Status = 'Not Available'), GETDATE(), DATEADD(YYYY, 5, GETDATE()), 1, GETDATE(), 1, GETDATE(), 1
FROM [StateLineOfBusinesses] slb
INNER JOIN STATEMASTER sm
 ON slb.StateId = sm.Id
INNER JOIN LineOfBusiness lb
 ON slb.LineOfBusinessId = lb.Id
WHERE sm.StateCode = 'LA'
 AND lb.abbreviation = 'BOP'
INSERT INTO StateLobStatus SELECT (NEXT VALUE FOR SequenceStateLobStatus), slb.Id, (SELECT Id FROM LobStatus WHERE Status = 'Not Available'), GETDATE(), DATEADD(YYYY, 5, GETDATE()), 1, GETDATE(), 1, GETDATE(), 1
FROM [StateLineOfBusinesses] slb
INNER JOIN STATEMASTER sm
 ON slb.StateId = sm.Id
INNER JOIN LineOfBusiness lb
 ON slb.LineOfBusinessId = lb.Id
WHERE sm.StateCode = 'ME'
 AND lb.abbreviation = 'BOP'
INSERT INTO StateLobStatus SELECT (NEXT VALUE FOR SequenceStateLobStatus), slb.Id, (SELECT Id FROM LobStatus WHERE Status = 'Coming Soon'), GETDATE(), DATEADD(YYYY, 5, GETDATE()), 1, GETDATE(), 1, GETDATE(), 1
FROM [StateLineOfBusinesses] slb
INNER JOIN STATEMASTER sm
 ON slb.StateId = sm.Id
INNER JOIN LineOfBusiness lb
 ON slb.LineOfBusinessId = lb.Id
WHERE sm.StateCode = 'MD'
 AND lb.abbreviation = 'BOP'
INSERT INTO StateLobStatus SELECT (NEXT VALUE FOR SequenceStateLobStatus), slb.Id, (SELECT Id FROM LobStatus WHERE Status = 'Coming Soon'), GETDATE(), DATEADD(YYYY, 5, GETDATE()), 1, GETDATE(), 1, GETDATE(), 1
FROM [StateLineOfBusinesses] slb
INNER JOIN STATEMASTER sm
 ON slb.StateId = sm.Id
INNER JOIN LineOfBusiness lb
 ON slb.LineOfBusinessId = lb.Id
WHERE sm.StateCode = 'MA'
 AND lb.abbreviation = 'BOP'
INSERT INTO StateLobStatus SELECT (NEXT VALUE FOR SequenceStateLobStatus), slb.Id, (SELECT Id FROM LobStatus WHERE Status = 'Available'), GETDATE(), DATEADD(YYYY, 5, GETDATE()), 1, GETDATE(), 1, GETDATE(), 1
FROM [StateLineOfBusinesses] slb
INNER JOIN STATEMASTER sm
 ON slb.StateId = sm.Id
INNER JOIN LineOfBusiness lb
 ON slb.LineOfBusinessId = lb.Id
WHERE sm.StateCode = 'MI'
 AND lb.abbreviation = 'BOP'
INSERT INTO StateLobStatus SELECT (NEXT VALUE FOR SequenceStateLobStatus), slb.Id, (SELECT Id FROM LobStatus WHERE Status = 'Available'), GETDATE(), DATEADD(YYYY, 5, GETDATE()), 1, GETDATE(), 1, GETDATE(), 1
FROM [StateLineOfBusinesses] slb
INNER JOIN STATEMASTER sm
 ON slb.StateId = sm.Id
INNER JOIN LineOfBusiness lb
 ON slb.LineOfBusinessId = lb.Id
WHERE sm.StateCode = 'MN'
 AND lb.abbreviation = 'BOP'
INSERT INTO StateLobStatus SELECT (NEXT VALUE FOR SequenceStateLobStatus), slb.Id, (SELECT Id FROM LobStatus WHERE Status = 'Available'), GETDATE(), DATEADD(YYYY, 5, GETDATE()), 1, GETDATE(), 1, GETDATE(), 1
FROM [StateLineOfBusinesses] slb
INNER JOIN STATEMASTER sm
 ON slb.StateId = sm.Id
INNER JOIN LineOfBusiness lb
 ON slb.LineOfBusinessId = lb.Id
WHERE sm.StateCode = 'MS'
 AND lb.abbreviation = 'BOP'
INSERT INTO StateLobStatus SELECT (NEXT VALUE FOR SequenceStateLobStatus), slb.Id, (SELECT Id FROM LobStatus WHERE Status = 'Available'), GETDATE(), DATEADD(YYYY, 5, GETDATE()), 1, GETDATE(), 1, GETDATE(), 1
FROM [StateLineOfBusinesses] slb
INNER JOIN STATEMASTER sm
 ON slb.StateId = sm.Id
INNER JOIN LineOfBusiness lb
 ON slb.LineOfBusinessId = lb.Id
WHERE sm.StateCode = 'MO'
 AND lb.abbreviation = 'BOP'
INSERT INTO StateLobStatus SELECT (NEXT VALUE FOR SequenceStateLobStatus), slb.Id, (SELECT Id FROM LobStatus WHERE Status = 'Not Available'), GETDATE(), DATEADD(YYYY, 5, GETDATE()), 1, GETDATE(), 1, GETDATE(), 1
FROM [StateLineOfBusinesses] slb
INNER JOIN STATEMASTER sm
 ON slb.StateId = sm.Id
INNER JOIN LineOfBusiness lb
 ON slb.LineOfBusinessId = lb.Id
WHERE sm.StateCode = 'MT'
 AND lb.abbreviation = 'BOP'
INSERT INTO StateLobStatus SELECT (NEXT VALUE FOR SequenceStateLobStatus), slb.Id, (SELECT Id FROM LobStatus WHERE Status = 'Available'), GETDATE(), DATEADD(YYYY, 5, GETDATE()), 1, GETDATE(), 1, GETDATE(), 1
FROM [StateLineOfBusinesses] slb
INNER JOIN STATEMASTER sm
 ON slb.StateId = sm.Id
INNER JOIN LineOfBusiness lb
 ON slb.LineOfBusinessId = lb.Id
WHERE sm.StateCode = 'NE'
 AND lb.abbreviation = 'BOP'
INSERT INTO StateLobStatus SELECT (NEXT VALUE FOR SequenceStateLobStatus), slb.Id, (SELECT Id FROM LobStatus WHERE Status = 'Available'), GETDATE(), DATEADD(YYYY, 5, GETDATE()), 1, GETDATE(), 1, GETDATE(), 1
FROM [StateLineOfBusinesses] slb
INNER JOIN STATEMASTER sm
 ON slb.StateId = sm.Id
INNER JOIN LineOfBusiness lb
 ON slb.LineOfBusinessId = lb.Id
WHERE sm.StateCode = 'NV'
 AND lb.abbreviation = 'BOP'
INSERT INTO StateLobStatus SELECT (NEXT VALUE FOR SequenceStateLobStatus), slb.Id, (SELECT Id FROM LobStatus WHERE Status = 'Available'), GETDATE(), DATEADD(YYYY, 5, GETDATE()), 1, GETDATE(), 1, GETDATE(), 1
FROM [StateLineOfBusinesses] slb
INNER JOIN STATEMASTER sm
 ON slb.StateId = sm.Id
INNER JOIN LineOfBusiness lb
 ON slb.LineOfBusinessId = lb.Id
WHERE sm.StateCode = 'NH'
 AND lb.abbreviation = 'BOP'
INSERT INTO StateLobStatus SELECT (NEXT VALUE FOR SequenceStateLobStatus), slb.Id, (SELECT Id FROM LobStatus WHERE Status = 'Available'), GETDATE(), DATEADD(YYYY, 5, GETDATE()), 1, GETDATE(), 1, GETDATE(), 1
FROM [StateLineOfBusinesses] slb
INNER JOIN STATEMASTER sm
 ON slb.StateId = sm.Id
INNER JOIN LineOfBusiness lb
 ON slb.LineOfBusinessId = lb.Id
WHERE sm.StateCode = 'NJ'
 AND lb.abbreviation = 'BOP'
INSERT INTO StateLobStatus SELECT (NEXT VALUE FOR SequenceStateLobStatus), slb.Id, (SELECT Id FROM LobStatus WHERE Status = 'Not Available'), GETDATE(), DATEADD(YYYY, 5, GETDATE()), 1, GETDATE(), 1, GETDATE(), 1
FROM [StateLineOfBusinesses] slb
INNER JOIN STATEMASTER sm
 ON slb.StateId = sm.Id
INNER JOIN LineOfBusiness lb
 ON slb.LineOfBusinessId = lb.Id
WHERE sm.StateCode = 'NM'
 AND lb.abbreviation = 'BOP'
INSERT INTO StateLobStatus SELECT (NEXT VALUE FOR SequenceStateLobStatus), slb.Id, (SELECT Id FROM LobStatus WHERE Status = 'Available'), GETDATE(), DATEADD(YYYY, 5, GETDATE()), 1, GETDATE(), 1, GETDATE(), 1
FROM [StateLineOfBusinesses] slb
INNER JOIN STATEMASTER sm
 ON slb.StateId = sm.Id
INNER JOIN LineOfBusiness lb
 ON slb.LineOfBusinessId = lb.Id
WHERE sm.StateCode = 'NY'
 AND lb.abbreviation = 'BOP'
INSERT INTO StateLobStatus SELECT (NEXT VALUE FOR SequenceStateLobStatus), slb.Id, (SELECT Id FROM LobStatus WHERE Status = 'Available'), GETDATE(), DATEADD(YYYY, 5, GETDATE()), 1, GETDATE(), 1, GETDATE(), 1
FROM [StateLineOfBusinesses] slb
INNER JOIN STATEMASTER sm
 ON slb.StateId = sm.Id
INNER JOIN LineOfBusiness lb
 ON slb.LineOfBusinessId = lb.Id
WHERE sm.StateCode = 'NC'
 AND lb.abbreviation = 'BOP'
INSERT INTO StateLobStatus SELECT (NEXT VALUE FOR SequenceStateLobStatus), slb.Id, (SELECT Id FROM LobStatus WHERE Status = 'Not Available'), GETDATE(), DATEADD(YYYY, 5, GETDATE()), 1, GETDATE(), 1, GETDATE(), 1
FROM [StateLineOfBusinesses] slb
INNER JOIN STATEMASTER sm
 ON slb.StateId = sm.Id
INNER JOIN LineOfBusiness lb
 ON slb.LineOfBusinessId = lb.Id
WHERE sm.StateCode = 'ND'
 AND lb.abbreviation = 'BOP'
INSERT INTO StateLobStatus SELECT (NEXT VALUE FOR SequenceStateLobStatus), slb.Id, (SELECT Id FROM LobStatus WHERE Status = 'Available'), GETDATE(), DATEADD(YYYY, 5, GETDATE()), 1, GETDATE(), 1, GETDATE(), 1
FROM [StateLineOfBusinesses] slb
INNER JOIN STATEMASTER sm
 ON slb.StateId = sm.Id
INNER JOIN LineOfBusiness lb
 ON slb.LineOfBusinessId = lb.Id
WHERE sm.StateCode = 'OH'
 AND lb.abbreviation = 'BOP'
INSERT INTO StateLobStatus SELECT (NEXT VALUE FOR SequenceStateLobStatus), slb.Id, (SELECT Id FROM LobStatus WHERE Status = 'Not Available'), GETDATE(), DATEADD(YYYY, 5, GETDATE()), 1, GETDATE(), 1, GETDATE(), 1
FROM [StateLineOfBusinesses] slb
INNER JOIN STATEMASTER sm
 ON slb.StateId = sm.Id
INNER JOIN LineOfBusiness lb
 ON slb.LineOfBusinessId = lb.Id
WHERE sm.StateCode = 'OK'
 AND lb.abbreviation = 'BOP'
INSERT INTO StateLobStatus SELECT (NEXT VALUE FOR SequenceStateLobStatus), slb.Id, (SELECT Id FROM LobStatus WHERE Status = 'Not Available'), GETDATE(), DATEADD(YYYY, 5, GETDATE()), 1, GETDATE(), 1, GETDATE(), 1
FROM [StateLineOfBusinesses] slb
INNER JOIN STATEMASTER sm
 ON slb.StateId = sm.Id
INNER JOIN LineOfBusiness lb
 ON slb.LineOfBusinessId = lb.Id
WHERE sm.StateCode = 'OR'
 AND lb.abbreviation = 'BOP'
INSERT INTO StateLobStatus SELECT (NEXT VALUE FOR SequenceStateLobStatus), slb.Id, (SELECT Id FROM LobStatus WHERE Status = 'Available'), GETDATE(), DATEADD(YYYY, 5, GETDATE()), 1, GETDATE(), 1, GETDATE(), 1
FROM [StateLineOfBusinesses] slb
INNER JOIN STATEMASTER sm
 ON slb.StateId = sm.Id
INNER JOIN LineOfBusiness lb
 ON slb.LineOfBusinessId = lb.Id
WHERE sm.StateCode = 'PA'
 AND lb.abbreviation = 'BOP'
INSERT INTO StateLobStatus SELECT (NEXT VALUE FOR SequenceStateLobStatus), slb.Id, (SELECT Id FROM LobStatus WHERE Status = 'Not Available'), GETDATE(), DATEADD(YYYY, 5, GETDATE()), 1, GETDATE(), 1, GETDATE(), 1
FROM [StateLineOfBusinesses] slb
INNER JOIN STATEMASTER sm
 ON slb.StateId = sm.Id
INNER JOIN LineOfBusiness lb
 ON slb.LineOfBusinessId = lb.Id
WHERE sm.StateCode = 'RI'
 AND lb.abbreviation = 'BOP'
INSERT INTO StateLobStatus SELECT (NEXT VALUE FOR SequenceStateLobStatus), slb.Id, (SELECT Id FROM LobStatus WHERE Status = 'Available'), GETDATE(), DATEADD(YYYY, 5, GETDATE()), 1, GETDATE(), 1, GETDATE(), 1
FROM [StateLineOfBusinesses] slb
INNER JOIN STATEMASTER sm
 ON slb.StateId = sm.Id
INNER JOIN LineOfBusiness lb
 ON slb.LineOfBusinessId = lb.Id
WHERE sm.StateCode = 'SC'
 AND lb.abbreviation = 'BOP'
INSERT INTO StateLobStatus SELECT (NEXT VALUE FOR SequenceStateLobStatus), slb.Id, (SELECT Id FROM LobStatus WHERE Status = 'Not Available'), GETDATE(), DATEADD(YYYY, 5, GETDATE()), 1, GETDATE(), 1, GETDATE(), 1
FROM [StateLineOfBusinesses] slb
INNER JOIN STATEMASTER sm
 ON slb.StateId = sm.Id
INNER JOIN LineOfBusiness lb
 ON slb.LineOfBusinessId = lb.Id
WHERE sm.StateCode = 'SD'
 AND lb.abbreviation = 'BOP'
INSERT INTO StateLobStatus SELECT (NEXT VALUE FOR SequenceStateLobStatus), slb.Id, (SELECT Id FROM LobStatus WHERE Status = 'Available'), GETDATE(), DATEADD(YYYY, 5, GETDATE()), 1, GETDATE(), 1, GETDATE(), 1
FROM [StateLineOfBusinesses] slb
INNER JOIN STATEMASTER sm
 ON slb.StateId = sm.Id
INNER JOIN LineOfBusiness lb
 ON slb.LineOfBusinessId = lb.Id
WHERE sm.StateCode = 'TN'
 AND lb.abbreviation = 'BOP'
INSERT INTO StateLobStatus SELECT (NEXT VALUE FOR SequenceStateLobStatus), slb.Id, (SELECT Id FROM LobStatus WHERE Status = 'Available'), GETDATE(), DATEADD(YYYY, 5, GETDATE()), 1, GETDATE(), 1, GETDATE(), 1
FROM [StateLineOfBusinesses] slb
INNER JOIN STATEMASTER sm
 ON slb.StateId = sm.Id
INNER JOIN LineOfBusiness lb
 ON slb.LineOfBusinessId = lb.Id
WHERE sm.StateCode = 'TX'
 AND lb.abbreviation = 'BOP'
INSERT INTO StateLobStatus SELECT (NEXT VALUE FOR SequenceStateLobStatus), slb.Id, (SELECT Id FROM LobStatus WHERE Status = 'Not Available'), GETDATE(), DATEADD(YYYY, 5, GETDATE()), 1, GETDATE(), 1, GETDATE(), 1
FROM [StateLineOfBusinesses] slb
INNER JOIN STATEMASTER sm
 ON slb.StateId = sm.Id
INNER JOIN LineOfBusiness lb
 ON slb.LineOfBusinessId = lb.Id
WHERE sm.StateCode = 'UT'
 AND lb.abbreviation = 'BOP'
INSERT INTO StateLobStatus SELECT (NEXT VALUE FOR SequenceStateLobStatus), slb.Id, (SELECT Id FROM LobStatus WHERE Status = 'Not Available'), GETDATE(), DATEADD(YYYY, 5, GETDATE()), 1, GETDATE(), 1, GETDATE(), 1
FROM [StateLineOfBusinesses] slb
INNER JOIN STATEMASTER sm
 ON slb.StateId = sm.Id
INNER JOIN LineOfBusiness lb
 ON slb.LineOfBusinessId = lb.Id
WHERE sm.StateCode = 'VT'
 AND lb.abbreviation = 'BOP'
INSERT INTO StateLobStatus SELECT (NEXT VALUE FOR SequenceStateLobStatus), slb.Id, (SELECT Id FROM LobStatus WHERE Status = 'Coming Soon'), GETDATE(), DATEADD(YYYY, 5, GETDATE()), 1, GETDATE(), 1, GETDATE(), 1
FROM [StateLineOfBusinesses] slb
INNER JOIN STATEMASTER sm
 ON slb.StateId = sm.Id
INNER JOIN LineOfBusiness lb
 ON slb.LineOfBusinessId = lb.Id
WHERE sm.StateCode = 'VA'
 AND lb.abbreviation = 'BOP'
INSERT INTO StateLobStatus SELECT (NEXT VALUE FOR SequenceStateLobStatus), slb.Id, (SELECT Id FROM LobStatus WHERE Status = 'Available'), GETDATE(), DATEADD(YYYY, 5, GETDATE()), 1, GETDATE(), 1, GETDATE(), 1
FROM [StateLineOfBusinesses] slb
INNER JOIN STATEMASTER sm
 ON slb.StateId = sm.Id
INNER JOIN LineOfBusiness lb
 ON slb.LineOfBusinessId = lb.Id
WHERE sm.StateCode = 'WA'
 AND lb.abbreviation = 'BOP'
INSERT INTO StateLobStatus SELECT (NEXT VALUE FOR SequenceStateLobStatus), slb.Id, (SELECT Id FROM LobStatus WHERE Status = 'Not Available'), GETDATE(), DATEADD(YYYY, 5, GETDATE()), 1, GETDATE(), 1, GETDATE(), 1
FROM [StateLineOfBusinesses] slb
INNER JOIN STATEMASTER sm
 ON slb.StateId = sm.Id
INNER JOIN LineOfBusiness lb
 ON slb.LineOfBusinessId = lb.Id
WHERE sm.StateCode = 'WV'
 AND lb.abbreviation = 'BOP'
INSERT INTO StateLobStatus SELECT (NEXT VALUE FOR SequenceStateLobStatus), slb.Id, (SELECT Id FROM LobStatus WHERE Status = 'Not Available'), GETDATE(), DATEADD(YYYY, 5, GETDATE()), 1, GETDATE(), 1, GETDATE(), 1
FROM [StateLineOfBusinesses] slb
INNER JOIN STATEMASTER sm
 ON slb.StateId = sm.Id
INNER JOIN LineOfBusiness lb
 ON slb.LineOfBusinessId = lb.Id
WHERE sm.StateCode = 'WI'
 AND lb.abbreviation = 'BOP'
INSERT INTO StateLobStatus SELECT (NEXT VALUE FOR SequenceStateLobStatus), slb.Id, (SELECT Id FROM LobStatus WHERE Status = 'Not Available'), GETDATE(), DATEADD(YYYY, 5, GETDATE()), 1, GETDATE(), 1, GETDATE(), 1
FROM [StateLineOfBusinesses] slb
INNER JOIN STATEMASTER sm
 ON slb.StateId = sm.Id
INNER JOIN LineOfBusiness lb
 ON slb.LineOfBusinessId = lb.Id
WHERE sm.StateCode = 'WY'
 AND lb.abbreviation = 'BOP'

INSERT INTO StateLobStatus SELECT (NEXT VALUE FOR SequenceStateLobStatus), slb.Id, (SELECT Id FROM LobStatus WHERE Status = 'Available'), GETDATE(), DATEADD(YYYY, 5, GETDATE()), 1, GETDATE(), 1, GETDATE(), 1
FROM [StateLineOfBusinesses] slb
INNER JOIN STATEMASTER sm
 ON slb.StateId = sm.Id
INNER JOIN LineOfBusiness lb
 ON slb.LineOfBusinessId = lb.Id
WHERE sm.StateCode = 'AL'
 AND lb.abbreviation = 'WC'
INSERT INTO StateLobStatus SELECT (NEXT VALUE FOR SequenceStateLobStatus), slb.Id, (SELECT Id FROM LobStatus WHERE Status = 'Available'), GETDATE(), DATEADD(YYYY, 5, GETDATE()), 1, GETDATE(), 1, GETDATE(), 1
FROM [StateLineOfBusinesses] slb
INNER JOIN STATEMASTER sm
 ON slb.StateId = sm.Id
INNER JOIN LineOfBusiness lb
 ON slb.LineOfBusinessId = lb.Id
WHERE sm.StateCode = 'AK'
 AND lb.abbreviation = 'WC'
INSERT INTO StateLobStatus SELECT (NEXT VALUE FOR SequenceStateLobStatus), slb.Id, (SELECT Id FROM LobStatus WHERE Status = 'Available'), GETDATE(), DATEADD(YYYY, 5, GETDATE()), 1, GETDATE(), 1, GETDATE(), 1
FROM [StateLineOfBusinesses] slb
INNER JOIN STATEMASTER sm
 ON slb.StateId = sm.Id
INNER JOIN LineOfBusiness lb
 ON slb.LineOfBusinessId = lb.Id
WHERE sm.StateCode = 'AZ'
 AND lb.abbreviation = 'WC'
INSERT INTO StateLobStatus SELECT (NEXT VALUE FOR SequenceStateLobStatus), slb.Id, (SELECT Id FROM LobStatus WHERE Status = 'Available'), GETDATE(), DATEADD(YYYY, 5, GETDATE()), 1, GETDATE(), 1, GETDATE(), 1
FROM [StateLineOfBusinesses] slb
INNER JOIN STATEMASTER sm
 ON slb.StateId = sm.Id
INNER JOIN LineOfBusiness lb
 ON slb.LineOfBusinessId = lb.Id
WHERE sm.StateCode = 'AR'
 AND lb.abbreviation = 'WC'
INSERT INTO StateLobStatus SELECT (NEXT VALUE FOR SequenceStateLobStatus), slb.Id, (SELECT Id FROM LobStatus WHERE Status = 'Available'), GETDATE(), DATEADD(YYYY, 5, GETDATE()), 1, GETDATE(), 1, GETDATE(), 1
FROM [StateLineOfBusinesses] slb
INNER JOIN STATEMASTER sm
 ON slb.StateId = sm.Id
INNER JOIN LineOfBusiness lb
 ON slb.LineOfBusinessId = lb.Id
WHERE sm.StateCode = 'CA'
 AND lb.abbreviation = 'WC'
INSERT INTO StateLobStatus SELECT (NEXT VALUE FOR SequenceStateLobStatus), slb.Id, (SELECT Id FROM LobStatus WHERE Status = 'Available'), GETDATE(), DATEADD(YYYY, 5, GETDATE()), 1, GETDATE(), 1, GETDATE(), 1
FROM [StateLineOfBusinesses] slb
INNER JOIN STATEMASTER sm
 ON slb.StateId = sm.Id
INNER JOIN LineOfBusiness lb
 ON slb.LineOfBusinessId = lb.Id
WHERE sm.StateCode = 'CO'
 AND lb.abbreviation = 'WC'
INSERT INTO StateLobStatus SELECT (NEXT VALUE FOR SequenceStateLobStatus), slb.Id, (SELECT Id FROM LobStatus WHERE Status = 'Available'), GETDATE(), DATEADD(YYYY, 5, GETDATE()), 1, GETDATE(), 1, GETDATE(), 1
FROM [StateLineOfBusinesses] slb
INNER JOIN STATEMASTER sm
 ON slb.StateId = sm.Id
INNER JOIN LineOfBusiness lb
 ON slb.LineOfBusinessId = lb.Id
WHERE sm.StateCode = 'CT'
 AND lb.abbreviation = 'WC'
INSERT INTO StateLobStatus SELECT (NEXT VALUE FOR SequenceStateLobStatus), slb.Id, (SELECT Id FROM LobStatus WHERE Status = 'Available'), GETDATE(), DATEADD(YYYY, 5, GETDATE()), 1, GETDATE(), 1, GETDATE(), 1
FROM [StateLineOfBusinesses] slb
INNER JOIN STATEMASTER sm
 ON slb.StateId = sm.Id
INNER JOIN LineOfBusiness lb
 ON slb.LineOfBusinessId = lb.Id
WHERE sm.StateCode = 'DE'
 AND lb.abbreviation = 'WC'
INSERT INTO StateLobStatus SELECT (NEXT VALUE FOR SequenceStateLobStatus), slb.Id, (SELECT Id FROM LobStatus WHERE Status = 'Available'), GETDATE(), DATEADD(YYYY, 5, GETDATE()), 1, GETDATE(), 1, GETDATE(), 1
FROM [StateLineOfBusinesses] slb
INNER JOIN STATEMASTER sm
 ON slb.StateId = sm.Id
INNER JOIN LineOfBusiness lb
 ON slb.LineOfBusinessId = lb.Id
WHERE sm.StateCode = 'DC'
 AND lb.abbreviation = 'WC'
INSERT INTO StateLobStatus SELECT (NEXT VALUE FOR SequenceStateLobStatus), slb.Id, (SELECT Id FROM LobStatus WHERE Status = 'Available'), GETDATE(), DATEADD(YYYY, 5, GETDATE()), 1, GETDATE(), 1, GETDATE(), 1
FROM [StateLineOfBusinesses] slb
INNER JOIN STATEMASTER sm
 ON slb.StateId = sm.Id
INNER JOIN LineOfBusiness lb
 ON slb.LineOfBusinessId = lb.Id
WHERE sm.StateCode = 'FL'
 AND lb.abbreviation = 'WC'
INSERT INTO StateLobStatus SELECT (NEXT VALUE FOR SequenceStateLobStatus), slb.Id, (SELECT Id FROM LobStatus WHERE Status = 'Available'), GETDATE(), DATEADD(YYYY, 5, GETDATE()), 1, GETDATE(), 1, GETDATE(), 1
FROM [StateLineOfBusinesses] slb
INNER JOIN STATEMASTER sm
 ON slb.StateId = sm.Id
INNER JOIN LineOfBusiness lb
 ON slb.LineOfBusinessId = lb.Id
WHERE sm.StateCode = 'GA'
 AND lb.abbreviation = 'WC'
INSERT INTO StateLobStatus SELECT (NEXT VALUE FOR SequenceStateLobStatus), slb.Id, (SELECT Id FROM LobStatus WHERE Status = 'Available'), GETDATE(), DATEADD(YYYY, 5, GETDATE()), 1, GETDATE(), 1, GETDATE(), 1
FROM [StateLineOfBusinesses] slb
INNER JOIN STATEMASTER sm
 ON slb.StateId = sm.Id
INNER JOIN LineOfBusiness lb
 ON slb.LineOfBusinessId = lb.Id
WHERE sm.StateCode = 'HI'
 AND lb.abbreviation = 'WC'
INSERT INTO StateLobStatus SELECT (NEXT VALUE FOR SequenceStateLobStatus), slb.Id, (SELECT Id FROM LobStatus WHERE Status = 'Available'), GETDATE(), DATEADD(YYYY, 5, GETDATE()), 1, GETDATE(), 1, GETDATE(), 1
FROM [StateLineOfBusinesses] slb
INNER JOIN STATEMASTER sm
 ON slb.StateId = sm.Id
INNER JOIN LineOfBusiness lb
 ON slb.LineOfBusinessId = lb.Id
WHERE sm.StateCode = 'ID'
 AND lb.abbreviation = 'WC'
INSERT INTO StateLobStatus SELECT (NEXT VALUE FOR SequenceStateLobStatus), slb.Id, (SELECT Id FROM LobStatus WHERE Status = 'Available'), GETDATE(), DATEADD(YYYY, 5, GETDATE()), 1, GETDATE(), 1, GETDATE(), 1
FROM [StateLineOfBusinesses] slb
INNER JOIN STATEMASTER sm
 ON slb.StateId = sm.Id
INNER JOIN LineOfBusiness lb
 ON slb.LineOfBusinessId = lb.Id
WHERE sm.StateCode = 'IL'
 AND lb.abbreviation = 'WC'
INSERT INTO StateLobStatus SELECT (NEXT VALUE FOR SequenceStateLobStatus), slb.Id, (SELECT Id FROM LobStatus WHERE Status = 'Available'), GETDATE(), DATEADD(YYYY, 5, GETDATE()), 1, GETDATE(), 1, GETDATE(), 1
FROM [StateLineOfBusinesses] slb
INNER JOIN STATEMASTER sm
 ON slb.StateId = sm.Id
INNER JOIN LineOfBusiness lb
 ON slb.LineOfBusinessId = lb.Id
WHERE sm.StateCode = 'IN'
 AND lb.abbreviation = 'WC'
INSERT INTO StateLobStatus SELECT (NEXT VALUE FOR SequenceStateLobStatus), slb.Id, (SELECT Id FROM LobStatus WHERE Status = 'Available'), GETDATE(), DATEADD(YYYY, 5, GETDATE()), 1, GETDATE(), 1, GETDATE(), 1
FROM [StateLineOfBusinesses] slb
INNER JOIN STATEMASTER sm
 ON slb.StateId = sm.Id
INNER JOIN LineOfBusiness lb
 ON slb.LineOfBusinessId = lb.Id
WHERE sm.StateCode = 'IA'
 AND lb.abbreviation = 'WC'
INSERT INTO StateLobStatus SELECT (NEXT VALUE FOR SequenceStateLobStatus), slb.Id, (SELECT Id FROM LobStatus WHERE Status = 'Available'), GETDATE(), DATEADD(YYYY, 5, GETDATE()), 1, GETDATE(), 1, GETDATE(), 1
FROM [StateLineOfBusinesses] slb
INNER JOIN STATEMASTER sm
 ON slb.StateId = sm.Id
INNER JOIN LineOfBusiness lb
 ON slb.LineOfBusinessId = lb.Id
WHERE sm.StateCode = 'KS'
 AND lb.abbreviation = 'WC'
INSERT INTO StateLobStatus SELECT (NEXT VALUE FOR SequenceStateLobStatus), slb.Id, (SELECT Id FROM LobStatus WHERE Status = 'Available'), GETDATE(), DATEADD(YYYY, 5, GETDATE()), 1, GETDATE(), 1, GETDATE(), 1
FROM [StateLineOfBusinesses] slb
INNER JOIN STATEMASTER sm
 ON slb.StateId = sm.Id
INNER JOIN LineOfBusiness lb
 ON slb.LineOfBusinessId = lb.Id
WHERE sm.StateCode = 'KY'
 AND lb.abbreviation = 'WC'
INSERT INTO StateLobStatus SELECT (NEXT VALUE FOR SequenceStateLobStatus), slb.Id, (SELECT Id FROM LobStatus WHERE Status = 'Available'), GETDATE(), DATEADD(YYYY, 5, GETDATE()), 1, GETDATE(), 1, GETDATE(), 1
FROM [StateLineOfBusinesses] slb
INNER JOIN STATEMASTER sm
 ON slb.StateId = sm.Id
INNER JOIN LineOfBusiness lb
 ON slb.LineOfBusinessId = lb.Id
WHERE sm.StateCode = 'LA'
 AND lb.abbreviation = 'WC'
INSERT INTO StateLobStatus SELECT (NEXT VALUE FOR SequenceStateLobStatus), slb.Id, (SELECT Id FROM LobStatus WHERE Status = 'Available'), GETDATE(), DATEADD(YYYY, 5, GETDATE()), 1, GETDATE(), 1, GETDATE(), 1
FROM [StateLineOfBusinesses] slb
INNER JOIN STATEMASTER sm
 ON slb.StateId = sm.Id
INNER JOIN LineOfBusiness lb
 ON slb.LineOfBusinessId = lb.Id
WHERE sm.StateCode = 'ME'
 AND lb.abbreviation = 'WC'
INSERT INTO StateLobStatus SELECT (NEXT VALUE FOR SequenceStateLobStatus), slb.Id, (SELECT Id FROM LobStatus WHERE Status = 'Available'), GETDATE(), DATEADD(YYYY, 5, GETDATE()), 1, GETDATE(), 1, GETDATE(), 1
FROM [StateLineOfBusinesses] slb
INNER JOIN STATEMASTER sm
 ON slb.StateId = sm.Id
INNER JOIN LineOfBusiness lb
 ON slb.LineOfBusinessId = lb.Id
WHERE sm.StateCode = 'MD'
 AND lb.abbreviation = 'WC'
INSERT INTO StateLobStatus SELECT (NEXT VALUE FOR SequenceStateLobStatus), slb.Id, (SELECT Id FROM LobStatus WHERE Status = 'Available'), GETDATE(), DATEADD(YYYY, 5, GETDATE()), 1, GETDATE(), 1, GETDATE(), 1
FROM [StateLineOfBusinesses] slb
INNER JOIN STATEMASTER sm
 ON slb.StateId = sm.Id
INNER JOIN LineOfBusiness lb
 ON slb.LineOfBusinessId = lb.Id
WHERE sm.StateCode = 'MA'
 AND lb.abbreviation = 'WC'
INSERT INTO StateLobStatus SELECT (NEXT VALUE FOR SequenceStateLobStatus), slb.Id, (SELECT Id FROM LobStatus WHERE Status = 'Available'), GETDATE(), DATEADD(YYYY, 5, GETDATE()), 1, GETDATE(), 1, GETDATE(), 1
FROM [StateLineOfBusinesses] slb
INNER JOIN STATEMASTER sm
 ON slb.StateId = sm.Id
INNER JOIN LineOfBusiness lb
 ON slb.LineOfBusinessId = lb.Id
WHERE sm.StateCode = 'MI'
 AND lb.abbreviation = 'WC'
INSERT INTO StateLobStatus SELECT (NEXT VALUE FOR SequenceStateLobStatus), slb.Id, (SELECT Id FROM LobStatus WHERE Status = 'Available'), GETDATE(), DATEADD(YYYY, 5, GETDATE()), 1, GETDATE(), 1, GETDATE(), 1
FROM [StateLineOfBusinesses] slb
INNER JOIN STATEMASTER sm
 ON slb.StateId = sm.Id
INNER JOIN LineOfBusiness lb
 ON slb.LineOfBusinessId = lb.Id
WHERE sm.StateCode = 'MN'
 AND lb.abbreviation = 'WC'
INSERT INTO StateLobStatus SELECT (NEXT VALUE FOR SequenceStateLobStatus), slb.Id, (SELECT Id FROM LobStatus WHERE Status = 'Available'), GETDATE(), DATEADD(YYYY, 5, GETDATE()), 1, GETDATE(), 1, GETDATE(), 1
FROM [StateLineOfBusinesses] slb
INNER JOIN STATEMASTER sm
 ON slb.StateId = sm.Id
INNER JOIN LineOfBusiness lb
 ON slb.LineOfBusinessId = lb.Id
WHERE sm.StateCode = 'MS'
 AND lb.abbreviation = 'WC'
INSERT INTO StateLobStatus SELECT (NEXT VALUE FOR SequenceStateLobStatus), slb.Id, (SELECT Id FROM LobStatus WHERE Status = 'Available'), GETDATE(), DATEADD(YYYY, 5, GETDATE()), 1, GETDATE(), 1, GETDATE(), 1
FROM [StateLineOfBusinesses] slb
INNER JOIN STATEMASTER sm
 ON slb.StateId = sm.Id
INNER JOIN LineOfBusiness lb
 ON slb.LineOfBusinessId = lb.Id
WHERE sm.StateCode = 'MO'
 AND lb.abbreviation = 'WC'
INSERT INTO StateLobStatus SELECT (NEXT VALUE FOR SequenceStateLobStatus), slb.Id, (SELECT Id FROM LobStatus WHERE Status = 'Available'), GETDATE(), DATEADD(YYYY, 5, GETDATE()), 1, GETDATE(), 1, GETDATE(), 1
FROM [StateLineOfBusinesses] slb
INNER JOIN STATEMASTER sm
 ON slb.StateId = sm.Id
INNER JOIN LineOfBusiness lb
 ON slb.LineOfBusinessId = lb.Id
WHERE sm.StateCode = 'MT'
 AND lb.abbreviation = 'WC'
INSERT INTO StateLobStatus SELECT (NEXT VALUE FOR SequenceStateLobStatus), slb.Id, (SELECT Id FROM LobStatus WHERE Status = 'Available'), GETDATE(), DATEADD(YYYY, 5, GETDATE()), 1, GETDATE(), 1, GETDATE(), 1
FROM [StateLineOfBusinesses] slb
INNER JOIN STATEMASTER sm
 ON slb.StateId = sm.Id
INNER JOIN LineOfBusiness lb
 ON slb.LineOfBusinessId = lb.Id
WHERE sm.StateCode = 'NE'
 AND lb.abbreviation = 'WC'
INSERT INTO StateLobStatus SELECT (NEXT VALUE FOR SequenceStateLobStatus), slb.Id, (SELECT Id FROM LobStatus WHERE Status = 'Available'), GETDATE(), DATEADD(YYYY, 5, GETDATE()), 1, GETDATE(), 1, GETDATE(), 1
FROM [StateLineOfBusinesses] slb
INNER JOIN STATEMASTER sm
 ON slb.StateId = sm.Id
INNER JOIN LineOfBusiness lb
 ON slb.LineOfBusinessId = lb.Id
WHERE sm.StateCode = 'NV'
 AND lb.abbreviation = 'WC'
INSERT INTO StateLobStatus SELECT (NEXT VALUE FOR SequenceStateLobStatus), slb.Id, (SELECT Id FROM LobStatus WHERE Status = 'Available'), GETDATE(), DATEADD(YYYY, 5, GETDATE()), 1, GETDATE(), 1, GETDATE(), 1
FROM [StateLineOfBusinesses] slb
INNER JOIN STATEMASTER sm
 ON slb.StateId = sm.Id
INNER JOIN LineOfBusiness lb
 ON slb.LineOfBusinessId = lb.Id
WHERE sm.StateCode = 'NH'
 AND lb.abbreviation = 'WC'
INSERT INTO StateLobStatus SELECT (NEXT VALUE FOR SequenceStateLobStatus), slb.Id, (SELECT Id FROM LobStatus WHERE Status = 'Available'), GETDATE(), DATEADD(YYYY, 5, GETDATE()), 1, GETDATE(), 1, GETDATE(), 1
FROM [StateLineOfBusinesses] slb
INNER JOIN STATEMASTER sm
 ON slb.StateId = sm.Id
INNER JOIN LineOfBusiness lb
 ON slb.LineOfBusinessId = lb.Id
WHERE sm.StateCode = 'NJ'
 AND lb.abbreviation = 'WC'
INSERT INTO StateLobStatus SELECT (NEXT VALUE FOR SequenceStateLobStatus), slb.Id, (SELECT Id FROM LobStatus WHERE Status = 'Available'), GETDATE(), DATEADD(YYYY, 5, GETDATE()), 1, GETDATE(), 1, GETDATE(), 1
FROM [StateLineOfBusinesses] slb
INNER JOIN STATEMASTER sm
 ON slb.StateId = sm.Id
INNER JOIN LineOfBusiness lb
 ON slb.LineOfBusinessId = lb.Id
WHERE sm.StateCode = 'NM'
 AND lb.abbreviation = 'WC'
INSERT INTO StateLobStatus SELECT (NEXT VALUE FOR SequenceStateLobStatus), slb.Id, (SELECT Id FROM LobStatus WHERE Status = 'Available'), GETDATE(), DATEADD(YYYY, 5, GETDATE()), 1, GETDATE(), 1, GETDATE(), 1
FROM [StateLineOfBusinesses] slb
INNER JOIN STATEMASTER sm
 ON slb.StateId = sm.Id
INNER JOIN LineOfBusiness lb
 ON slb.LineOfBusinessId = lb.Id
WHERE sm.StateCode = 'NY'
 AND lb.abbreviation = 'WC'
INSERT INTO StateLobStatus SELECT (NEXT VALUE FOR SequenceStateLobStatus), slb.Id, (SELECT Id FROM LobStatus WHERE Status = 'Available'), GETDATE(), DATEADD(YYYY, 5, GETDATE()), 1, GETDATE(), 1, GETDATE(), 1
FROM [StateLineOfBusinesses] slb
INNER JOIN STATEMASTER sm
 ON slb.StateId = sm.Id
INNER JOIN LineOfBusiness lb
 ON slb.LineOfBusinessId = lb.Id
WHERE sm.StateCode = 'NC'
 AND lb.abbreviation = 'WC'
INSERT INTO StateLobStatus SELECT (NEXT VALUE FOR SequenceStateLobStatus), slb.Id, (SELECT Id FROM LobStatus WHERE Status = 'Not Available'), GETDATE(), DATEADD(YYYY, 5, GETDATE()), 1, GETDATE(), 1, GETDATE(), 1
FROM [StateLineOfBusinesses] slb
INNER JOIN STATEMASTER sm
 ON slb.StateId = sm.Id
INNER JOIN LineOfBusiness lb
 ON slb.LineOfBusinessId = lb.Id
WHERE sm.StateCode = 'ND'
 AND lb.abbreviation = 'WC'
INSERT INTO StateLobStatus SELECT (NEXT VALUE FOR SequenceStateLobStatus), slb.Id, (SELECT Id FROM LobStatus WHERE Status = 'Not Available'), GETDATE(), DATEADD(YYYY, 5, GETDATE()), 1, GETDATE(), 1, GETDATE(), 1
FROM [StateLineOfBusinesses] slb
INNER JOIN STATEMASTER sm
 ON slb.StateId = sm.Id
INNER JOIN LineOfBusiness lb
 ON slb.LineOfBusinessId = lb.Id
WHERE sm.StateCode = 'OH'
 AND lb.abbreviation = 'WC'
INSERT INTO StateLobStatus SELECT (NEXT VALUE FOR SequenceStateLobStatus), slb.Id, (SELECT Id FROM LobStatus WHERE Status = 'Available'), GETDATE(), DATEADD(YYYY, 5, GETDATE()), 1, GETDATE(), 1, GETDATE(), 1
FROM [StateLineOfBusinesses] slb
INNER JOIN STATEMASTER sm
 ON slb.StateId = sm.Id
INNER JOIN LineOfBusiness lb
 ON slb.LineOfBusinessId = lb.Id
WHERE sm.StateCode = 'OK'
 AND lb.abbreviation = 'WC'
INSERT INTO StateLobStatus SELECT (NEXT VALUE FOR SequenceStateLobStatus), slb.Id, (SELECT Id FROM LobStatus WHERE Status = 'Available'), GETDATE(), DATEADD(YYYY, 5, GETDATE()), 1, GETDATE(), 1, GETDATE(), 1
FROM [StateLineOfBusinesses] slb
INNER JOIN STATEMASTER sm
 ON slb.StateId = sm.Id
INNER JOIN LineOfBusiness lb
 ON slb.LineOfBusinessId = lb.Id
WHERE sm.StateCode = 'OR'
 AND lb.abbreviation = 'WC'
INSERT INTO StateLobStatus SELECT (NEXT VALUE FOR SequenceStateLobStatus), slb.Id, (SELECT Id FROM LobStatus WHERE Status = 'Available'), GETDATE(), DATEADD(YYYY, 5, GETDATE()), 1, GETDATE(), 1, GETDATE(), 1
FROM [StateLineOfBusinesses] slb
INNER JOIN STATEMASTER sm
 ON slb.StateId = sm.Id
INNER JOIN LineOfBusiness lb
 ON slb.LineOfBusinessId = lb.Id
WHERE sm.StateCode = 'PA'
 AND lb.abbreviation = 'WC'
INSERT INTO StateLobStatus SELECT (NEXT VALUE FOR SequenceStateLobStatus), slb.Id, (SELECT Id FROM LobStatus WHERE Status = 'Available'), GETDATE(), DATEADD(YYYY, 5, GETDATE()), 1, GETDATE(), 1, GETDATE(), 1
FROM [StateLineOfBusinesses] slb
INNER JOIN STATEMASTER sm
 ON slb.StateId = sm.Id
INNER JOIN LineOfBusiness lb
 ON slb.LineOfBusinessId = lb.Id
WHERE sm.StateCode = 'RI'
 AND lb.abbreviation = 'WC'
INSERT INTO StateLobStatus SELECT (NEXT VALUE FOR SequenceStateLobStatus), slb.Id, (SELECT Id FROM LobStatus WHERE Status = 'Available'), GETDATE(), DATEADD(YYYY, 5, GETDATE()), 1, GETDATE(), 1, GETDATE(), 1
FROM [StateLineOfBusinesses] slb
INNER JOIN STATEMASTER sm
 ON slb.StateId = sm.Id
INNER JOIN LineOfBusiness lb
 ON slb.LineOfBusinessId = lb.Id
WHERE sm.StateCode = 'SC'
 AND lb.abbreviation = 'WC'
INSERT INTO StateLobStatus SELECT (NEXT VALUE FOR SequenceStateLobStatus), slb.Id, (SELECT Id FROM LobStatus WHERE Status = 'Available'), GETDATE(), DATEADD(YYYY, 5, GETDATE()), 1, GETDATE(), 1, GETDATE(), 1
FROM [StateLineOfBusinesses] slb
INNER JOIN STATEMASTER sm
 ON slb.StateId = sm.Id
INNER JOIN LineOfBusiness lb
 ON slb.LineOfBusinessId = lb.Id
WHERE sm.StateCode = 'SD'
 AND lb.abbreviation = 'WC'
INSERT INTO StateLobStatus SELECT (NEXT VALUE FOR SequenceStateLobStatus), slb.Id, (SELECT Id FROM LobStatus WHERE Status = 'Available'), GETDATE(), DATEADD(YYYY, 5, GETDATE()), 1, GETDATE(), 1, GETDATE(), 1
FROM [StateLineOfBusinesses] slb
INNER JOIN STATEMASTER sm
 ON slb.StateId = sm.Id
INNER JOIN LineOfBusiness lb
 ON slb.LineOfBusinessId = lb.Id
WHERE sm.StateCode = 'TN'
 AND lb.abbreviation = 'WC'
INSERT INTO StateLobStatus SELECT (NEXT VALUE FOR SequenceStateLobStatus), slb.Id, (SELECT Id FROM LobStatus WHERE Status = 'Available'), GETDATE(), DATEADD(YYYY, 5, GETDATE()), 1, GETDATE(), 1, GETDATE(), 1
FROM [StateLineOfBusinesses] slb
INNER JOIN STATEMASTER sm
 ON slb.StateId = sm.Id
INNER JOIN LineOfBusiness lb
 ON slb.LineOfBusinessId = lb.Id
WHERE sm.StateCode = 'TX'
 AND lb.abbreviation = 'WC'
INSERT INTO StateLobStatus SELECT (NEXT VALUE FOR SequenceStateLobStatus), slb.Id, (SELECT Id FROM LobStatus WHERE Status = 'Available'), GETDATE(), DATEADD(YYYY, 5, GETDATE()), 1, GETDATE(), 1, GETDATE(), 1
FROM [StateLineOfBusinesses] slb
INNER JOIN STATEMASTER sm
 ON slb.StateId = sm.Id
INNER JOIN LineOfBusiness lb
 ON slb.LineOfBusinessId = lb.Id
WHERE sm.StateCode = 'UT'
 AND lb.abbreviation = 'WC'
INSERT INTO StateLobStatus SELECT (NEXT VALUE FOR SequenceStateLobStatus), slb.Id, (SELECT Id FROM LobStatus WHERE Status = 'Available'), GETDATE(), DATEADD(YYYY, 5, GETDATE()), 1, GETDATE(), 1, GETDATE(), 1
FROM [StateLineOfBusinesses] slb
INNER JOIN STATEMASTER sm
 ON slb.StateId = sm.Id
INNER JOIN LineOfBusiness lb
 ON slb.LineOfBusinessId = lb.Id
WHERE sm.StateCode = 'VT'
 AND lb.abbreviation = 'WC'
INSERT INTO StateLobStatus SELECT (NEXT VALUE FOR SequenceStateLobStatus), slb.Id, (SELECT Id FROM LobStatus WHERE Status = 'Available'), GETDATE(), DATEADD(YYYY, 5, GETDATE()), 1, GETDATE(), 1, GETDATE(), 1
FROM [StateLineOfBusinesses] slb
INNER JOIN STATEMASTER sm
 ON slb.StateId = sm.Id
INNER JOIN LineOfBusiness lb
 ON slb.LineOfBusinessId = lb.Id
WHERE sm.StateCode = 'VA'
 AND lb.abbreviation = 'WC'
INSERT INTO StateLobStatus SELECT (NEXT VALUE FOR SequenceStateLobStatus), slb.Id, (SELECT Id FROM LobStatus WHERE Status = 'Not Available'), GETDATE(), DATEADD(YYYY, 5, GETDATE()), 1, GETDATE(), 1, GETDATE(), 1
FROM [StateLineOfBusinesses] slb
INNER JOIN STATEMASTER sm
 ON slb.StateId = sm.Id
INNER JOIN LineOfBusiness lb
 ON slb.LineOfBusinessId = lb.Id
WHERE sm.StateCode = 'WA'
 AND lb.abbreviation = 'WC'
INSERT INTO StateLobStatus SELECT (NEXT VALUE FOR SequenceStateLobStatus), slb.Id, (SELECT Id FROM LobStatus WHERE Status = 'Available'), GETDATE(), DATEADD(YYYY, 5, GETDATE()), 1, GETDATE(), 1, GETDATE(), 1
FROM [StateLineOfBusinesses] slb
INNER JOIN STATEMASTER sm
 ON slb.StateId = sm.Id
INNER JOIN LineOfBusiness lb
 ON slb.LineOfBusinessId = lb.Id
WHERE sm.StateCode = 'WV'
 AND lb.abbreviation = 'WC'
INSERT INTO StateLobStatus SELECT (NEXT VALUE FOR SequenceStateLobStatus), slb.Id, (SELECT Id FROM LobStatus WHERE Status = 'Available'), GETDATE(), DATEADD(YYYY, 5, GETDATE()), 1, GETDATE(), 1, GETDATE(), 1
FROM [StateLineOfBusinesses] slb
INNER JOIN STATEMASTER sm
 ON slb.StateId = sm.Id
INNER JOIN LineOfBusiness lb
 ON slb.LineOfBusinessId = lb.Id
WHERE sm.StateCode = 'WI'
 AND lb.abbreviation = 'WC'
INSERT INTO StateLobStatus SELECT (NEXT VALUE FOR SequenceStateLobStatus), slb.Id, (SELECT Id FROM LobStatus WHERE Status = 'Not Available'), GETDATE(), DATEADD(YYYY, 5, GETDATE()), 1, GETDATE(), 1, GETDATE(), 1
FROM [StateLineOfBusinesses] slb
INNER JOIN STATEMASTER sm
 ON slb.StateId = sm.Id
INNER JOIN LineOfBusiness lb
 ON slb.LineOfBusinessId = lb.Id
WHERE sm.StateCode = 'WY'
 AND lb.abbreviation = 'WC'

INSERT INTO StateLobStatus SELECT (NEXT VALUE FOR SequenceStateLobStatus), slb.Id, (SELECT Id FROM LobStatus WHERE Status = 'Available'), GETDATE(), DATEADD(YYYY, 5, GETDATE()), 1, GETDATE(), 1, GETDATE(), 1
FROM [StateLineOfBusinesses] slb
INNER JOIN STATEMASTER sm
 ON slb.StateId = sm.Id
INNER JOIN LineOfBusiness lb
 ON slb.LineOfBusinessId = lb.Id
WHERE sm.StateCode = 'AL'
 AND lb.abbreviation = 'CA'
INSERT INTO StateLobStatus SELECT (NEXT VALUE FOR SequenceStateLobStatus), slb.Id, (SELECT Id FROM LobStatus WHERE Status = 'Not Available'), GETDATE(), DATEADD(YYYY, 5, GETDATE()), 1, GETDATE(), 1, GETDATE(), 1
FROM [StateLineOfBusinesses] slb
INNER JOIN STATEMASTER sm
 ON slb.StateId = sm.Id
INNER JOIN LineOfBusiness lb
 ON slb.LineOfBusinessId = lb.Id
WHERE sm.StateCode = 'AK'
 AND lb.abbreviation = 'CA'
INSERT INTO StateLobStatus SELECT (NEXT VALUE FOR SequenceStateLobStatus), slb.Id, (SELECT Id FROM LobStatus WHERE Status = 'Available'), GETDATE(), DATEADD(YYYY, 5, GETDATE()), 1, GETDATE(), 1, GETDATE(), 1
FROM [StateLineOfBusinesses] slb
INNER JOIN STATEMASTER sm
 ON slb.StateId = sm.Id
INNER JOIN LineOfBusiness lb
 ON slb.LineOfBusinessId = lb.Id
WHERE sm.StateCode = 'AZ'
 AND lb.abbreviation = 'CA'
INSERT INTO StateLobStatus SELECT (NEXT VALUE FOR SequenceStateLobStatus), slb.Id, (SELECT Id FROM LobStatus WHERE Status = 'Available'), GETDATE(), DATEADD(YYYY, 5, GETDATE()), 1, GETDATE(), 1, GETDATE(), 1
FROM [StateLineOfBusinesses] slb
INNER JOIN STATEMASTER sm
 ON slb.StateId = sm.Id
INNER JOIN LineOfBusiness lb
 ON slb.LineOfBusinessId = lb.Id
WHERE sm.StateCode = 'AR'
 AND lb.abbreviation = 'CA'
INSERT INTO StateLobStatus SELECT (NEXT VALUE FOR SequenceStateLobStatus), slb.Id, (SELECT Id FROM LobStatus WHERE Status = 'Available'), GETDATE(), DATEADD(YYYY, 5, GETDATE()), 1, GETDATE(), 1, GETDATE(), 1
FROM [StateLineOfBusinesses] slb
INNER JOIN STATEMASTER sm
 ON slb.StateId = sm.Id
INNER JOIN LineOfBusiness lb
 ON slb.LineOfBusinessId = lb.Id
WHERE sm.StateCode = 'CA'
 AND lb.abbreviation = 'CA'
INSERT INTO StateLobStatus SELECT (NEXT VALUE FOR SequenceStateLobStatus), slb.Id, (SELECT Id FROM LobStatus WHERE Status = 'Available'), GETDATE(), DATEADD(YYYY, 5, GETDATE()), 1, GETDATE(), 1, GETDATE(), 1
FROM [StateLineOfBusinesses] slb
INNER JOIN STATEMASTER sm
 ON slb.StateId = sm.Id
INNER JOIN LineOfBusiness lb
 ON slb.LineOfBusinessId = lb.Id
WHERE sm.StateCode = 'CO'
 AND lb.abbreviation = 'CA'
INSERT INTO StateLobStatus SELECT (NEXT VALUE FOR SequenceStateLobStatus), slb.Id, (SELECT Id FROM LobStatus WHERE Status = 'Available'), GETDATE(), DATEADD(YYYY, 5, GETDATE()), 1, GETDATE(), 1, GETDATE(), 1
FROM [StateLineOfBusinesses] slb
INNER JOIN STATEMASTER sm
 ON slb.StateId = sm.Id
INNER JOIN LineOfBusiness lb
 ON slb.LineOfBusinessId = lb.Id
WHERE sm.StateCode = 'CT'
 AND lb.abbreviation = 'CA'
INSERT INTO StateLobStatus SELECT (NEXT VALUE FOR SequenceStateLobStatus), slb.Id, (SELECT Id FROM LobStatus WHERE Status = 'Available'), GETDATE(), DATEADD(YYYY, 5, GETDATE()), 1, GETDATE(), 1, GETDATE(), 1
FROM [StateLineOfBusinesses] slb
INNER JOIN STATEMASTER sm
 ON slb.StateId = sm.Id
INNER JOIN LineOfBusiness lb
 ON slb.LineOfBusinessId = lb.Id
WHERE sm.StateCode = 'DE'
 AND lb.abbreviation = 'CA'
INSERT INTO StateLobStatus SELECT (NEXT VALUE FOR SequenceStateLobStatus), slb.Id, (SELECT Id FROM LobStatus WHERE Status = 'Available'), GETDATE(), DATEADD(YYYY, 5, GETDATE()), 1, GETDATE(), 1, GETDATE(), 1
FROM [StateLineOfBusinesses] slb
INNER JOIN STATEMASTER sm
 ON slb.StateId = sm.Id
INNER JOIN LineOfBusiness lb
 ON slb.LineOfBusinessId = lb.Id
WHERE sm.StateCode = 'DC'
 AND lb.abbreviation = 'CA'
INSERT INTO StateLobStatus SELECT (NEXT VALUE FOR SequenceStateLobStatus), slb.Id, (SELECT Id FROM LobStatus WHERE Status = 'Available'), GETDATE(), DATEADD(YYYY, 5, GETDATE()), 1, GETDATE(), 1, GETDATE(), 1
FROM [StateLineOfBusinesses] slb
INNER JOIN STATEMASTER sm
 ON slb.StateId = sm.Id
INNER JOIN LineOfBusiness lb
 ON slb.LineOfBusinessId = lb.Id
WHERE sm.StateCode = 'FL'
 AND lb.abbreviation = 'CA'
INSERT INTO StateLobStatus SELECT (NEXT VALUE FOR SequenceStateLobStatus), slb.Id, (SELECT Id FROM LobStatus WHERE Status = 'Available'), GETDATE(), DATEADD(YYYY, 5, GETDATE()), 1, GETDATE(), 1, GETDATE(), 1
FROM [StateLineOfBusinesses] slb
INNER JOIN STATEMASTER sm
 ON slb.StateId = sm.Id
INNER JOIN LineOfBusiness lb
 ON slb.LineOfBusinessId = lb.Id
WHERE sm.StateCode = 'GA'
 AND lb.abbreviation = 'CA'
INSERT INTO StateLobStatus SELECT (NEXT VALUE FOR SequenceStateLobStatus), slb.Id, (SELECT Id FROM LobStatus WHERE Status = 'Not Available'), GETDATE(), DATEADD(YYYY, 5, GETDATE()), 1, GETDATE(), 1, GETDATE(), 1
FROM [StateLineOfBusinesses] slb
INNER JOIN STATEMASTER sm
 ON slb.StateId = sm.Id
INNER JOIN LineOfBusiness lb
 ON slb.LineOfBusinessId = lb.Id
WHERE sm.StateCode = 'HI'
 AND lb.abbreviation = 'CA'
INSERT INTO StateLobStatus SELECT (NEXT VALUE FOR SequenceStateLobStatus), slb.Id, (SELECT Id FROM LobStatus WHERE Status = 'Available'), GETDATE(), DATEADD(YYYY, 5, GETDATE()), 1, GETDATE(), 1, GETDATE(), 1
FROM [StateLineOfBusinesses] slb
INNER JOIN STATEMASTER sm
 ON slb.StateId = sm.Id
INNER JOIN LineOfBusiness lb
 ON slb.LineOfBusinessId = lb.Id
WHERE sm.StateCode = 'ID'
 AND lb.abbreviation = 'CA'
INSERT INTO StateLobStatus SELECT (NEXT VALUE FOR SequenceStateLobStatus), slb.Id, (SELECT Id FROM LobStatus WHERE Status = 'Available'), GETDATE(), DATEADD(YYYY, 5, GETDATE()), 1, GETDATE(), 1, GETDATE(), 1
FROM [StateLineOfBusinesses] slb
INNER JOIN STATEMASTER sm
 ON slb.StateId = sm.Id
INNER JOIN LineOfBusiness lb
 ON slb.LineOfBusinessId = lb.Id
WHERE sm.StateCode = 'IL'
 AND lb.abbreviation = 'CA'
INSERT INTO StateLobStatus SELECT (NEXT VALUE FOR SequenceStateLobStatus), slb.Id, (SELECT Id FROM LobStatus WHERE Status = 'Available'), GETDATE(), DATEADD(YYYY, 5, GETDATE()), 1, GETDATE(), 1, GETDATE(), 1
FROM [StateLineOfBusinesses] slb
INNER JOIN STATEMASTER sm
 ON slb.StateId = sm.Id
INNER JOIN LineOfBusiness lb
 ON slb.LineOfBusinessId = lb.Id
WHERE sm.StateCode = 'IN'
 AND lb.abbreviation = 'CA'
INSERT INTO StateLobStatus SELECT (NEXT VALUE FOR SequenceStateLobStatus), slb.Id, (SELECT Id FROM LobStatus WHERE Status = 'Available'), GETDATE(), DATEADD(YYYY, 5, GETDATE()), 1, GETDATE(), 1, GETDATE(), 1
FROM [StateLineOfBusinesses] slb
INNER JOIN STATEMASTER sm
 ON slb.StateId = sm.Id
INNER JOIN LineOfBusiness lb
 ON slb.LineOfBusinessId = lb.Id
WHERE sm.StateCode = 'IA'
 AND lb.abbreviation = 'CA'
INSERT INTO StateLobStatus SELECT (NEXT VALUE FOR SequenceStateLobStatus), slb.Id, (SELECT Id FROM LobStatus WHERE Status = 'Available'), GETDATE(), DATEADD(YYYY, 5, GETDATE()), 1, GETDATE(), 1, GETDATE(), 1
FROM [StateLineOfBusinesses] slb
INNER JOIN STATEMASTER sm
 ON slb.StateId = sm.Id
INNER JOIN LineOfBusiness lb
 ON slb.LineOfBusinessId = lb.Id
WHERE sm.StateCode = 'KS'
 AND lb.abbreviation = 'CA'
INSERT INTO StateLobStatus SELECT (NEXT VALUE FOR SequenceStateLobStatus), slb.Id, (SELECT Id FROM LobStatus WHERE Status = 'Not Available'), GETDATE(), DATEADD(YYYY, 5, GETDATE()), 1, GETDATE(), 1, GETDATE(), 1
FROM [StateLineOfBusinesses] slb
INNER JOIN STATEMASTER sm
 ON slb.StateId = sm.Id
INNER JOIN LineOfBusiness lb
 ON slb.LineOfBusinessId = lb.Id
WHERE sm.StateCode = 'KY'
 AND lb.abbreviation = 'CA'
INSERT INTO StateLobStatus SELECT (NEXT VALUE FOR SequenceStateLobStatus), slb.Id, (SELECT Id FROM LobStatus WHERE Status = 'Available'), GETDATE(), DATEADD(YYYY, 5, GETDATE()), 1, GETDATE(), 1, GETDATE(), 1
FROM [StateLineOfBusinesses] slb
INNER JOIN STATEMASTER sm
 ON slb.StateId = sm.Id
INNER JOIN LineOfBusiness lb
 ON slb.LineOfBusinessId = lb.Id
WHERE sm.StateCode = 'LA'
 AND lb.abbreviation = 'CA'
INSERT INTO StateLobStatus SELECT (NEXT VALUE FOR SequenceStateLobStatus), slb.Id, (SELECT Id FROM LobStatus WHERE Status = 'Available'), GETDATE(), DATEADD(YYYY, 5, GETDATE()), 1, GETDATE(), 1, GETDATE(), 1
FROM [StateLineOfBusinesses] slb
INNER JOIN STATEMASTER sm
 ON slb.StateId = sm.Id
INNER JOIN LineOfBusiness lb
 ON slb.LineOfBusinessId = lb.Id
WHERE sm.StateCode = 'ME'
 AND lb.abbreviation = 'CA'
INSERT INTO StateLobStatus SELECT (NEXT VALUE FOR SequenceStateLobStatus), slb.Id, (SELECT Id FROM LobStatus WHERE Status = 'Available'), GETDATE(), DATEADD(YYYY, 5, GETDATE()), 1, GETDATE(), 1, GETDATE(), 1
FROM [StateLineOfBusinesses] slb
INNER JOIN STATEMASTER sm
 ON slb.StateId = sm.Id
INNER JOIN LineOfBusiness lb
 ON slb.LineOfBusinessId = lb.Id
WHERE sm.StateCode = 'MD'
 AND lb.abbreviation = 'CA'
INSERT INTO StateLobStatus SELECT (NEXT VALUE FOR SequenceStateLobStatus), slb.Id, (SELECT Id FROM LobStatus WHERE Status = 'Not Available'), GETDATE(), DATEADD(YYYY, 5, GETDATE()), 1, GETDATE(), 1, GETDATE(), 1
FROM [StateLineOfBusinesses] slb
INNER JOIN STATEMASTER sm
 ON slb.StateId = sm.Id
INNER JOIN LineOfBusiness lb
 ON slb.LineOfBusinessId = lb.Id
WHERE sm.StateCode = 'MA'
 AND lb.abbreviation = 'CA'
INSERT INTO StateLobStatus SELECT (NEXT VALUE FOR SequenceStateLobStatus), slb.Id, (SELECT Id FROM LobStatus WHERE Status = 'Not Available'), GETDATE(), DATEADD(YYYY, 5, GETDATE()), 1, GETDATE(), 1, GETDATE(), 1
FROM [StateLineOfBusinesses] slb
INNER JOIN STATEMASTER sm
 ON slb.StateId = sm.Id
INNER JOIN LineOfBusiness lb
 ON slb.LineOfBusinessId = lb.Id
WHERE sm.StateCode = 'MI'
 AND lb.abbreviation = 'CA'
INSERT INTO StateLobStatus SELECT (NEXT VALUE FOR SequenceStateLobStatus), slb.Id, (SELECT Id FROM LobStatus WHERE Status = 'Available'), GETDATE(), DATEADD(YYYY, 5, GETDATE()), 1, GETDATE(), 1, GETDATE(), 1
FROM [StateLineOfBusinesses] slb
INNER JOIN STATEMASTER sm
 ON slb.StateId = sm.Id
INNER JOIN LineOfBusiness lb
 ON slb.LineOfBusinessId = lb.Id
WHERE sm.StateCode = 'MN'
 AND lb.abbreviation = 'CA'
INSERT INTO StateLobStatus SELECT (NEXT VALUE FOR SequenceStateLobStatus), slb.Id, (SELECT Id FROM LobStatus WHERE Status = 'Coming Soon'), GETDATE(), DATEADD(YYYY, 5, GETDATE()), 1, GETDATE(), 1, GETDATE(), 1
FROM [StateLineOfBusinesses] slb
INNER JOIN STATEMASTER sm
 ON slb.StateId = sm.Id
INNER JOIN LineOfBusiness lb
 ON slb.LineOfBusinessId = lb.Id
WHERE sm.StateCode = 'MS'
 AND lb.abbreviation = 'CA'
INSERT INTO StateLobStatus SELECT (NEXT VALUE FOR SequenceStateLobStatus), slb.Id, (SELECT Id FROM LobStatus WHERE Status = 'Available'), GETDATE(), DATEADD(YYYY, 5, GETDATE()), 1, GETDATE(), 1, GETDATE(), 1
FROM [StateLineOfBusinesses] slb
INNER JOIN STATEMASTER sm
 ON slb.StateId = sm.Id
INNER JOIN LineOfBusiness lb
 ON slb.LineOfBusinessId = lb.Id
WHERE sm.StateCode = 'MO'
 AND lb.abbreviation = 'CA'
INSERT INTO StateLobStatus SELECT (NEXT VALUE FOR SequenceStateLobStatus), slb.Id, (SELECT Id FROM LobStatus WHERE Status = 'Available'), GETDATE(), DATEADD(YYYY, 5, GETDATE()), 1, GETDATE(), 1, GETDATE(), 1
FROM [StateLineOfBusinesses] slb
INNER JOIN STATEMASTER sm
 ON slb.StateId = sm.Id
INNER JOIN LineOfBusiness lb
 ON slb.LineOfBusinessId = lb.Id
WHERE sm.StateCode = 'MT'
 AND lb.abbreviation = 'CA'
INSERT INTO StateLobStatus SELECT (NEXT VALUE FOR SequenceStateLobStatus), slb.Id, (SELECT Id FROM LobStatus WHERE Status = 'Available'), GETDATE(), DATEADD(YYYY, 5, GETDATE()), 1, GETDATE(), 1, GETDATE(), 1
FROM [StateLineOfBusinesses] slb
INNER JOIN STATEMASTER sm
 ON slb.StateId = sm.Id
INNER JOIN LineOfBusiness lb
 ON slb.LineOfBusinessId = lb.Id
WHERE sm.StateCode = 'NE'
 AND lb.abbreviation = 'CA'
INSERT INTO StateLobStatus SELECT (NEXT VALUE FOR SequenceStateLobStatus), slb.Id, (SELECT Id FROM LobStatus WHERE Status = 'Available'), GETDATE(), DATEADD(YYYY, 5, GETDATE()), 1, GETDATE(), 1, GETDATE(), 1
FROM [StateLineOfBusinesses] slb
INNER JOIN STATEMASTER sm
 ON slb.StateId = sm.Id
INNER JOIN LineOfBusiness lb
 ON slb.LineOfBusinessId = lb.Id
WHERE sm.StateCode = 'NV'
 AND lb.abbreviation = 'CA'
INSERT INTO StateLobStatus SELECT (NEXT VALUE FOR SequenceStateLobStatus), slb.Id, (SELECT Id FROM LobStatus WHERE Status = 'Not Available'), GETDATE(), DATEADD(YYYY, 5, GETDATE()), 1, GETDATE(), 1, GETDATE(), 1
FROM [StateLineOfBusinesses] slb
INNER JOIN STATEMASTER sm
 ON slb.StateId = sm.Id
INNER JOIN LineOfBusiness lb
 ON slb.LineOfBusinessId = lb.Id
WHERE sm.StateCode = 'NH'
 AND lb.abbreviation = 'CA'
INSERT INTO StateLobStatus SELECT (NEXT VALUE FOR SequenceStateLobStatus), slb.Id, (SELECT Id FROM LobStatus WHERE Status = 'Available'), GETDATE(), DATEADD(YYYY, 5, GETDATE()), 1, GETDATE(), 1, GETDATE(), 1
FROM [StateLineOfBusinesses] slb
INNER JOIN STATEMASTER sm
 ON slb.StateId = sm.Id
INNER JOIN LineOfBusiness lb
 ON slb.LineOfBusinessId = lb.Id
WHERE sm.StateCode = 'NJ'
 AND lb.abbreviation = 'CA'
INSERT INTO StateLobStatus SELECT (NEXT VALUE FOR SequenceStateLobStatus), slb.Id, (SELECT Id FROM LobStatus WHERE Status = 'Available'), GETDATE(), DATEADD(YYYY, 5, GETDATE()), 1, GETDATE(), 1, GETDATE(), 1
FROM [StateLineOfBusinesses] slb
INNER JOIN STATEMASTER sm
 ON slb.StateId = sm.Id
INNER JOIN LineOfBusiness lb
 ON slb.LineOfBusinessId = lb.Id
WHERE sm.StateCode = 'NM'
 AND lb.abbreviation = 'CA'
INSERT INTO StateLobStatus SELECT (NEXT VALUE FOR SequenceStateLobStatus), slb.Id, (SELECT Id FROM LobStatus WHERE Status = 'Available'), GETDATE(), DATEADD(YYYY, 5, GETDATE()), 1, GETDATE(), 1, GETDATE(), 1
FROM [StateLineOfBusinesses] slb
INNER JOIN STATEMASTER sm
 ON slb.StateId = sm.Id
INNER JOIN LineOfBusiness lb
 ON slb.LineOfBusinessId = lb.Id
WHERE sm.StateCode = 'NY'
 AND lb.abbreviation = 'CA'
INSERT INTO StateLobStatus SELECT (NEXT VALUE FOR SequenceStateLobStatus), slb.Id, (SELECT Id FROM LobStatus WHERE Status = 'Not Available'), GETDATE(), DATEADD(YYYY, 5, GETDATE()), 1, GETDATE(), 1, GETDATE(), 1
FROM [StateLineOfBusinesses] slb
INNER JOIN STATEMASTER sm
 ON slb.StateId = sm.Id
INNER JOIN LineOfBusiness lb
 ON slb.LineOfBusinessId = lb.Id
WHERE sm.StateCode = 'NC'
 AND lb.abbreviation = 'CA'
INSERT INTO StateLobStatus SELECT (NEXT VALUE FOR SequenceStateLobStatus), slb.Id, (SELECT Id FROM LobStatus WHERE Status = 'Available'), GETDATE(), DATEADD(YYYY, 5, GETDATE()), 1, GETDATE(), 1, GETDATE(), 1
FROM [StateLineOfBusinesses] slb
INNER JOIN STATEMASTER sm
 ON slb.StateId = sm.Id
INNER JOIN LineOfBusiness lb
 ON slb.LineOfBusinessId = lb.Id
WHERE sm.StateCode = 'ND'
 AND lb.abbreviation = 'CA'
INSERT INTO StateLobStatus SELECT (NEXT VALUE FOR SequenceStateLobStatus), slb.Id, (SELECT Id FROM LobStatus WHERE Status = 'Available'), GETDATE(), DATEADD(YYYY, 5, GETDATE()), 1, GETDATE(), 1, GETDATE(), 1
FROM [StateLineOfBusinesses] slb
INNER JOIN STATEMASTER sm
 ON slb.StateId = sm.Id
INNER JOIN LineOfBusiness lb
 ON slb.LineOfBusinessId = lb.Id
WHERE sm.StateCode = 'OH'
 AND lb.abbreviation = 'CA'
INSERT INTO StateLobStatus SELECT (NEXT VALUE FOR SequenceStateLobStatus), slb.Id, (SELECT Id FROM LobStatus WHERE Status = 'Available'), GETDATE(), DATEADD(YYYY, 5, GETDATE()), 1, GETDATE(), 1, GETDATE(), 1
FROM [StateLineOfBusinesses] slb
INNER JOIN STATEMASTER sm
 ON slb.StateId = sm.Id
INNER JOIN LineOfBusiness lb
 ON slb.LineOfBusinessId = lb.Id
WHERE sm.StateCode = 'OK'
 AND lb.abbreviation = 'CA'
INSERT INTO StateLobStatus SELECT (NEXT VALUE FOR SequenceStateLobStatus), slb.Id, (SELECT Id FROM LobStatus WHERE Status = 'Available'), GETDATE(), DATEADD(YYYY, 5, GETDATE()), 1, GETDATE(), 1, GETDATE(), 1
FROM [StateLineOfBusinesses] slb
INNER JOIN STATEMASTER sm
 ON slb.StateId = sm.Id
INNER JOIN LineOfBusiness lb
 ON slb.LineOfBusinessId = lb.Id
WHERE sm.StateCode = 'OR'
 AND lb.abbreviation = 'CA'
INSERT INTO StateLobStatus SELECT (NEXT VALUE FOR SequenceStateLobStatus), slb.Id, (SELECT Id FROM LobStatus WHERE Status = 'Available'), GETDATE(), DATEADD(YYYY, 5, GETDATE()), 1, GETDATE(), 1, GETDATE(), 1
FROM [StateLineOfBusinesses] slb
INNER JOIN STATEMASTER sm
 ON slb.StateId = sm.Id
INNER JOIN LineOfBusiness lb
 ON slb.LineOfBusinessId = lb.Id
WHERE sm.StateCode = 'PA'
 AND lb.abbreviation = 'CA'
INSERT INTO StateLobStatus SELECT (NEXT VALUE FOR SequenceStateLobStatus), slb.Id, (SELECT Id FROM LobStatus WHERE Status = 'Available'), GETDATE(), DATEADD(YYYY, 5, GETDATE()), 1, GETDATE(), 1, GETDATE(), 1
FROM [StateLineOfBusinesses] slb
INNER JOIN STATEMASTER sm
 ON slb.StateId = sm.Id
INNER JOIN LineOfBusiness lb
 ON slb.LineOfBusinessId = lb.Id
WHERE sm.StateCode = 'RI'
 AND lb.abbreviation = 'CA'
INSERT INTO StateLobStatus SELECT (NEXT VALUE FOR SequenceStateLobStatus), slb.Id, (SELECT Id FROM LobStatus WHERE Status = 'Available'), GETDATE(), DATEADD(YYYY, 5, GETDATE()), 1, GETDATE(), 1, GETDATE(), 1
FROM [StateLineOfBusinesses] slb
INNER JOIN STATEMASTER sm
 ON slb.StateId = sm.Id
INNER JOIN LineOfBusiness lb
 ON slb.LineOfBusinessId = lb.Id
WHERE sm.StateCode = 'SC'
 AND lb.abbreviation = 'CA'
INSERT INTO StateLobStatus SELECT (NEXT VALUE FOR SequenceStateLobStatus), slb.Id, (SELECT Id FROM LobStatus WHERE Status = 'Available'), GETDATE(), DATEADD(YYYY, 5, GETDATE()), 1, GETDATE(), 1, GETDATE(), 1
FROM [StateLineOfBusinesses] slb
INNER JOIN STATEMASTER sm
 ON slb.StateId = sm.Id
INNER JOIN LineOfBusiness lb
 ON slb.LineOfBusinessId = lb.Id
WHERE sm.StateCode = 'SD'
 AND lb.abbreviation = 'CA'
INSERT INTO StateLobStatus SELECT (NEXT VALUE FOR SequenceStateLobStatus), slb.Id, (SELECT Id FROM LobStatus WHERE Status = 'Available'), GETDATE(), DATEADD(YYYY, 5, GETDATE()), 1, GETDATE(), 1, GETDATE(), 1
FROM [StateLineOfBusinesses] slb
INNER JOIN STATEMASTER sm
 ON slb.StateId = sm.Id
INNER JOIN LineOfBusiness lb
 ON slb.LineOfBusinessId = lb.Id
WHERE sm.StateCode = 'TN'
 AND lb.abbreviation = 'CA'
INSERT INTO StateLobStatus SELECT (NEXT VALUE FOR SequenceStateLobStatus), slb.Id, (SELECT Id FROM LobStatus WHERE Status = 'Available'), GETDATE(), DATEADD(YYYY, 5, GETDATE()), 1, GETDATE(), 1, GETDATE(), 1
FROM [StateLineOfBusinesses] slb
INNER JOIN STATEMASTER sm
 ON slb.StateId = sm.Id
INNER JOIN LineOfBusiness lb
 ON slb.LineOfBusinessId = lb.Id
WHERE sm.StateCode = 'TX'
 AND lb.abbreviation = 'CA'
INSERT INTO StateLobStatus SELECT (NEXT VALUE FOR SequenceStateLobStatus), slb.Id, (SELECT Id FROM LobStatus WHERE Status = 'Not Available'), GETDATE(), DATEADD(YYYY, 5, GETDATE()), 1, GETDATE(), 1, GETDATE(), 1
FROM [StateLineOfBusinesses] slb
INNER JOIN STATEMASTER sm
 ON slb.StateId = sm.Id
INNER JOIN LineOfBusiness lb
 ON slb.LineOfBusinessId = lb.Id
WHERE sm.StateCode = 'UT'
 AND lb.abbreviation = 'CA'
INSERT INTO StateLobStatus SELECT (NEXT VALUE FOR SequenceStateLobStatus), slb.Id, (SELECT Id FROM LobStatus WHERE Status = 'Available'), GETDATE(), DATEADD(YYYY, 5, GETDATE()), 1, GETDATE(), 1, GETDATE(), 1
FROM [StateLineOfBusinesses] slb
INNER JOIN STATEMASTER sm
 ON slb.StateId = sm.Id
INNER JOIN LineOfBusiness lb
 ON slb.LineOfBusinessId = lb.Id
WHERE sm.StateCode = 'VT'
 AND lb.abbreviation = 'CA'
INSERT INTO StateLobStatus SELECT (NEXT VALUE FOR SequenceStateLobStatus), slb.Id, (SELECT Id FROM LobStatus WHERE Status = 'Available'), GETDATE(), DATEADD(YYYY, 5, GETDATE()), 1, GETDATE(), 1, GETDATE(), 1
FROM [StateLineOfBusinesses] slb
INNER JOIN STATEMASTER sm
 ON slb.StateId = sm.Id
INNER JOIN LineOfBusiness lb
 ON slb.LineOfBusinessId = lb.Id
WHERE sm.StateCode = 'VA'
 AND lb.abbreviation = 'CA'
INSERT INTO StateLobStatus SELECT (NEXT VALUE FOR SequenceStateLobStatus), slb.Id, (SELECT Id FROM LobStatus WHERE Status = 'Available'), GETDATE(), DATEADD(YYYY, 5, GETDATE()), 1, GETDATE(), 1, GETDATE(), 1
FROM [StateLineOfBusinesses] slb
INNER JOIN STATEMASTER sm
 ON slb.StateId = sm.Id
INNER JOIN LineOfBusiness lb
 ON slb.LineOfBusinessId = lb.Id
WHERE sm.StateCode = 'WA'
 AND lb.abbreviation = 'CA'
INSERT INTO StateLobStatus SELECT (NEXT VALUE FOR SequenceStateLobStatus), slb.Id, (SELECT Id FROM LobStatus WHERE Status = 'Available'), GETDATE(), DATEADD(YYYY, 5, GETDATE()), 1, GETDATE(), 1, GETDATE(), 1
FROM [StateLineOfBusinesses] slb
INNER JOIN STATEMASTER sm
 ON slb.StateId = sm.Id
INNER JOIN LineOfBusiness lb
 ON slb.LineOfBusinessId = lb.Id
WHERE sm.StateCode = 'WV'
 AND lb.abbreviation = 'CA'
INSERT INTO StateLobStatus SELECT (NEXT VALUE FOR SequenceStateLobStatus), slb.Id, (SELECT Id FROM LobStatus WHERE Status = 'Available'), GETDATE(), DATEADD(YYYY, 5, GETDATE()), 1, GETDATE(), 1, GETDATE(), 1
FROM [StateLineOfBusinesses] slb
INNER JOIN STATEMASTER sm
 ON slb.StateId = sm.Id
INNER JOIN LineOfBusiness lb
 ON slb.LineOfBusinessId = lb.Id
WHERE sm.StateCode = 'WI'
 AND lb.abbreviation = 'CA'
INSERT INTO StateLobStatus SELECT (NEXT VALUE FOR SequenceStateLobStatus), slb.Id, (SELECT Id FROM LobStatus WHERE Status = 'Available'), GETDATE(), DATEADD(YYYY, 5, GETDATE()), 1, GETDATE(), 1, GETDATE(), 1
FROM [StateLineOfBusinesses] slb
INNER JOIN STATEMASTER sm
 ON slb.StateId = sm.Id
INNER JOIN LineOfBusiness lb
 ON slb.LineOfBusinessId = lb.Id
WHERE sm.StateCode = 'WY'
 AND lb.abbreviation = 'CA'
