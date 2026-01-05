USE FlightBookingDb;
GO

PRINT 'Creating dbo.Airline table...';

CREATE TABLE dbo.Airline (
	Id INT IDENTITY(1,1) NOT NULL,
	IATACode NCHAR(3) NOT NULL,
	Name NVARCHAR(100) NOT NULL,
	
	CONSTRAINT PK_Airline PRIMARY KEY (Id)
);
GO