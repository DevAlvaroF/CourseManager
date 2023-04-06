CREATE DATABASE CourseReport;
GO

USE CourseReport;
GO

CREATE TABLE [dbo].[Course]
(
	CourseId INT IDENTITY(1,1) NOT NULL,
	CourseCode VARCHAR(5) NOT NULL,
	[Description] VARCHAR(50) NOT NULL,
	PRIMARY KEY(CourseId)
);
	
GO

CREATE TABLE [dbo].[Student]
(
	StudentId INT IDENTITY(1,1) NOT NULL,
	FirstName VARCHAR(50) NOT NULL,
	LastName VARCHAR(50) NOT NULL,
	PRIMARY KEY(StudentId)
);
GO

CREATE TABLE [dbo].[Enrollments]
(
	EnrollmentId INT IDENTITY(1,1) NOT NULL,
	StudentId INT NOT NULL,
	CourseId INT NOT NULL,
	CreateDate DATETIME NOT NULL DEFAULT(GETDATE()),
	CreateId VARCHAR(50) NOT NULL DEFAULT('System'),
	UpdateDate DATETIME NOT NULL DEFAULT(GETDATE()),
	UpdateId VARCHAR(50) NOT NULL DEFAULT('System'),
	FOREIGN KEY(StudentId) REFERENCES Student(StudentId),
	FOREIGN KEY(CourseId) REFERENCES Course(CourseId),
	PRIMARY KEY(EnrollmentId)
);
GO

-- Course
INSERT INTO [dbo].[Course] (CourseCode, [Description]) VALUES ('AF', 'Accounting & Finance');
INSERT INTO [dbo].[Course] (CourseCode, [Description]) VALUES ('ME', 'Aeronautical & Manufacturing Engineering');
INSERT INTO [dbo].[Course] (CourseCode, [Description]) VALUES ('AF', 'Agriculture & Forestry');
INSERT INTO [dbo].[Course] (CourseCode, [Description]) VALUES ('AS', 'American Studies');
INSERT INTO [dbo].[Course] (CourseCode, [Description]) VALUES ('APSY', 'Anatomy & Physiology');
INSERT INTO [dbo].[Course] (CourseCode, [Description]) VALUES ('ANT', 'Anthropology');
INSERT INTO [dbo].[Course] (CourseCode, [Description]) VALUES ('ARC', 'Archaeology');
INSERT INTO [dbo].[Course] (CourseCode, [Description]) VALUES ('ARCH', 'Architecture');

-- Student
INSERT INTO [dbo].[Student] (FirstName, LastName) VALUES ('Millard', 'Lamb');
INSERT INTO [dbo].[Student] (FirstName, LastName) VALUES ('Gayle', 'Reid');
INSERT INTO [dbo].[Student] (FirstName, LastName) VALUES ('Quinton', 'Beltran');
INSERT INTO [dbo].[Student] (FirstName, LastName) VALUES ('Eusebio', 'Moyer');
INSERT INTO [dbo].[Student] (FirstName, LastName) VALUES ('Imelda', 'Shea');
INSERT INTO [dbo].[Student] (FirstName, LastName) VALUES ('Ellsworth', 'Fletcher');

-- Enrollments
INSERT INTO [dbo].[Enrollments] (StudentId, CourseId) VALUES (1, 1);
INSERT INTO [dbo].[Enrollments] (StudentId, CourseId) VALUES (2, 1);
INSERT INTO [dbo].[Enrollments] (StudentId, CourseId) VALUES (3, 1);
INSERT INTO [dbo].[Enrollments] (StudentId, CourseId) VALUES (1, 2);
INSERT INTO [dbo].[Enrollments] (StudentId, CourseId) VALUES (2, 2);
INSERT INTO [dbo].[Enrollments] (StudentId, CourseId) VALUES (3, 2);
INSERT INTO [dbo].[Enrollments] (StudentId, CourseId) VALUES (4, 2);
INSERT INTO [dbo].[Enrollments] (StudentId, CourseId) VALUES (5, 3);
INSERT INTO [dbo].[Enrollments] (StudentId, CourseId) VALUES (6, 4);
GO

-- View for C# Program
CREATE VIEW [dbo].[EnrollmentReport] AS
	SELECT
		t1.EnrollmentId,
		t2.FirstName,
		t2.LastName,
		t3.CourseCode,
		t3.[Description]
	FROM
		[dbo].[Enrollments] t1
	INNER JOIN
		[dbo].[Student] t2 ON t2.StudentId = t1.StudentId
	INNER JOIN
		[dbo].[Course] t3 ON t3.CourseId = t1.CourseId
GO

-- Store Procedure to make c# Easier to Fetch Data
-- Data will be grabbed from the View
CREATE PROCEDURE [dbo].[EnrollmentReport_GetList]
AS
	SELECT
		EnrollmentId,
		FirstName,
		LastName,
		CourseCode,
		[Description]
	FROM
		[dbo].[EnrollmentReport]
GO


CREATE PROCEDURE [dbo].[Course_GetList]
AS
	SELECT
		CourseId,
		CourseCode,
		[Description]
	FROM
		[dbo].[Course]

GO

CREATE PROCEDURE [dbo].[Student_GetList]
AS
	SELECT
		StudentId,
		FirstName,
		LastName
	FROM
		[dbo].[Student]

GO

CREATE PROCEDURE [dbo].[Enrollments_GetList]
AS
	SELECT
		EnrollmentId,
		StudentId,
		CourseId
	FROM
		[dbo].[Enrollments]

GO

CREATE TYPE [dbo].[EnrollmentType] AS TABLE
(
	[EnrollmentId] INT NOT NULL,
	[StudentId] INT NOT NULL,
	[CourseId] INT NOT NULL
);

GO

CREATE PROCEDURE [dbo].[Enrollments_Upsert]
	@EnrollmentType [EnrollmentType] READONLY,
	@UserId VARCHAR(50)
AS
	MERGE INTO [dbo].[Enrollments] TARGET
	USING
	(
		SELECT
			[EnrollmentId],
			[StudentId],
			[CourseId],
			@UserId [UpdateId],
			GETDATE() [UpdateDate],
			@UserId [CreateId],
			GETDATE() [CreateDate]
		FROM
			@EnrollmentType
	) AS SOURCE

	ON
	(
		TARGET.[EnrollmentId] = SOURCE.[EnrollmentId]
	)
	WHEN MATCHED THEN
		UPDATE SET
			TARGET.[StudentId] = SOURCE.[StudentId],
			TARGET.[CourseId] = SOURCE.[CourseId],
			TARGET.[UpdateId] = SOURCE.[UpdateId],
			TARGET.[UpdateDate] = SOURCE.[UpdateDate]
	WHEN NOT MATCHED BY TARGET then
		INSERT
		(
			[StudentId],
			[CourseId],
			[CreateDate],
			[CreateId],
			[UpdateDate],
			[UpdateId]
		)
		VALUES
		(
			SOURCE.[StudentId],
			SOURCE.[CourseId],
			SOURCE.[CreateDate],
			SOURCE.[CreateId],
			SOURCE.[UpdateDate],
			SOURCE.[UpdateId]
		);
GO