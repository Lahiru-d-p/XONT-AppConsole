IF EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[usp_GetUserData]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[usp_GetUserData]
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
CREATE PROCEDURE  [dbo].[usp_GetUserData]
 
	-- Parameters
	@UserName char(30)='',
	@BusinessUnit nchar(4) ='',    
	@Password varchar(50)='',
	@RoleCode nchar(10) ='', 
	@MenuCode nchar(10) ='',
	@ExecutionType char(1) = '',
	@DefaultBusinessUnit char(1) = '' --V2004

AS

 IF @ExecutionType = '1'
		
 BEGIN
					SELECT ZYUser.UserName,ZYUser.UserFullName,ZYUserBusUnit.BusinessUnit,ZYUser.TimeStamp,ZYUser.UserLevelGroup,ZYUser.Password,ZYUser.ActiveFlag 
                    ,ZYUser.PasswordLocked,ZYUser.PowerUser,ZYUser.Theme,ZYUser.Language,ZYUser.FontColor,ZYUser.FontName,ZYUser.FontSize 
                    --VR002 Begin
                    ,DistributorCode
                    --VR002 End
                    --VR013
                    ,RestrictFOCInvoice
                    --VR013
                    ,SupplierCode--VR024
                    ,CustomerCode--VR024
                    ,ExecutiveCode --VR028
					,ProPicAvailable--New Ventura
					,PasswordChange --V2004
					,ZYUser.RoleCode  --V2014
                    FROM ZYUser 
                    INNER JOIN ZYUserBusUnit ON ZYUser.UserName = ZYUserBusUnit.UserName 
                     where ZYUser.UserName = @UserName  and ZYUser.Password=@Password
					 and ZYUserBusUnit.DefaultBusinessUnit = @DefaultBusinessUnit
   

   END


IF @ExecutionType = '2'
		
BEGIN

				SELECT   DISTINCT  ZYPasswordControl.MinLength, ZYPasswordControl.MaxLength,
                ZYPasswordControl.NoOfSpecialCharacters,ZYPasswordControl.ReusePeriod,
                ZYPasswordControl.NoOfAttempts,ZYPasswordControl.ExpirePeriodInMonths,ZYPasswordControl.UserExpirePeriodInMonths 
                FROM         ZYPasswordControl Inner Join ZYUserBusUnit ON 
                ZYUserBusUnit.BusinessUnit = ZYPasswordControl.BusinessUnit
                WHERE     (ZYUserBusUnit.BusinessUnit = @BusinessUnit)
				AND (ZYUserBusUnit.DefaultBusinessUnit = @DefaultBusinessUnit) --V2004
   

END

IF @ExecutionType = '3'

BEGIN
		
			SELECT  isnull(left(MAX(Date),12),LEFT(CONVERT(VARCHAR(26), GETDATE(), 100),12)) AS Date,isnull(right(MAX([Time] ),7),right((CONVERT(VARCHAR(26),GETDATE(), 100)),7))AS [Time]  
             From ZYPasswordLoginDetails 
             WHERE UserName=@UserName
              and   SuccessfulLogin='1' and
			 Date=(select Max(Date) from ZYPasswordLoginDetails  WHERE UserName=@UserName   
		      and SuccessfulLogin='1' )

END

IF @ExecutionType = '4'
	 BEGIN
                    SELECT     MAX(Date) AS Date, MAX(Time) AS Time, UserName, SuccessfulLogin, LogOutTime
                     FROM         ZYPasswordLoginDetails 
                     WHERE     (UserName = @UserName)
                     GROUP BY UserName, SuccessfulLogin, LogOutTime HAVING      (SuccessfulLogin = N'1') AND (LogOutTime IS  NULL)



END



-- exec [dbo].[usp_GetUserData] 'nadeeka','','dmHNOXNYsfw=','','','1'