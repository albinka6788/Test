-- ================================================      
/*      
@Author      : Sreeram       
@Description : This procedure will policy id based on email     
@CreatedOn   : 2015-Oct-29       
      
@Updation History :       
   @Update-1 : 2015-Oct-29, Sreeram, created
   @update-2 : 2015-Oct-30, Gurpreet, retrive policy number from user table if not found then from policy table
   @update-2 : 2015-Nov-13, albin, retrive policy number from policy table only
@ExampleCall : EXEC GetPolicyID 'aa@xceedance.com'      
      
*/      
-- ================================================  

ALTER PROCEDURE [dbo].[GetPolicyID] 
(        
  @Email VARCHAR(150)    
)        
AS         
BEGIN        
        
 -- SET NOCOUNT ON added to prevent extra result sets from        
 SET NOCOUNT ON;   
 SELECT distinct policy.PolicyNumber PolicyID  
	FROM OrganisationUserDetail Org
	inner join Quote quote on Org.id=quote.OrganizationUserDetailID    
	inner join Policy policy on quote.id=policy.QuoteID
	WHERE Org.EmailID = @Email         
END 


GO
/****** Object:  StoredProcedure [dbo].[ChangePassword]    Script Date: 11/13/2015 4:25:24 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO


/*  ================================================      
     
@Author      : Sreeram   
@Description : This procedure will return no  
@CreatedOn   : 2015-Nov-6       
@ExampleCall : EXEC ChangePassword 'aa@xceedance.com','123456'
   
 ================================================   */    
CREATE PROCEDURE [dbo].[ChangePassword]
(      
  @email VARCHAR(150), 
  @newpassword VARCHAR(500) 
)      
AS       
BEGIN      
 UPDATE OrganisationUserDetail
 SET Password =@newpassword
 WHERE EmailID=@email
END 


GO
/****** Object:  StoredProcedure [dbo].[GetQuoteDetails]    Script Date: 11/13/2015 1:17:25 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
 /*=============================================
 Author:		<Gurpreet Singh>
 Create date: <13-Nov-2015>
 Description:	<Get Saved Quotes of user>
exec GetUserQuotes '2'
 =============================================*/
Create PROCEDURE [dbo].[GetUserQuotes]
(
@UserID int
)
AS
BEGIN

select Qt.[Id]
      ,Qt.[OrganizationUserDetailID]
      ,Qt.[OrganizationAddressID]
      ,Qt.[QuoteNumber]
      ,Qt.[LineOfBusinessId]	  
      ,Qt.[ExternalSystemID]
      ,Qt.[RequestDate]
      ,Qt.[ExpiryDate]
      ,Qt.[PremiumAmount]
      ,Qt.[IsActive]
      ,Qt.[CreatedDate]
      ,Qt.[CreatedBy]
      ,Qt.[ModifiedDate]
      ,Qt.[ModifiedBy]
      ,Qt.[PaymentoptionId]
      ,Qt.[AgencyCode] 
	  ,Lob.LineOfBusinessName
	  ,Cs.SessionData
	  from [dbo].[Quote] Qt 
	  inner join [dbo].[LineOfBusiness] Lob on Qt.LineOfBusinessId=Lob.Id
	  left join [dbo].[PurchaePathCustomSession] Cs on qt.QuoteNumber= Cs.QuoteId
	  where Qt.OrganizationUserDetailID =  @UserID

END

GO
/****** Object:  StoredProcedure [dbo].[GetCredentials]    Script Date: 11/13/2015 4:28:43 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO


  
  
-- ================================================      
/*      
@Author      : Sreeram       
@Description : This procedure will return Email/Passowrd for supplied Email & password      
@CreatedOn   : 2015-Oct-15       
      
@Updation History :       
   @Update-1 : 2015-Oct-16, Sreeram, created
   @update-2 : 2015-Oct-29, Gurpreet, retrive user detail  
   @update-3 : 2015-nov-02, Gurpreet, retrive password to encypt and match    
      
@ExampleCall : EXEC GetCredentials 'Test@234.com','123'      
      
*/      
-- ================================================      
  
ALTER PROCEDURE [dbo].[GetCredentials]      
(      
  @Email VARCHAR(150)   
)      
AS       
BEGIN      
      
 -- SET NOCOUNT ON added to prevent extra result sets from      
 SET NOCOUNT ON;      
    
     
 SELECT Id,EmailID,FirstName,LastName,password from OrganisationUserDetail where EmailID=@Email
 
       
END 


Go

	  





