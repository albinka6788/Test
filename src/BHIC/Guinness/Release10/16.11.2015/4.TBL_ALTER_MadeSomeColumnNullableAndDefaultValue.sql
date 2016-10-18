

ALTER TABLE OraganisationAddress ALTER COLUMN ContactNumber1 BIGINT 
ALTER TABLE OraganisationAddress ALTER COLUMN ContactNumber2 BIGINT NULL
ALTER TABLE OraganisationAddress ALTER COLUMN Fax BIGINT NULL

ALTER TABLE OraganisationAddress ALTER COLUMN Address3 VARCHAR(200) NULL
ALTER TABLE OraganisationAddress ALTER COLUMN County VARCHAR(200) NULL
ALTER TABLE OraganisationAddress ADD DEFAULT 1 FOR CountryID




