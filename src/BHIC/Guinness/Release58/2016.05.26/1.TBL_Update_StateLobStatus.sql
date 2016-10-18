﻿UPDATE StateLobStatus
SET StatusId = 1
WHERE StateLobStatus.StateLobId IN 
(
SELECT sls.StateLobId FROM StateLobStatus sls
INNER JOIN StateLineOfBusinesses sl
	on sls.StateLobId = sl.Id
INNER JOIN StateMaster sm 
	ON sl.StateId = sm.Id
INNER JOIN LineOfBusiness lb
	ON sl.LineOfBusinessId = lb.Id
WHERE StateCode IN ('MS') and lb.Id = 3)

UPDATE StateLobStatus
SET StatusId = 1
WHERE StateLobStatus.StateLobId IN 
(
SELECT sls.StateLobId FROM StateLobStatus sls
INNER JOIN StateLineOfBusinesses sl
	on sls.StateLobId = sl.Id
INNER JOIN StateMaster sm 
	ON sl.StateId = sm.Id
INNER JOIN LineOfBusiness lb
	ON sl.LineOfBusinessId = lb.Id
WHERE StateCode IN ('MA', 'MD') and lb.Id = 2)
