USE FlightBookingDb;
GO

-- FOREIGN KEYS

-- Aircraft ? Airline   
ALTER TABLE dbo.Aircraft
ADD CONSTRAINT FK_Aircraft_Airline
    FOREIGN KEY (AirlineId)
    REFERENCES dbo.Airline(Id);
GO

-- Flight ? Airline   
ALTER TABLE dbo.Flight
ADD CONSTRAINT FK_Flight_Airline
    FOREIGN KEY (AirlineId)
    REFERENCES dbo.Airline(Id)
    ON DELETE NO ACTION;  
GO

-- Airport ? Address
ALTER TABLE dbo.Airport
ADD CONSTRAINT FK_Airport_Address
    FOREIGN KEY (AddressId)
    REFERENCES dbo.Address(Id)
    ON DELETE NO ACTION;   
GO

-- Gate ? Airport
ALTER TABLE dbo.Gate
ADD CONSTRAINT FK_Gate_Airport
    FOREIGN KEY (AirportId)
    REFERENCES dbo.Airport(Id)
    ON DELETE CASCADE;     
GO

-- Flight ? Airport (origin)
ALTER TABLE dbo.Flight
ADD CONSTRAINT FK_Flight_OriginAirport
    FOREIGN KEY (OriginAirportId)
    REFERENCES dbo.Airport(Id)
    ON DELETE NO ACTION;  
GO

-- Flight ? Airport (destination)
ALTER TABLE dbo.Flight
ADD CONSTRAINT FK_Flight_DestinationAirport
    FOREIGN KEY (DestinationAirportId)
    REFERENCES dbo.Airport(Id)
    ON DELETE NO ACTION;
GO

-- Flight ? Aircraft (default aircraft)
ALTER TABLE dbo.Flight
ADD CONSTRAINT FK_Flight_DefaultAircraft
    FOREIGN KEY (DefaultAircraftId)
    REFERENCES dbo.Aircraft(Id)
    ON DELETE SET NULL;   
GO

-- FlightSchedule ? Aircraft (assigned aircraft)
ALTER TABLE dbo.FlightSchedule
ADD CONSTRAINT FK_FlightSchedule_AssignedAircraft
    FOREIGN KEY (AssignedAircraftId)
    REFERENCES dbo.Aircraft(Id)
    ON DELETE SET NULL;    
GO

-- FlightSchedule ? Flight 
ALTER TABLE dbo.FlightSchedule
ADD CONSTRAINT FK_FlightSchedule_Flight
    FOREIGN KEY (FlightId)
    REFERENCES dbo.Flight(Id)
    ON DELETE CASCADE;     
GO

-- FlightSchedule ? Gate 
ALTER TABLE dbo.FlightSchedule
ADD CONSTRAINT FK_FlightSchedule_Gate
    FOREIGN KEY (GateId)
    REFERENCES dbo.Gate(Id)
    ON DELETE SET NULL;   
GO

-- FlightSchedule ? FlightStatus 
ALTER TABLE dbo.FlightSchedule
ADD CONSTRAINT FK_FlightSchedule_Status
    FOREIGN KEY (StatusId)
    REFERENCES dbo.FlightStatus(Id)
    ON DELETE NO ACTION;   
GO

-- Booking ? User 
--ALTER TABLE dbo.Booking
--ADD CONSTRAINT FK_Booking_User
--    FOREIGN KEY (UserId)
--    REFERENCES dbo.[User](Id)
--    ON DELETE NO ACTION;   
--GO

-- Booking ? BookingStatus 
ALTER TABLE dbo.Booking
ADD CONSTRAINT FK_Booking_BookingStatus
    FOREIGN KEY (BookingStatusId)
    REFERENCES dbo.BookingStatus(Id)
    ON DELETE NO ACTION;   
GO

-- Ticket ? Booking 
ALTER TABLE dbo.Ticket
ADD CONSTRAINT FK_Ticket_Booking
    FOREIGN KEY (BookingId)
    REFERENCES dbo.Booking(Id)
    ON DELETE CASCADE;     
GO

-- Ticket ? FlightSchedule 
ALTER TABLE dbo.Ticket
ADD CONSTRAINT FK_Ticket_FlightSchedule
    FOREIGN KEY (FlightScheduleId)
    REFERENCES dbo.FlightSchedule(Id)
    ON DELETE NO ACTION;   
GO

-- UNIQUE
ALTER TABLE dbo.Airline
ADD CONSTRAINT UQ_Airline_IATACode UNIQUE (IATACode);
GO

ALTER TABLE dbo.Aircraft
ADD CONSTRAINT UQ_Aircraft_TailNumber UNIQUE(TailNumber);
GO

ALTER TABLE dbo.Airport
ADD CONSTRAINT UQ_Airport_IATACode UNIQUE(IATACode);
GO

ALTER TABLE dbo.Gate
ADD CONSTRAINT UQ_Gate_AirportId_Code UNIQUE(AirportId, Code);
GO

ALTER TABLE dbo.[User]
ADD CONSTRAINT UQ_User_Email UNIQUE (Email);
GO

ALTER TABLE dbo.Booking
ADD CONSTRAINT UQ_Booking_ConfirmationCode UNIQUE(ConfirmationCode);
GO

ALTER TABLE dbo.Booking
ADD CONSTRAINT CK_Booking_Quantity_Positive
    CHECK(Quantity >= 0);
GO

--CHECK
ALTER TABLE dbo.Aircraft
ADD CONSTRAINT CK_Aircraft_SeatCapacity_Positive
    CHECK (SeatCapacity > 0);
GO

ALTER TABLE dbo.Flight
ADD CONSTRAINT CK_Flight_Origin_Not_Destination
    CHECK (OriginAirportId <> DestinationAirportId);
GO

ALTER TABLE dbo.FlightSchedule
ADD CONSTRAINT CK_FlightSchedule_ArrivalAfterDeparture
    CHECK(ScheduledArrivalUtc > ScheduledDepartureUtc);
GO

ALTER TABLE dbo.Ticket
ADD CONSTRAINT CK_Ticket_BasePrice_NonNegative
    CHECK(BasePrice >= 0);
GO

ALTER TABLE dbo.Ticket
ADD CONSTRAINT CK_Ticket_Taxes_NonNegative
    CHECK(Taxes >= 0);
GO

ALTER TABLE dbo.Ticket
ADD CONSTRAINT CK_Ticket_TotalPrice_NonNegative
    CHECK(TotalPrice >= 0);
GO

ALTER TABLE dbo.Ticket
ADD CONSTRAINT CK_Ticket_TotalPrice_Sum
    CHECK(TotalPrice = BasePrice + Taxes);
GO

