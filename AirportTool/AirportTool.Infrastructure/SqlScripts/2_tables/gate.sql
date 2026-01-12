USE FlightBookingDb;
GO

PRINT 'Creating dbo.Gate table...';

CREATE TABLE dbo.Gate(
    Id INT IDENTITY(1,1) NOT NULL,
    AirportId INT NOT NULL,
    Code NVARCHAR(10) NOT NULL,

    CONSTRAINT PK_Gate PRIMARY KEY(Id)
);
GO
