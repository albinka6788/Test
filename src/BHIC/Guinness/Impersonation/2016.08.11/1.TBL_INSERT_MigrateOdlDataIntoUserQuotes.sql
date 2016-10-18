
INSERT INTO UserQuotes(Id,QuoteId,OrganizationUserDetailID,StartDate,CreatedDate,CreatedBy,ModifiedDate,ModifiedBy) 
Select (NEXT VALUE FOR SEQUENCEUserQuote) SeqId,
Id QuoteId,ISNULL(OrganizationUserDetailID,1) UserId,GETDATE() StartDate,GETDATE() CreatedDate,1 CreatedBy,GETDATE() ModifiedDate,1 ModifiedBy
From Quote Where ISNULL(IsDeleted,0) != 1 AND Id NOT IN 
(
	Select DISTINCT T2.QuoteId From Quote T1 INNER JOIN UserQuotes T2 ON T1.Id=T2.QuoteId
	Where ISNULL(T1.OrganizationUserDetailID,1)=T2.OrganizationUserDetailID AND ISNULL(T1.IsDeleted,0) != 1 --Order By Id  Desc
)