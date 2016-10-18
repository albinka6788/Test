
-- ================================================      
/*      
@Author      : Anuj Kumar Singh       
@Description : This procedure will fetch PolicyPaymentDetail id based on transaction code   
@CreatedOn   : 2016-Jan-19     
      
@Updation History :       
   @Update-1 : 2016-Jan-19, Anuj Kumar Singh, created
      
@ExampleCall : EXEC GetPolicyPaymentIdByTransactionCode 'N9WC637456'      
      
*/      
-- ================================================  

ALTER PROCEDURE [dbo].[GetPolicyPaymentIdByTransactionCode]
(        
  @transactionCode VARCHAR(200)    
)        
AS         
BEGIN        
        
 -- SET NOCOUNT ON added to prevent extra result sets from        
 SET NOCOUNT ON;   
 SELECT policyPayment.Id
	FROM PolicyPaymentDetail policyPayment
	WHERE policyPayment.TransactionCode= @transactionCode and policyPayment.IsActive=1
END 
