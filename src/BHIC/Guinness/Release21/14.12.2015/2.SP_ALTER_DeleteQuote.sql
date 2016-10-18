-- ================================================      
/*      
@Author      : Sreeram       
@Description : This procedure will update IsDeleted coulmn in Quote table
			   based on QuoteNumber
@CreatedOn   : 2015-Dec-14       
            
@ExampleCall : EXEC DeleteQuote 1
      
*/      
-- ================================================      
  
CREATE PROCEDURE [dbo].[DeleteQuote]
(      
  @QuoteNumber varchar(150),  
  @IsQuoteDeleted bit
)      
AS       
BEGIN      
      
 -- SET NOCOUNT ON added to prevent extra result sets from      
SET NOCOUNT OFF;      
    
     UPDATE Quote
	 
	 SET  IsDeleted = @IsQuoteDeleted

	 WHERE QuoteNumber=@QuoteNumber; 
       
END 