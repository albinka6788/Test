-- ================================================      
/*      
@Author      : Sreeram       
@Description : This procedure will update IsEmailverified in OrganisationUserDetail table
			   based on Email
@CreatedOn   : 2015-Nov-30       
@Updation History :       
   @Update-1 : 2016-Jan-6, Sreeram, created
   @update-2 : 2016-Jan-27, Albin, Added ModifiedDate, ModifiedBy in the proc                   
@ExampleCall : 
EXEC UpdateVerificationEmail 'Abc@a.com', 1   
      
*/      
-- ================================================      
  
ALTER PROCEDURE [dbo].[UpdateVerificationEmail]
(      
  @Email VARCHAR(150),  
  @IsEmailverified bit
)      
AS       
BEGIN      
      
 -- SET NOCOUNT ON added to prevent extra result sets from      
SET NOCOUNT OFF;      
    
     UPDATE OrganisationUserDetail
	 
	 SET  IsEmailVerified = @IsEmailverified ,ModifiedDate = GetDate(), ModifiedBy = Id

	 WHERE EmailID=@Email; 
       
END 