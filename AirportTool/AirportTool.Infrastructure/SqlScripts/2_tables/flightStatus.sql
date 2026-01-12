USE FlightBookingDb;
GO

PRINT 'Creating dbo.FlightStatus table...';

CREATE TABLE dbo.FlightStatus(
    Id INT IDENTITY(1,1) NOT NULL,
    Status NVARCHAR(40) NOT NULL,

    CONSTRAINT PK_FlightStatus PRIMARY KEY(Id)
);
GO
