-- ================================================
/*
@Author      : Krishna
-- [DBO].[POLICYCANCELOPTIONS] seting isactive to false for the User story GUIN-150(d)
-- one time execution
*/
-- ================================================

UPDATE [DBO].[POLICYCANCELOPTIONS] SET ISACTIVE=0  WHERE ID=7

