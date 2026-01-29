alter PROCEDURE CreateProject
    @title NVARCHAR(100),
    @description NVARCHAR(MAX),
    @coverImgUrl NVARCHAR(50),
    @tags NVARCHAR(max),
    @details ProjectDetailsType READONLY
AS
BEGIN
    SET NOCOUNT ON;

    DECLARE @newProjectId NCHAR(4);
    
    BEGIN TRY
        BEGIN TRAN;

        -- 1. Benzersiz proje kimliði oluþturma
        -- Not: 4 karakterli ID'lerde çarpýþma (collision) riski yüksektir, döngü güvenli kýlýndý.
        DECLARE @counter INT = 0;
        WHILE 1 = 1
        BEGIN
            SET @newProjectId = LEFT(UPPER(REPLACE(NEWID(), '-', '')), 4);
            IF NOT EXISTS (SELECT 1 FROM Projects WHERE projectId = @newProjectId)
                BREAK;

            SET @counter = @counter + 1;
            IF @counter > 100 -- Sonsuz döngü korumasý
                RAISERROR('Benzersiz bir Proje ID oluþturulamadý. Lütfen tekrar deneyin.', 16, 1);
        END

        -- 2. Proje oluþturma
        INSERT INTO Projects (projectId, title, description, coverImgUrl, tags, deleteId)
        VALUES (@newProjectId, @title, @description, @coverImgUrl, @tags, 0);

        -- 3. Proje detaylarýný ekleme
        INSERT INTO ProjectDetails (projectId, type, visibleContent, content, subContent, deleteId)
        SELECT @newProjectId, type, visibleContent, content, subContent, 0 
        FROM @details;

        -- Her þey yolundaysa kaydet
        COMMIT TRAN;

        -- Baþarý durumunda yeni ID'yi döndür
        SELECT 1 AS Success, 'Proje baþarýyla oluþturuldu' AS Message, @newProjectId AS ProjectId;

    END TRY
    BEGIN CATCH
        -- Hata varsa geri al
        IF @@TRANCOUNT > 0
            ROLLBACK TRAN;

        -- Hata bilgilerini döndür
        DECLARE @ErrorMessage NVARCHAR(4000) = ERROR_MESSAGE();
        DECLARE @ErrorSeverity INT = ERROR_SEVERITY();
        DECLARE @ErrorState INT = ERROR_STATE();

        SELECT 0 AS Success, @ErrorMessage AS Message, NULL AS ProjectId;
        -- Opsiyonel: Hatayý dýþarýya (uygulamaya) fýrlat
        -- RAISERROR (@ErrorMessage, @ErrorSeverity, @ErrorState);
    END CATCH
END


select * from AdminLoginAttempts
