

select * from [StateLineOfBusinesses]
delete from [dbo].[StateLobStatus]

DELETE FROM [dbo].[StateLineOfBusinesses]

alter sequence sequenceStateLineOfBusinesses restart with 1

alter sequence sequenceStateLobStatus restart with 1

insert into StateLineOfBusinesses 
select (next value for sequenceStateLineOfBusinesses), s.id, l.id, 1, getdate(), 1, getdate(), 1
from statemaster s,
LineOfBusiness l
select * from [StateLineOfBusinesses]

GO


