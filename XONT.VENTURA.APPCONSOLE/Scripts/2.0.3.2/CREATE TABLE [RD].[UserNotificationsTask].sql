
/****** Object:  Table [RD].[UserNotificationsTask]    Script Date: 7/26/2017 6:06:45 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

SET ANSI_PADDING ON
GO

CREATE TABLE [RD].[UserNotificationsTask](
	[RecID] [bigint] IDENTITY(1,1) NOT NULL,
	[BusinessUnit] [char](4) NOT NULL,
	[UserName] [char](30) NOT NULL,	
	[SourceTaskCode] [char](10)NOT NULL,
	[TaskCode] [char](10) NOT NULL,
	[Description] [varchar](200)NOT NULL,
	[Status] [char](1) NOT NULL,
	[CreatedBy] [varchar](40) NOT NULL,
	[CreatedOn] [datetime] NOT NULL,
	[UpdatedBy] [varchar](40) NOT NULL,
	[UpdatedOn] [datetime] NOT NULL,
 CONSTRAINT [PK_UserNotificationsTask] PRIMARY KEY CLUSTERED 
(
	[RecID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, FILLFACTOR = 70) ON [PRIMARY]
) ON [PRIMARY]

GO

SET ANSI_PADDING OFF
GO


