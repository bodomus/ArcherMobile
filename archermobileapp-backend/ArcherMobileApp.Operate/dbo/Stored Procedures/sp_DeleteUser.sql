CREATE PROCEDURE [dbo].[sp_DeleteUser] 
(
	@userId  nvarchar(450)
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
      SAVE TRANSACTION sp_DeleteUser;

    DELETE FROM Users  WHERE Users.ID = @userId
	DELETE FROM [dbo].[AspNetUserRoles]  WHERE UserId  =  @userId
	DELETE FROM [dbo].[AspNetUsers]  WHERE Id  =  @userId

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
      ROLLBACK TRANSACTION sp_DeleteUser;
    RAISERROR ('sp_DeleteUser raise an error: %d: %s', 16, 1, @error, @message) ;
    RETURN -1
  END CATCH  

  RETURN 1;
END

