
 /*=============================================
 Author:		<Amit Kumar>
 Create date: <11-July-2016>
 Description:	<selecting Loss Control Filename from GUID>
exec GetLossControlFileName 'fsdf-sdgsd-fsd-fgsd-fsd-fsfsgsd'
 =============================================*/

CREATE PROCEDURE [dbo].[GetLossControlFileName]
(
	@Guid VARCHAR(50)
)
AS
BEGIN
	SELECT [FileName] FROM [LossControlFiles] WHERE [Guid] =  @Guid
END


GO


