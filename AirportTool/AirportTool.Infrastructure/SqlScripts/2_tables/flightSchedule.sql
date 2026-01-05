USE FlightBookingDb;
GO

PRINT 'Creating dbo.FlightSchedule table...';

CREATE TABLE dbo.FlightSchedule(
    Id INT IDENTITY(1,1) NOT NULL,
    FlightId INT NOT NULL,
    ScheduledDepartureUtc DATETIME2 NOT NULL,
    ScheduledArrivalUtc DATETIME2 NOT NULL,
    GateId INT NULL,
    AssignedAircraftId INT NULL,
    StatusId INT NOT NULL,

    CONSTRAINT PK_FlightSchedule PRIMARY KEY(Id)
);
GO
