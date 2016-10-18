

ALTER TABLE Quote ALTER COLUMN OrganizationUserDetailID INTEGER NULL
ALTER TABLE Quote ALTER COLUMN OrganizationAddressID INTEGER NULL 

EXECUTE SP_RENAME 'Quote.QuotePolicyTypeID', 'LineOfBusinessId', 'COLUMN';