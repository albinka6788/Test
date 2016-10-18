-- ================================================      
/*      
@Author      : Sreeram       
@Description : This procedure will give the count of PolicyNumber based on PolicyNumber     
@CreatedOn   : 2015-Dec-4       
      
@Updation History :       
  
      
@ExampleCall : EXEC IsPolicyExists 'N9WC637709'      
      
*/      
-- ================================================  

Create PROCEDURE [dbo].[IsPolicyExists] 
(        
  @PolicyNumber VARCHAR(150)    
)        
AS         
BEGIN        
        
 -- SET NOCOUNT ON added to prevent extra result sets from        
 SET NOCOUNT OFF;   

 SELECT COUNT (*) as PolicyCount FROM Policy WHERE  PolicyNumber = @PolicyNumber; 
      
END 





