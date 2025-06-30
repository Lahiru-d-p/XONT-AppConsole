IF EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[usp_UpdateZYActiveTask]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[usp_UpdateZYActiveTask]
GO

-- =============================================	
-- Last Modified Version : 2.0.31.0
-- =============================================  

CREATE PROCEDURE [dbo].[usp_UpdateZYActiveTask]	
	@BusinessUnit char(4) ='',  
	@ApplicationCode char(2)='',
	@TaskCode char(10) = '', 
	@ExclusivityMode int = 0 ,
	@UserName varchar(40)='',
	@WorkstationID varchar(60)='',
	@StatusFlag char(1) = '',
	@PowerUser char(1) = '', 
	@UserDB varchar(100) = '',
	@TerritoryCode char(4) = '',
	@SessionID varchar(500) = '',
	--@Password varchar(50)='',
	--@RoleCode nchar(10) ='', 
	--@MenuCode nchar(10) ='',
	
	@ExecutionType char(1) = ''
AS
BEGIN	

if(@ExecutionType = '1')
begin

	declare @NextJobNumber int = 0

	select @NextJobNumber = isnull(max(JobNumber), 0) + 1
	from  dbo.ZYActiveTask

	insert into dbo.ZYActiveTask
	(
	JobNumber
	, BusinessUnit
	, ApplicationCode
	, TaskCode
	, ExclusivityMode
	, UserName
	, DateTimeCreated
	, WorkstationID
	, ExplorerTime
	, StatusFlag
	, TerritoryCode
	, SessionID
	)
	values
	(
	@NextJobNumber
	, @BusinessUnit
	, @ApplicationCode
	, @TaskCode
	, @ExclusivityMode
	, @UserName
	, getdate()
	, @WorkstationID
	, getdate()
	, @StatusFlag
	, @TerritoryCode
	, @SessionID
	)
end

if(@ExecutionType = '2')
begin
	update dbo.ZYActiveTask
	set StatusFlag = '0'
	, EndDateTime = getdate()
	where BusinessUnit = @BusinessUnit 
	and TaskCode = case when  @TaskCode  <> '' then @TaskCode else TaskCode end
	and SessionID = @SessionID
	and StatusFlag = '1'
end
 	
END