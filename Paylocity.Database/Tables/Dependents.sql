CREATE TABLE [dbo].[Dependents]
(
	[DependentId]	INT NOT NULL PRIMARY KEY IDENTITY(1, 1), 
	[EmployeeId]	INT NOT NULL,
	[FirstName]		VARCHAR (50) NOT NULL,
	[LastName]		VARCHAR (50) NOT NULL,
	[RelationToEmployee] INT NOT NULL DEFAULT(3),
	CONSTRAINT [FK_Dependents_Employees] FOREIGN KEY ([EmployeeId]) REFERENCES [Employees]([EmployeeId]),
	CONSTRAINT [FK_Dependents_Employees_Relationship] FOREIGN KEY ([RelationToEmployee]) REFERENCES [Relationship]([RelationId])
)

