-- ================================================      
/*      
@Author      : Sreeram       
@Description : This procedure will update IsEmailverified in OrganisationUserDetail table
			   based on Email
@CreatedOn   : 2015-Nov-30       
            
@ExampleCall : 
EXEC UpdateVerificationEmail 'Abc@a.com', 1   
      
*/      
-- ================================================      
  
Create PROCEDURE [dbo].[UpdateVerificationEmail]
(      
  @Email VARCHAR(150),  
  @IsEmailverified bit
)      
AS       
BEGIN      
      
 -- SET NOCOUNT ON added to prevent extra result sets from      
SET NOCOUNT OFF;      
    
     UPDATE OrganisationUserDetail
	 
	 SET  IsEmailVerified = @IsEmailverified

	 WHERE EmailID=@Email; 
       
END 