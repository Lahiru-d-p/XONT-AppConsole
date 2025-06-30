SET ANSI_PADDING OFF
GO
ALTER TABLE [dbo].[ZYUserBusUnit] ADD    [DefaultBusinessUnit] [char](1) NOT NULL DEFAULT ('0')
GO
alter table dbo.ZYBusinessUnit add LicenseAlertMailIDs varchar(200) not null default('')
go 
alter table dbo.ZYBusinessUnit add LastLicenseAlertSentDate datetime null default(null)
go