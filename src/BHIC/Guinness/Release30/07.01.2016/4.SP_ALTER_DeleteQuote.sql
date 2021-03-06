USE [Guinness_DB]
GO
/****** Object:  StoredProcedure [dbo].[DeleteQuote]    Script Date: 1/7/2016 11:58:39 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
/*      
@Author      : Sreeram       
@Description : This procedure will update IsDeleted coulmn in Quote table
			   based on QuoteNumber
@CreatedOn   : 2015-Dec-14   
 
@Updation History :       
   @Update-1 : 2016-Jan-7, Gurpreet, also check for UserID before delete any quote
            
@ExampleCall : EXEC DeleteQuote 1
      
*/      
-- ================================================      
  
ALTER PROCEDURE [dbo].[DeleteQuote]
(      
  @QuoteNumber varchar(150),  
  @IsQuoteDeleted bit,
  @UserID int
)      
AS       
BEGIN      
      
 -- SET NOCOUNT ON added to prevent extra result sets from      
SET NOCOUNT OFF;      
    
     UPDATE Quote
	 
	 SET  IsDeleted = @IsQuoteDeleted

	 WHERE OrganizationUserDetailID=@UserID and QuoteNumber=@QuoteNumber; 
       
END