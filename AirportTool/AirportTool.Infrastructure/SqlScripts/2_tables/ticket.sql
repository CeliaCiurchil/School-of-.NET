USE FlightBookingDb;
GO

PRINT 'Creating dbo.Ticket table...';

CREATE TABLE dbo.Ticket(
    Id BIGINT IDENTITY(1,1) NOT NULL,
    BookingId BIGINT NOT NULL,
    FlightScheduleId INT NOT NULL,
    FareClass NVARCHAR(2) NOT NULL,
    BasePrice DECIMAL(10,2) NOT NULL,
    Taxes DECIMAL(10,2) NOT NULL,
    TotalPrice DECIMAL(10,2) NOT NULL,
    Currency NCHAR(3) NOT NULL,
    IsRefundable BIT NOT NULL CONSTRAINT DF_Ticket_IsRefundable DEFAULT(0),
    SeatNumber NVARCHAR(4) NOT NULL,
    PassengerFullName NVARCHAR(120) NOT NULL,
    PassengerEmail NVARCHAR(120) NOT NULL,

    CONSTRAINT PK_Ticket PRIMARY KEY(Id)
);
GO
