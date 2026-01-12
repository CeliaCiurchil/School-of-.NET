USE FlightBookingDb;
GO

PRINT 'Creating dbo.Aircraft table...';

CREATE TABLE dbo.Aircraft(
	Id INT IDENTITY(1,1) NOT NULL,
	TailNumber NVARCHAR(10) NOT NULL,
	Model NVARCHAR(60) NOT NULL,
	SeatCapacity INT NOT NULL,
	AirlineId INT,

	CONSTRAINT PK_Aircraft PRIMARY KEY (Id)
);