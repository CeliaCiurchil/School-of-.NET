USE FlightBookingDb;
GO


CREATE NONCLUSTERED INDEX IX_Flight_AirlineId_FlightNumber
ON dbo.Flight (AirlineId, FlightNumber);
GO


CREATE NONCLUSTERED INDEX IX_Flight_IsActive_ActiveOnly
ON dbo.Flight (IsActive)
WHERE IsActive = 1;
GO


CREATE NONCLUSTERED INDEX IX_Flight_OriginAirportId_DestinationAirportId
ON dbo.Flight (OriginAirportId, DestinationAirportId);
GO


CREATE NONCLUSTERED INDEX IX_FlightSchedule_FlightId_ScheduledDepartureUtc
ON dbo.FlightSchedule (FlightId, ScheduledDepartureUtc);
GO


CREATE NONCLUSTERED INDEX IX_Ticket_FlightScheduleId_FareClass
ON dbo.Ticket (FlightScheduleId, FareClass);
GO
