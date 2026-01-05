-- create_database.sql
USE master;
GO

IF DB_ID('FlightBookingDb') IS NOT NULL
BEGIN 
    PRINT 'Dropping existing Database';

    ALTER DATABASE FlightBookingDb SET SINGLE_USER WITH ROLLBACK IMMEDIATE;

    DROP DATABASE FlightBookingDb;

END
GO

PRINT 'Creating new FlightBookingDb Database...';
CREATE DATABASE FlightBookingDb;
GO
