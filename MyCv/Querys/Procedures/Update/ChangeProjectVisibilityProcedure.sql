create procedure ChangeProjectVisibility	
   @projectId nchar(4),
   @visibility bit
as
BEGIN

SET NOCOUNT ON;
   BEGIN TRY
      BEGIN TRAN;
	     Update Projects SET deleteId = @visibility where projectId = @projectId;
	  COMMIT TRAN;
	   END TRY
    BEGIN CATCH
        IF @@TRANCOUNT > 0
            ROLLBACK TRAN;

        DECLARE @ErrMessage NVARCHAR(4000) = ERROR_MESSAGE();
        SELECT 0 AS Success, @ErrMessage AS Message;
    END CATCH
END 