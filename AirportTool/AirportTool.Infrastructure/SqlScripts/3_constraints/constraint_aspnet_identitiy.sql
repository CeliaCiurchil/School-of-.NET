-- Clear dependent rows
  DELETE FROM dbo.Ticket;
  DELETE FROM dbo.Booking;

  -- Drop old FK to dbo.User
  ALTER TABLE dbo.Booking DROP CONSTRAINT FK_Booking_User;

  -- Change UserId to string
  ALTER TABLE dbo.Booking
      ALTER COLUMN UserId NVARCHAR(450) NOT NULL;

  -- Add FK to Identity
  ALTER TABLE dbo.Booking
      ADD CONSTRAINT FK_Booking_AspNetUsers
          FOREIGN KEY (UserId) REFERENCES dbo.AspNetUsers(Id);


