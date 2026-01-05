USE FlightBookingDb;
GO

INSERT INTO dbo.FlightStatus(Status) VALUES
    ('Planned'),
    ('Boarding'),
    ('Departed'),
    ('Cancelled'),
    ('Delayed');
GO

INSERT INTO dbo.BookingStatus(Status)
VALUES
    ('Active'),
    ('Cancelled');
GO


INSERT INTO dbo.[Address](Country, City, Street)
VALUES
    ('Romania', 'Cluj-Napoca', 'Avram Iancu 1'),
    ('Romania', 'Bucharest', 'Henri Coandă 1');
GO


INSERT INTO dbo.Airline(IATACode, Name)
VALUES
    ('RO ', 'Romanian Air'),
    ('EN ', 'Endava Air');
GO


INSERT INTO dbo.Airport(IATACode, Name, TimeZone, AddressId)
VALUES
    ('CLJ', 'Cluj-Napoca International Airport', 'Europe/Bucharest', 1),
    ('OTP', 'Henri Coandă International Airport', 'Europe/Bucharest', 2);
GO

-- Gates for CLJ 
INSERT INTO dbo.Gate(AirportId, Code)
VALUES
    (1, 'A1'),
    (1, 'A2'),
    (1, 'A3');

-- Gates for OTP
INSERT INTO dbo.Gate(AirportId, Code)
VALUES
    (2, 'B1'),
    (2, 'B2'),
    (2, 'B3');
GO

INSERT INTO dbo.Aircraft(TailNumber, Model, SeatCapacity, AirlineId)
VALUES
    ('YR-ABC', 'Boeing 737-800', 180, 1);
GO

INSERT INTO dbo.Flight(
    AirlineId,
    FlightNumber,
    OriginAirportId,
    DestinationAirportId,
    DefaultAircraftId,
    IsActive
)
VALUES
    (1, 'RO1001', 1, 2, 1, 1),   
    (1, 'RO1002', 2, 1, 1, 1);   
GO


INSERT INTO dbo.FlightSchedule(
    FlightId,
    ScheduledDepartureUtc,
    ScheduledArrivalUtc,
    GateId,
    AssignedAircraftId,
    StatusId
)
VALUES
    (1, '2025-01-01T06:00:00', '2025-01-01T07:00:00', 1, 1, 1),
    (2, '2025-01-01T18:00:00', '2025-01-01T19:00:00', 4, 1, 1);
GO

INSERT INTO dbo.[User](Name, Email)
VALUES
    ('Demo User', 'demo.user@example.com');
GO


INSERT INTO dbo.Booking(
    UserId,
    BookingStatusId,
    CreatedUtc,
    ConfirmationCode,
    Quantity
)
VALUES
    (1, 1, SYSUTCDATETIME(), 'ABC123', 2);
GO


INSERT INTO dbo.Ticket(
    BookingId,
    FlightScheduleId,
    FareClass,
    BasePrice,
    Taxes,
    TotalPrice,
    Currency,
    IsRefundable,
    SeatNumber,
    PassengerFullName,
    PassengerEmail
)
VALUES
    (1, 1, 'Y', 100.00, 20.00, 120.00, 'EUR', 0, '12A',
     'John Doe', 'john.doe@example.com'),
    (1, 2, 'Y', 100.00, 20.00, 120.00, 'EUR', 0, '12B',
     'Jane Doe', 'jane.doe@example.com');
GO
