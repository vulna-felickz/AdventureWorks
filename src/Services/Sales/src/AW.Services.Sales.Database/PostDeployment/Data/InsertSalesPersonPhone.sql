﻿IF NOT EXISTS (SELECT TOP 1 1 FROM SalesPersonPhone)
BEGIN
	PRINT CONVERT(varchar(20), GETDATE(), 113) + ' Populating table SalesPersonPhone...'

	INSERT [dbo].[SalesPersonPhone] ([SalesPersonID], [PhoneNumberType], [PhoneNumber]) VALUES (274, N'Cell', N'238-555-0197')
	INSERT [dbo].[SalesPersonPhone] ([SalesPersonID], [PhoneNumberType], [PhoneNumber]) VALUES (275, N'Cell', N'257-555-0154')
	INSERT [dbo].[SalesPersonPhone] ([SalesPersonID], [PhoneNumberType], [PhoneNumber]) VALUES (276, N'Work', N'883-555-0116')
	INSERT [dbo].[SalesPersonPhone] ([SalesPersonID], [PhoneNumberType], [PhoneNumber]) VALUES (277, N'Work', N'517-555-0117')
	INSERT [dbo].[SalesPersonPhone] ([SalesPersonID], [PhoneNumberType], [PhoneNumber]) VALUES (278, N'Work', N'922-555-0165')
	INSERT [dbo].[SalesPersonPhone] ([SalesPersonID], [PhoneNumberType], [PhoneNumber]) VALUES (279, N'Work', N'664-555-0112')
	INSERT [dbo].[SalesPersonPhone] ([SalesPersonID], [PhoneNumberType], [PhoneNumber]) VALUES (280, N'Cell', N'340-555-0193')
	INSERT [dbo].[SalesPersonPhone] ([SalesPersonID], [PhoneNumberType], [PhoneNumber]) VALUES (281, N'Cell', N'330-555-0120')
	INSERT [dbo].[SalesPersonPhone] ([SalesPersonID], [PhoneNumberType], [PhoneNumber]) VALUES (282, N'Work', N'185-555-0169')
	INSERT [dbo].[SalesPersonPhone] ([SalesPersonID], [PhoneNumberType], [PhoneNumber]) VALUES (283, N'Work', N'740-555-0182')
	INSERT [dbo].[SalesPersonPhone] ([SalesPersonID], [PhoneNumberType], [PhoneNumber]) VALUES (284, N'Work', N'615-555-0153')
	INSERT [dbo].[SalesPersonPhone] ([SalesPersonID], [PhoneNumberType], [PhoneNumber]) VALUES (285, N'Work', N'926-555-0182')
	INSERT [dbo].[SalesPersonPhone] ([SalesPersonID], [PhoneNumberType], [PhoneNumber]) VALUES (286, N'Cell', N'1 (11) 500 555-0190')
	INSERT [dbo].[SalesPersonPhone] ([SalesPersonID], [PhoneNumberType], [PhoneNumber]) VALUES (287, N'Work', N'775-555-0164')
	INSERT [dbo].[SalesPersonPhone] ([SalesPersonID], [PhoneNumberType], [PhoneNumber]) VALUES (288, N'Cell', N'1 (11) 500 555-0140')
	INSERT [dbo].[SalesPersonPhone] ([SalesPersonID], [PhoneNumberType], [PhoneNumber]) VALUES (289, N'Work', N'1 (11) 500 555-0145')
	INSERT [dbo].[SalesPersonPhone] ([SalesPersonID], [PhoneNumberType], [PhoneNumber]) VALUES (290, N'Cell', N'1 (11) 500 555-0117')
END
GO