delete from Policy
delete from Quote
delete from  OraganisationAddress
delete from OrganisationUserDetail
TRUNCATE TABLE PurchaePathCustomSession
TRUNCATE TABLE PolicyPaymentDetail
TRUNCATE TABLE FeinPremiumFactor

--Select * From OraganisationAddress
--Select * From OrganisationUserDetail
--Select * From PurchaePathCustomSession
--Select * From PolicyPaymentDetail
--Select * From Policy
--Select * From Quote
--Select * From FeinPremiumFactor

alter sequence SEQUENCEOraganisationAddress	    Restart with 1
alter sequence SEQUENCEOrganisationUserDetail	Restart with 1
alter sequence SEQUENCEPurchaePathCustomSession	Restart with 1
alter sequence SEQUENCEPolicyPaymentDetail		Restart with 1
alter sequence SEQUENCEPolicy					Restart with 1
alter sequence SEQUENCEQuote					Restart with 1
alter sequence SEQUENCEFeinPremiumFactor		Restart with 1
