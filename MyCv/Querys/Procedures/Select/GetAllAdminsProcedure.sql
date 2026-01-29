create Procedure GetAllAdmins
AS
BEGIN
 SET NOCOUNT ON;
 
 OPEN SYMMETRIC KEY MySymmetricKey
 DECRYPTION BY CERTIFICATE MyCert;

Select 
    userId, 
	nickName,
	Role,
	email,
    CASE 
	    WHEN Role = 'Admin' THEN '***'
		WHEN Role = 'NewAdmin' THEN Convert(nvarchar(max),DecryptbyKey(password))
		else 'Yetkisiz'
	end as password
	from AdminUsers;

END