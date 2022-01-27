CREATE PROCEDURE [dbo].[sp_UpdateUser] 
(
	@userId  nvarchar(450),
	@isAdmin integer = 0, 
	@keepLogged bit = 0
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
      SAVE TRANSACTION sp_UpdateUser;

    IF  NOT EXISTS(SELECT ID FROM AspNetUsers  AU WHERE AU.Id  =  @userId)
		BEGIN
			RAISERROR ('sp_UpdateUser raise an error: "User  with  the ID does  not  exist in table aspnetuser". %d: %s', 16, 1) ;
		END

	IF NOT EXISTS(SELECT ID FROM Users U WHERE U.Id  =  @userId)
		BEGIN
			RAISERROR ('sp_UpdateUser raise an error: "User  with  the  ID does  not exist in table Users". %d: %s', 16, 1) ;
		END

	UPDATE Users  SET DateUpdated  =  GETDATE(), isAdmin =  @isAdmin, keepLogged = @keepLogged  WHERE Users.ID = @userId
	

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
      ROLLBACK TRANSACTION sp_UpdateUser;
    RAISERROR ('sp_UpdateUser raise an error: %d: %s', 16, 1, @error, @message) ;
    RETURN -1
  END CATCH  

  RETURN 1;
END

