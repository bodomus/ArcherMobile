CREATE PROCEDURE [dbo].[sp_GetUsers] 
(
	@userId  nvarchar(450)  =  NULL,
	@start integer = 1,
	@page integer  =  1
)
AS
BEGIN
    SET NOCOUNT ON
	DECLARE @trancount int; 
	SET @trancount = @@trancount;
	BEGIN TRY
    IF @trancount = 0
      BEGIN TRANSACTION
    ELSE
      SAVE TRANSACTION sp_GetUsers;


	SELECT 
	U.Id  as  UserId,  
	AR.Id  as  RoleId,
	AR.Name  as  RoleName,
	U.UserName  as   UserName,  
	AU.Email  as  Email,  
	AU.EmailConfirmed  as  EmailConfirmed,
	AU.LockoutEnabled  as  LockoutEnabled,
	U.IsAdmin  as  IsAdmin
	,U.LastLogin  as  LastLogin
	,U.ConfirmICR as ConfirmICR
	,U.KeepLogged as KeepLogged
	
	FROM [dbo].[AspNetUsers] AU JOIN  [dbo].[AspNetUserRoles]  AUR  ON AU.Id =  AUR.UserId  
	JOIN [dbo].[AspNetRoles] AR ON AUR.RoleID = AR.Id 
	JOIN Users  U ON  AU.ID  = U.Id
	WHERE UserId =  ISNULL(@userId, UserId)  
	UNION
	SELECT '1'  as  UserId,  
	'1'  as  RoleId,
	'1'  as  RoleName,
	'1'  as   UserName,  
	'1'  as  Email,  
	1  as  EmailConfirmed,
	0  as  LockoutEnabled,
	1  as  IsAdmin,
	GETDATE(),
	1 as ConfirmICR,
	1 as  KeepLogged
	
	IF @trancount = 0   
      COMMIT;
   
  END TRY
  BEGIN CATCH
    DECLARE @error int, @message varchar(4000), @xstate int;
    SELECT @error = ERROR_NUMBER(), @message = ERROR_MESSAGE(), @xstate = XACT_STATE();
    IF @xstate = -1
      ROLLBACK;
    IF @xstate = 1 AND @trancount = 0
      ROLLBACK
    IF @xstate = 1 AND @trancount > 0
      ROLLBACK TRANSACTION sp_GetUsers;
    RAISERROR ('sp_GetUsers raise an error: %d: %s', 16, 1, @error, @message) ;
    RETURN -1
  END CATCH  

  RETURN 0;
END



--[sp_GetUsers] 'bbdb2d8b-0ac2-4603-8496-35b8766d615d'
GO

