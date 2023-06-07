DROP VIEW IF EXISTS PersonsClasses;

CREATE VIEW PersonsClasses AS
SELECT p.Id, p.Last, p.First, c.Name as Class, p.UserId
FROM Persons p
	INNER JOIN Classes c
    	ON c.Id = p.ClassId
ORDER BY c.Name, p.Last, p.First;
