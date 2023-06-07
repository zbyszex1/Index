DROP VIEW IF EXISTS UsersRoles;

CREATE VIEW UsersRoles AS
SELECT u.Id, u.Name, u.Email, u.Phone, r.Level, r.Name as Role
FROM Users u
	INNER JOIN Roles r
    	ON r.Id = u.RoleId
ORDER BY r.Name, u.Name;
