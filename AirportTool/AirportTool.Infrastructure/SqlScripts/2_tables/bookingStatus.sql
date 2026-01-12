USE FlightBookingDb;
GO

PRINT 'Creating dbo.BookingStatus table...';

CREATE TABLE dbo.BookingStatus(
    Id INT IDENTITY(1,1) NOT NULL,
    Status NVARCHAR(40) NOT NULL,

    CONSTRAINT PK_BookingStatus PRIMARY KEY(Id)
);
GO
