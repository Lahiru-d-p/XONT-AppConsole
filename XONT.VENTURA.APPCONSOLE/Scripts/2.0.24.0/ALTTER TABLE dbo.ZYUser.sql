ALTER TABLE [dbo].[ZYUser]  ADD POReturnAuthorizationLevel char(1) NOT NULL default '0';
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'0:No Authorization 1:Authorization Level 1 2:Authorization Level 2 3:Authorization Level 3', @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ZYUser', @level2type=N'COLUMN',@level2name=N'POReturnAuthorizationLevel'
GO
ALTER TABLE [dbo].[ZYUser]  ADD POReturnAuthorizationUpTo char(1) NOT NULL default '0';
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'0:No 1:Yes', @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ZYUser', @level2type=N'COLUMN',@level2name=N'POReturnAuthorizationUpTo'
GO