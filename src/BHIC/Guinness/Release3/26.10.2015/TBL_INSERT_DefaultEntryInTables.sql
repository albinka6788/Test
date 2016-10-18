INSERT INTO ExternalSystem
SELECT NEXT VALUE FOR [SEQUENCEExternalSystem], 'GUARD'

INSERT INTO QuotePolicyType
SELECT NEXT VALUE FOR [SEQUENCEQuotePolicyType], 'WC', 'Worker Compensation Quote'
