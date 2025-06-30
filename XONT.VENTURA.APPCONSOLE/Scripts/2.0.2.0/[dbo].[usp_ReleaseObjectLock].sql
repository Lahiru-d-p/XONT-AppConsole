IF EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[usp_ReleaseObjectLock]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[usp_ReleaseObjectLock]
GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE  [dbo].[usp_ReleaseObjectLock]
	@BusinessUnit char(4) ='',
	@SessionID varchar(50) = ''
	  
AS
BEGIN	

        UPDATE dbo.ZYObjectLock
		SET StatusFlag='0'
           ,UnlockReson='Session Expired'
                                                
		WHERE BusinessUnit=@BusinessUnit
			AND SessionID=@SessionID	
	
END