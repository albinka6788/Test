/****** Object:  StoredProcedure [dbo].[GetOrganisationAddressId]   Script Date: 20-01-2016 12:29:52 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
 
-- ================================================      
/*      
@Author      : Venkatesh       
@Description : This procedure will return organization address id for the orgnazation Id     
@CreatedOn   : 2016-Jan-16       
      
@Updation History :       
      
@ExampleCall : EXEC GetOrganisationAddressId 123      
      
*/      
-- ================================================      
  
CREATE PROCEDURE [dbo].[GetOrganisationAddressId]      
(      
  @OrganizationId int   
)      
AS       
BEGIN      
      
 -- SET NOCOUNT ON added to prevent extra result sets from      
 SET NOCOUNT ON;      
    
     
 SELECT Id from [dbo].[OraganisationAddress] where OrganizationID=@OrganizationId

 
       
END 



