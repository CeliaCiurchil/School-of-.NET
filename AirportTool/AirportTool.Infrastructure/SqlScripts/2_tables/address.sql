USE FlightBookingDb;
GO

PRINT 'Creating dbo.Address table...';

CREATE TABLE dbo.Address (
    Id      INT IDENTITY(1,1) NOT NULL,
    Country NVARCHAR(80)     NOT NULL,
    City    NVARCHAR(80)     NOT NULL,
    Street  NVARCHAR(200)     NOT NULL,

    CONSTRAINT PK_Address PRIMARY KEY (Id)
);
GO