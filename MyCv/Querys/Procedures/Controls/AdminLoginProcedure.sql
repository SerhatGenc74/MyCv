create PROCEDURE AdminLogin
    @nickName VARCHAR(25),
    @password VARCHAR(MAX),
    @deviceId UNIQUEIDENTIFIER,
    @userId VARCHAR(5) OUTPUT
AS
BEGIN
    SET NOCOUNT ON;

    DECLARE 
        @storedEncryptedPassword VARBINARY(MAX),
        @decryptedPassword VARCHAR(MAX),
        @lockUntil DATETIME;

    -- 1. CİHAZ KİLİT KONTROLÜ
    SELECT @lockUntil = LockUntil
    FROM AdminLoginAttempts
    WHERE DeviceId = @deviceId;

    IF @lockUntil IS NOT NULL AND @lockUntil > GETDATE()
    BEGIN
        DECLARE @dakika INT = DATEDIFF(MINUTE, GETDATE(), @lockUntil);
        RAISERROR ('Bu cihazdan çok fazla hatalı deneme yapıldı. %d dakika bekleyin.', 16, 1, @dakika);
        RETURN;
    END

    -- 2. ŞİFRE İŞLEMLERİ (Hata yönetimi ile)
    BEGIN TRY
        OPEN SYMMETRIC KEY MySymmetricKey DECRYPTION BY CERTIFICATE MyCert;

        SELECT @storedEncryptedPassword = password, @userId = userId
        FROM AdminUsers WHERE nickName = @nickName;

        IF @storedEncryptedPassword IS NOT NULL
            SET @decryptedPassword = CONVERT(VARCHAR(MAX), DECRYPTBYKEY(@storedEncryptedPassword));

        CLOSE SYMMETRIC KEY MySymmetricKey;
    END TRY
    BEGIN CATCH
        -- Hata durumunda anahtarı kapatmayı unutma
            CLOSE SYMMETRIC KEY MySymmetricKey;
        
        THROW;
    END CATCH

    -- 3. SONUÇ VE LOGLAMA
    -- Şifre doğruysa ve kullanıcı mevcutsa
    IF @decryptedPassword IS NOT NULL AND @decryptedPassword = @password
    BEGIN
        -- Giriş başarılı: Eğer listede varsa bu cihazın kayıtlarını temizle
        -- (İstediğin ana düzenleme burası)
        DELETE FROM AdminLoginAttempts WHERE DeviceId = @deviceId;
    END
    ELSE
    BEGIN
        -- Giriş başarısız: Cihaza ceza puanı ekle
        -- Output değişkenini temizle ki yanlış veri dönmesin
        SET @userId = NULL; 
        
        EXEC IncreaseFailCount @deviceId;
        RAISERROR ('Kullanıcı adı veya şifre hatalı.', 16, 1);
        RETURN;
    END
END