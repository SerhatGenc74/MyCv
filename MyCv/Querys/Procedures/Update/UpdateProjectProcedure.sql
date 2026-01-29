create PROCEDURE UpdateProject
    @projectId NCHAR(4),
    @title NVARCHAR(100),
    @description NVARCHAR(MAX),
    @coverImgUrl NVARCHAR(50),
    @tags NVARCHAR(MAX),
    @deleteId BIT,
    @details ProjectDetailsType READONLY
AS
BEGIN
    SET NOCOUNT ON;

    BEGIN TRY
        BEGIN TRAN;

        -- 1. Ana proje bilgilerini güncelle
        UPDATE Projects 
        SET title = @title, 
            description = @description, 
            coverImgUrl = @coverImgUrl, 
            tags = @tags,
            deleteId = @deleteId 
        WHERE projectId = @projectId;

        -- 2. Eski detaylarýn tamamýný sil
        DELETE FROM ProjectDetails 
        WHERE projectId = @projectId;

        -- 3. Gelen yeni detaylarý topluca ekle
        -- Not: ProjectDetailsType içindeki 'id' kolonu IDENTITY ise buraya yazýlmaz.
        INSERT INTO ProjectDetails (projectId, type, visibleContent, content, subContent, deleteId)
        SELECT @projectId, type, visibleContent, content, subContent, deleteId
        FROM @details;

        COMMIT TRAN;
        SELECT 1 AS Success, 'Proje ve detaylar baþarýyla güncellendi' AS Message;

    END TRY
    BEGIN CATCH
        IF @@TRANCOUNT > 0
            ROLLBACK TRAN;

        DECLARE @ErrMessage NVARCHAR(4000) = ERROR_MESSAGE();
        SELECT 0 AS Success, @ErrMessage AS Message;
    END CATCH
END