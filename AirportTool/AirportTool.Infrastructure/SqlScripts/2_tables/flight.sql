USE FlightBookingDb;
GO

PRINT 'Creating dbo.Flight table...';

CREATE TABLE dbo.Flight(
    Id INT IDENTITY(1,1) NOT NULL,
    AirlineId INT NOT NULL,
    FlightNumber NVARCHAR(8) NOT NULL,
    OriginAirportId INT NOT NULL,
    DestinationAirportId INT NOT NULL,
    DefaultAircraftId INT,
    IsActive BIT NOT NULL CONSTRAINT DF_Flight_IsActive DEFAULT(1),

    CONSTRAINT PK_Flight PRIMARY KEY(Id)
);
GO
