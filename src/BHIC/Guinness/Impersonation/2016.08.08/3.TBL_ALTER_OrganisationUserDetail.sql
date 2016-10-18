---Execute the below scripts in sequence
Alter table organisationuserdetail
add UserRoleId tinyint default(1)

----Execute after above script is successful
Update organisationuserdetail set UserRoleId=1