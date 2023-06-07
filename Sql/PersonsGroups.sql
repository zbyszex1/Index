DROP VIEW IF EXISTS PersonsGroups;

CREATE VIEW PersonsGroups AS
SELECT p.Class, count(*) as Count
FROM PersonsClasses p
GROUP BY p.Class
ORDER BY p.Class;
