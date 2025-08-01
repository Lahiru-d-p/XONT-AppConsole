IF EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[usp_GetAuthorizedTaskURLs]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[usp_GetAuthorizedTaskURLs]
GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		Nazeer
-- Last Modified Version: 2.0.35.0
-- Description:	Get URLs for login user for tasks with access
-- =============================================
CREATE PROCEDURE  [dbo].[usp_GetAuthorizedTaskURLs]
-- Parameters
@UserName char(30)=''
AS

SELECT T.TaskCode, T.ExecutionScript as [url]
FROM dbo.ZYUserRole AS UR 

INNER JOIN dbo.ZYRoleMenu AS RM ON RM.RoleCode = UR.RoleCode

INNER JOIN dbo.ZYMenuDetail AS MD ON RM.MenuCode = MD.MenuCode

INNER JOIN dbo.ZYTask AS T ON T.TaskCode = MD.TaskCode

WHERE UR.UserName = @UserName;