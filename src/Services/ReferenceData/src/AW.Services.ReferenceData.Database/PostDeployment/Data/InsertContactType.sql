﻿IF NOT EXISTS (SELECT TOP 1 1 FROM ContactType)
BEGIN
	PRINT CONVERT(varchar(20), GETDATE(), 113) + ' Populating table ContactType...'

	INSERT INTO ContactType (Name) VALUES ('Accounting Manager')
	INSERT INTO ContactType (Name) VALUES ('Assistant Sales Agent')
	INSERT INTO ContactType (Name) VALUES ('Assistant Sales Representative')
	INSERT INTO ContactType (Name) VALUES ('Coordinator Foreign Markets')
	INSERT INTO ContactType (Name) VALUES ('Export Administrator')
	INSERT INTO ContactType (Name) VALUES ('International Marketing Manager')
	INSERT INTO ContactType (Name) VALUES ('Marketing Assistant')
	INSERT INTO ContactType (Name) VALUES ('Marketing Manager')
	INSERT INTO ContactType (Name) VALUES ('Marketing Representative')
	INSERT INTO ContactType (Name) VALUES ('Order Administrator')
	INSERT INTO ContactType (Name) VALUES ('Owner')
	INSERT INTO ContactType (Name) VALUES ('Owner/Marketing Assistant')
	INSERT INTO ContactType (Name) VALUES ('Product Manager')
	INSERT INTO ContactType (Name) VALUES ('Purchasing Agent')
	INSERT INTO ContactType (Name) VALUES ('Purchasing Manager')
	INSERT INTO ContactType (Name) VALUES ('Regional Account Representative')
	INSERT INTO ContactType (Name) VALUES ('Sales Agent')
	INSERT INTO ContactType (Name) VALUES ('Sales Associate')
	INSERT INTO ContactType (Name) VALUES ('Sales Manager')
	INSERT INTO ContactType (Name) VALUES ('Sales Representative')
END
GO