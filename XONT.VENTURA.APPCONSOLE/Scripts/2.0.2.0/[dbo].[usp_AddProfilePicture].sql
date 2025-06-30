IF EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[usp_AddProfilePicture]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[usp_AddProfilePicture]
GO

SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[usp_AddProfilePicture]
@ProfilePicture image=null,
@UserName varchar(24)='',
@ProPicAvailable char(1) ='0'

AS

BEGIN UPDATE [dbo].[ZYUser]
SET ProfileImage = @ProfilePicture,
	ProPicAvailable =@ProPicAvailable
	where UserName = @UserName

END