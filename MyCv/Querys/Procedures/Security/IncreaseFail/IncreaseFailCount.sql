create PROCEDURE IncreaseFailCount
    @deviceId UNIQUEIDENTIFIER
AS
BEGIN
    SET NOCOUNT ON;

    -- Cihaz kaydý var mý kontrol et ve güncelle
    UPDATE AdminLoginAttempts
    SET 
        FailedCount = FailedCount + 1,
        LastAttempt = GETDATE(),
        LockUntil = CASE 
            WHEN FailedCount + 1 >= 5 THEN DATEADD(MINUTE, 15, GETDATE()) 
            ELSE NULL 
        END
    WHERE DeviceId = @deviceId;

    -- Eðer cihaz kaydý yoksa yeni ekle
    IF @@ROWCOUNT = 0
    BEGIN
        INSERT INTO AdminLoginAttempts (DeviceId, FailedCount, LastAttempt)
        VALUES (@deviceId, 1, GETDATE());
    END
END