UPDATE StateLobStatus
SET StatusId = 3
WHERE StateLobStatus.StateLobId IN 
(
SELECT sls.StateLobId FROM StateLobStatus sls
INNER JOIN StateLineOfBusinesses sl
	on sls.StateLobId = sl.Id
INNER JOIN StateMaster sm 
	ON sl.StateId = sm.Id
INNER JOIN LineOfBusiness lb
	ON sl.LineOfBusinessId = lb.Id
WHERE StateCode IN ('WA', 'NY') and lb.Id = 2)
