IF EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[usp_CheckNoOfAttemptsExceeded]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[usp_CheckNoOfAttemptsExceeded]
GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE  [dbo].[usp_CheckNoOfAttemptsExceeded]
@UserName char(30)=''
	
AS

BEGIN 

		SELECT (ISNULL(A.AtualAttempts,0)-ISNULL(ZYPasswordControl.NoOfAttempts,0)) as Attempt  
		FROM
		(
			SELECT COUNT(SuccessfulLogin) AtualAttempts,UserName
			From ZYPasswordLoginDetails
						 
			WHERE ZYPasswordLoginDetails.UserName=@UserName
			and   SuccessfulLogin='0'

			and Date>=
			(select Max(Date) from ZYPasswordLoginDetails  WHERE UserName=@UserName and SuccessfulLogin='1' )
			 
			and Time>(
			SELECT MAX(Time)
			From ZYPasswordLoginDetails 
			WHERE UserName=@UserName and SuccessfulLogin='1' 
			and Date=
			(select Max(Date) from ZYPasswordLoginDetails  WHERE UserName=@UserName and SuccessfulLogin='1' )
			)
			GROUP BY UserName
		) A

		INNER JOIN ZYUserBusUnit ON ZYUserBusUnit.UserName=A.UserName
		INNER JOIN ZYPasswordControl ON ZYPasswordControl.BusinessUnit=ZYUserBusUnit.BusinessUnit
		
		
		
END 