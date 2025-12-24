using System;
using System.Collections.Generic;
using AirportTool.Infrastructure.Persistence.Entities;
using Microsoft.EntityFrameworkCore;

namespace AirportTool.Infrastructure.Persistence;

public partial class FlightBookingDbContext : DbContext
{
    public FlightBookingDbContext()
    {
    }

    public FlightBookingDbContext(DbContextOptions<FlightBookingDbContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Address> Addresses { get; set; }

    public virtual DbSet<Aircraft> Aircraft { get; set; }

    public virtual DbSet<Airline> Airlines { get; set; }

    public virtual DbSet<Airport> Airports { get; set; }

    public virtual DbSet<Booking> Bookings { get; set; }

    public virtual DbSet<BookingStatus> BookingStatuses { get; set; }

    public virtual DbSet<Flight> Flights { get; set; }

    public virtual DbSet<FlightSchedule> FlightSchedules { get; set; }

    public virtual DbSet<FlightStatus> FlightStatuses { get; set; }

    public virtual DbSet<Gate> Gates { get; set; }

    public virtual DbSet<Ticket> Tickets { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Address>(entity =>
        {
            entity.ToTable("Address");

            entity.Property(e => e.City).HasMaxLength(80);
            entity.Property(e => e.Country).HasMaxLength(80);
            entity.Property(e => e.Street).HasMaxLength(200);
        });

        modelBuilder.Entity<Aircraft>(entity =>
        {
            entity.HasIndex(e => e.TailNumber, "UQ_Aircraft_TailNumber").IsUnique();

            entity.Property(e => e.Model).HasMaxLength(60);
            entity.Property(e => e.TailNumber).HasMaxLength(10);

            entity.HasOne(d => d.Airline).WithMany(p => p.Aircraft)
                .HasForeignKey(d => d.AirlineId)
                .HasConstraintName("FK_Aircraft_Airline");
        });

        modelBuilder.Entity<Airline>(entity =>
        {
            entity.ToTable("Airline");

            entity.HasIndex(e => e.Iatacode, "UQ_Airline_IATACode").IsUnique();

            entity.Property(e => e.Iatacode)
                .HasMaxLength(3)
                .IsFixedLength()
                .HasColumnName("IATACode");
            entity.Property(e => e.Name).HasMaxLength(100);
        });

        modelBuilder.Entity<Airport>(entity =>
        {
            entity.ToTable("Airport");

            entity.HasIndex(e => e.Iatacode, "UQ_Airport_IATACode").IsUnique();

            entity.Property(e => e.Iatacode)
                .HasMaxLength(3)
                .IsFixedLength()
                .HasColumnName("IATACode");
            entity.Property(e => e.Name).HasMaxLength(120);
            entity.Property(e => e.TimeZone).HasMaxLength(64);

            entity.HasOne(d => d.Address).WithMany(p => p.Airports)
                .HasForeignKey(d => d.AddressId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Airport_Address");
        });

        modelBuilder.Entity<Booking>(entity =>
        {
            entity.ToTable("Booking");

            entity.HasIndex(e => e.ConfirmationCode, "UQ_Booking_ConfirmationCode").IsUnique();

            entity.Property(e => e.ConfirmationCode).HasMaxLength(8);
            entity.Property(e => e.CreatedUtc).HasDefaultValueSql("(sysutcdatetime())");

            entity.Property(e => e.UserId).HasMaxLength(450);

            entity.HasOne(d => d.BookingStatus).WithMany(p => p.Bookings)
                .HasForeignKey(d => d.BookingStatusId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Booking_BookingStatus");
        });

        modelBuilder.Entity<BookingStatus>(entity =>
        {
            entity.ToTable("BookingStatus");

            entity.Property(e => e.Status).HasMaxLength(40);
        });

        modelBuilder.Entity<Flight>(entity =>
        {
            entity.ToTable("Flight");

            entity.HasIndex(e => new { e.AirlineId, e.FlightNumber }, "IX_Flight_AirlineId_FlightNumber");

            entity.HasIndex(e => e.IsActive, "IX_Flight_IsActive_ActiveOnly").HasFilter("([IsActive]=(1))");

            entity.HasIndex(e => new { e.OriginAirportId, e.DestinationAirportId }, "IX_Flight_OriginAirportId_DestinationAirportId");

            entity.Property(e => e.FlightNumber).HasMaxLength(8);
            entity.Property(e => e.IsActive).HasDefaultValue(true);

            entity.HasOne(d => d.Airline).WithMany(p => p.Flights)
                .HasForeignKey(d => d.AirlineId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Flight_Airline");

            entity.HasOne(d => d.DefaultAircraft).WithMany(p => p.Flights)
                .HasForeignKey(d => d.DefaultAircraftId)
                .OnDelete(DeleteBehavior.SetNull)
                .HasConstraintName("FK_Flight_DefaultAircraft");

            entity.HasOne(d => d.DestinationAirport).WithMany(p => p.FlightDestinationAirports)
                .HasForeignKey(d => d.DestinationAirportId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Flight_DestinationAirport");

            entity.HasOne(d => d.OriginAirport).WithMany(p => p.FlightOriginAirports)
                .HasForeignKey(d => d.OriginAirportId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Flight_OriginAirport");
        });

        modelBuilder.Entity<FlightSchedule>(entity =>
        {
            entity.ToTable("FlightSchedule");

            entity.HasIndex(e => new { e.FlightId, e.ScheduledDepartureUtc }, "IX_FlightSchedule_FlightId_ScheduledDepartureUtc");

            entity.HasOne(d => d.AssignedAircraft).WithMany(p => p.FlightSchedules)
                .HasForeignKey(d => d.AssignedAircraftId)
                .OnDelete(DeleteBehavior.SetNull)
                .HasConstraintName("FK_FlightSchedule_AssignedAircraft");

            entity.HasOne(d => d.Flight).WithMany(p => p.FlightSchedules)
                .HasForeignKey(d => d.FlightId)
                .HasConstraintName("FK_FlightSchedule_Flight");

            entity.HasOne(d => d.Gate).WithMany(p => p.FlightSchedules)
                .HasForeignKey(d => d.GateId)
                .OnDelete(DeleteBehavior.SetNull)
                .HasConstraintName("FK_FlightSchedule_Gate");

            entity.HasOne(d => d.Status).WithMany(p => p.FlightSchedules)
                .HasForeignKey(d => d.StatusId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_FlightSchedule_Status");
        });

        modelBuilder.Entity<FlightStatus>(entity =>
        {
            entity.ToTable("FlightStatus");

            entity.Property(e => e.Status).HasMaxLength(40);
        });

        modelBuilder.Entity<Gate>(entity =>
        {
            entity.ToTable("Gate");

            entity.HasIndex(e => new { e.AirportId, e.Code }, "UQ_Gate_AirportId_Code").IsUnique();

            entity.Property(e => e.Code).HasMaxLength(10);

            entity.HasOne(d => d.Airport).WithMany(p => p.Gates)
                .HasForeignKey(d => d.AirportId)
                .HasConstraintName("FK_Gate_Airport");
        });

        modelBuilder.Entity<Ticket>(entity =>
        {
            entity.ToTable("Ticket");

            entity.HasIndex(e => new { e.FlightScheduleId, e.FareClass }, "IX_Ticket_FlightScheduleId_FareClass");

            entity.Property(e => e.BasePrice).HasColumnType("decimal(10, 2)");
            entity.Property(e => e.Currency)
                .HasMaxLength(3)
                .IsFixedLength();
            entity.Property(e => e.FareClass).HasMaxLength(2);
            entity.Property(e => e.PassengerEmail).HasMaxLength(120);
            entity.Property(e => e.PassengerFullName).HasMaxLength(120);
            entity.Property(e => e.SeatNumber).HasMaxLength(4);
            entity.Property(e => e.Taxes).HasColumnType("decimal(10, 2)");
            entity.Property(e => e.TotalPrice).HasColumnType("decimal(10, 2)");

            entity.HasOne(d => d.Booking).WithMany(p => p.Tickets)
                .HasForeignKey(d => d.BookingId)
                .HasConstraintName("FK_Ticket_Booking");

            entity.HasOne(d => d.FlightSchedule).WithMany(p => p.Tickets)
                .HasForeignKey(d => d.FlightScheduleId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Ticket_FlightSchedule");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}

