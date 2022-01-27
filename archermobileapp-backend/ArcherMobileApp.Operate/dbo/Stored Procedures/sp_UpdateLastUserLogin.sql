CREATE PROCEDURE [dbo].[sp_UpdateLastUserLogin] 
(
	@userId  nvarchar(450), 
	@lastLogin  DATETIME 
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
      SAVE TRANSACTION sp_UpdateLastUserLogin;

    UPDATE Users SET Users.LastLogin = @lastLogin WHERE Users.ID = @userId
	
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
      ROLLBACK TRANSACTION sp_UpdateLastUserLogin;
    RAISERROR ('sp_UpdateLastUserLogin raise an error: %d: %s', 16, 1, @error, @message) ;
    RETURN -1
  END CATCH  

  RETURN 1;
END
