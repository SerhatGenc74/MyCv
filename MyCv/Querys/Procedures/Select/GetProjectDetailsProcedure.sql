Create Procedure GetProjectDetails
@projectId nchar(4)
AS
BEGIN
SET NOCOUNT ON;
Select * From ProjectDetails where projectId = @projectId;
END