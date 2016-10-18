-- ================================================      
/*      
@Author      : Sreeram       
@Description : This procedure will update the FirstName,LastName,Phone in OrganisationUserDetail table
			   based on Email
@CreatedOn   : 2015-Nov-20       
      
@Updation History :       
   @Update-1 : 2015-Nov-21, Sreeram, Updated  
   @Update-2 : 2015-Dec-2, Sreeram, Updated
@ExampleCall : 
EXEC UpdateOraganisationUserDetail 'Abc@a.com', 'FirstName', 'LastName',123413   

*/      
-- ================================================      
  
ALTER PROCEDURE [dbo].[UpdateOraganisationUserDetail]
(      
  @Email VARCHAR(150),  
  @Firstname VARCHAR(150), 
  @Lastname VARCHAR(150), 
  @Phone  BIGINT   
)      
AS       
BEGIN      
      
 -- SET NOCOUNT ON added to prevent extra result sets from      
SET NOCOUNT OFF;      
    
     UPDATE OrganisationUserDetail
	 
	 SET  FirstName =@Firstname,LastName = @Lastname,PhoneNumber = @Phone

	 WHERE EmailID=@Email; 
       
END 



