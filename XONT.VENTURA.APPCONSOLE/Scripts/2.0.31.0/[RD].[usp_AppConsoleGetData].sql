IF EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[RD].[usp_AppConsoleGetData]') AND type in (N'P', N'PC'))
DROP PROCEDURE [RD].[usp_AppConsoleGetData]
GO

-- =============================================	
-- Last Modified Version : 2.0.31.0
-- =============================================  

CREATE PROCEDURE [RD].[usp_AppConsoleGetData]
	@BusinessUnit char(4) ='',  
	@UserName varchar(40)='',
	@PowerUser char(1) = '', 
	@TerritoryCode char(4) = '',
	@ExecutionType char(1) = ''
AS
BEGIN	

if(@ExecutionType = '1')
begin
	select TerritoryCode  
	 from XA.UserTerritory T
	 where T.BusinessUnit = @BusinessUnit and T.UserName = @UserName
	 and Status = '1'

end

 	
END