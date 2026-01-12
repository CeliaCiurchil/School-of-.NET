USE FlightBookingDb;
GO

PRINT 'Creating dbo.Airport table...';

CREATE TABLE dbo.Airport(
    Id INT IDENTITY(1,1) NOT NULL,
    IATACode NCHAR(3) NOT NULL,
    Name NVARCHAR(120) NOT NULL,
    TimeZone NVARCHAR(64) NOT NULL,
    AddressId INT NOT NULL,

    CONSTRAINT PK_Airport PRIMARY KEY(Id)
);
GO
