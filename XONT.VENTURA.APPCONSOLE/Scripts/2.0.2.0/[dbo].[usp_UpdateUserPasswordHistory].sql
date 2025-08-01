IF EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[usp_UpdateUserPasswordHistory]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[usp_UpdateUserPasswordHistory]
GO

/****** Object:  StoredProcedure [dbo].[usp_UpdateUserPasswordHistory]    Script Date: 01/07/2011 09:16:40 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE  [dbo].[usp_UpdateUserPasswordHistory]
 
@UserName char(30),@Password varchar(50),@PasswordChange char(1)
AS
BEGIN
			BEGIN
				UPDATE [dbo].[ZYUser]
				SET  [Password] = @Password ,
					 [PasswordChange] = @PasswordChange
				WHERE UserName=@UserName
			END

			BEGIN
			INSERT INTO [dbo].[ZYPasswordHistory]
				   ([UserName]
				   ,[Date]
				   ,[Time]
				   ,[Password]
				   ,[PasswordType])
				VALUES
				   (@UserName, left(Convert(varchar(200),getdate(),109), 12),right(Convert(varchar(50),getdate(),109),14)
				   ,@Password,'C')
			END
END



