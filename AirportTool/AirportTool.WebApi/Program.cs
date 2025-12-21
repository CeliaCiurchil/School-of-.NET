using AirportTool.Application.Contracts;
using AirportTool.Application.Mappers;
using AirportTool.Application.Services;
using AirportTool.Application.Validators.Flight;
using FluentValidation;
using FluentValidation.AspNetCore;
using AirportTool.Infrastructure.Mappers;
using AirportTool.Infrastructure.Persistence;
using AirportTool.Infrastructure.Repositories;
using AirportTool.WebApi.Middleware;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

var connectionString = builder.Configuration.GetConnectionString("FlightBookingDbConnectionString");

builder.Services.AddDbContext<FlightBookingDbContext>(options => {
    options.UseSqlServer(connectionString);

    if (builder.Environment.IsDevelopment())
    {
        options.EnableSensitiveDataLogging();
        options.LogTo(Console.WriteLine, LogLevel.Information);
    }
});

//AutoMapper Configurations
builder.Services.AddAutoMapper(ctx =>
{

}, typeof(DomainDtoMapping), typeof(DomainEntityMapping));

// Add services to the container.

builder.Services.AddScoped<IFlightRepository, FlightRepository>();
builder.Services.AddScoped<IFlightScheduleRepository, FlightScheduleRepository>();

builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();

builder.Services.AddScoped<IFlightService, FlightService>();
builder.Services.AddScoped<IFlightScheduleService, FlightScheduleService>();

builder.Services.AddControllers();

builder.Services.AddFluentValidationAutoValidation();
builder.Services.AddValidatorsFromAssemblyContaining<BaseFlightDtoValidator>();

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseMiddleware<ExceptionHandlingMiddleware>();

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
