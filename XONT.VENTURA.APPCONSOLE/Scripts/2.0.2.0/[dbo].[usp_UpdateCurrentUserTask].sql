IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[usp_UpdateCurrentUserTask]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[usp_UpdateCurrentUserTask]
GO


SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE dbo.usp_UpdateCurrentUserTask	
	@UserName [char](30) ='',
	@BusinessUnit [char](4) ='',
	@SessionID [varchar](50) ='',
	@TaskCode [char](10) ='',	
	@EndDateTime [datetime] =NULL,
	@Status [char](1)='',
	@ExecutionType char(1) = '0'
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;
	if (@ExecutionType ='0')	
	begin
		--Close any existing Pages
		UPDATE [dbo].[ZYCurrentUserTask]
		SET[EndDateTime] = GETDATE()
		  ,[Status] = '9'
		WHERE [UserName] = @UserName
		  AND [TaskCode] = @TaskCode
		  AND [Status] = '1'
		  
		INSERT INTO [dbo].[ZYCurrentUserTask]
           ([SessionID]
           ,[TaskCode]
           ,[UserName]
           ,[BusinessUnit]
           ,[StartDateTime]
           ,[EndDateTime]
           ,[Status])
		VALUES
           (@SessionID
           ,@TaskCode
           ,@UserName
           ,@BusinessUnit
           ,getdate()
           ,@EndDateTime
           ,@Status)
    end
    else if (@ExecutionType ='1')
    begin
    	UPDATE [dbo].[ZYCurrentUserTask]
		SET[EndDateTime] = GETDATE()
		  ,[Status] = '0'
		WHERE [SessionID] = @SessionID
		  AND [TaskCode] = @TaskCode
    
    end
 	
END
GO
