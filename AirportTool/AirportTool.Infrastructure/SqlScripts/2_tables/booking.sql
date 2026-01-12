USE FlightBookingDb;
GO

PRINT 'Creating dbo.Booking table...';

CREATE TABLE dbo.Booking(
    Id BIGINT IDENTITY(1,1) NOT NULL,
    UserId NVARCHAR(450) NOT NULL,
    BookingStatusId INT NOT NULL,
    CreatedUtc DATETIME2 NOT NULL CONSTRAINT DF_Booking_CreatedUtc DEFAULT(SYSUTCDATETIME()),
    ConfirmationCode NVARCHAR(8) NOT NULL,
    Quantity INT NOT NULL,

    CONSTRAINT PK_Booking PRIMARY KEY(Id),
);
GO
