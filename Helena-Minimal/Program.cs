using Helena_Minimal.Context;
using Helena_Minimal.DTO;
using Helena_Minimal.Models;
using Helena_Minimal.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.EntityFrameworkCore;
using Npgsql;
using NpgsqlTypes;
using System.Linq;


var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(policy =>
    {
        policy.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod();
    });
});
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

builder.Services.AddDbContext<AppDbContext>(options =>
{
    options.UseNpgsql(connectionString);
});

builder.Services.AddScoped<DateTimeCreation>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors();

app.UseHttpsRedirection();

app.MapGet("/medications", async (AppDbContext context) =>
{

    var query = from medication in context.Medications
                join doctor in context.Doctors on medication.DoctorId equals doctor.Id
                join time in context.Times on medication.Id equals time.MedicationId
                group new { medication, doctor, time } by new
                {
                    medication.Id,
                    MedicationName = medication.Name,
                    medication.Lab,
                    medication.Type,
                    medication.Dosage,
                    medication.Notes,
                    medication.Start,
                    medication.End,
                    medication.FrequencyType,
                    medication.Recurrency,
                    DoctorName = doctor.Name,
                    doctor.Specialty,
                    medication.IndicatedFor
                } into g
                select new MedicationWithDoctor
                {
                    Id = g.Key.Id,
                    Name = g.Key.MedicationName,
                    Lab = g.Key.Lab,
                    Type = g.Key.Type,
                    Dosage = g.Key.Dosage,
                    Notes = g.Key.Notes,
                    Start = g.Key.Start,
                    End = g.Key.End,
                    FrequencyType = g.Key.FrequencyType,
                    Recurrency = g.Key.Recurrency,
                    DoctorName = g.Key.DoctorName,
                    DoctorSpecialty = g.Key.Specialty,
                    IndicatedFor = g.Key.IndicatedFor,
                    Times = g.Select(x => new TimeDTO
                    {
                        DateTime = x.time.DateTime,
                        IsTaken = x.time.IsTaken,
                    }).OrderBy(t => t.DateTime).ToList()
                };

    var result = await query.ToListAsync();

    return result;
});

// get by day
app.MapGet("/medications/{date}", async ([FromRoute] DateOnly date, AppDbContext context) =>
{


    var query = context.Medications
        .Where(m => context.Times.Any(t => t.MedicationId == m.Id && t.DateTime.Date == date.ToDateTime(TimeOnly.MinValue).ToUniversalTime().Date))
        .Select(medication => new DayMedicationDTO
        {
            MedicationId = medication.Id,
            Name = medication.Name,
            Notes = medication.Notes,
            Dosage = medication.Dosage,
            Times = context.Times
                        .Where(t => t.MedicationId == medication.Id && t.DateTime.Date == date.ToDateTime(TimeOnly.MinValue).ToUniversalTime().Date).OrderBy(t => t.DateTime)
                        .Select(t => new TimeDTO
                        {
                            Id = t.Id,
                            DateTime = DateTime.SpecifyKind(t.DateTime, DateTimeKind.Utc),  // Convertendo para UTC
                            IsTaken = t.IsTaken
                        }).ToList()
        });

    return await query.ToListAsync();

});

app.MapGet("/doctors", async (AppDbContext context) =>
{
    var doctors = await context.Doctors.Select(doctor => new Doctor
    {
        Id = doctor.Id,
        Name = doctor.Name,
        Specialty = doctor.Specialty

    }).ToListAsync();

    return doctors;
});

app.MapPost("/medications", async ([FromBody] NewMedDTO newMed, AppDbContext context, DateTimeCreation dateTimeService) =>
{
    var medication = new Medication
    {
        Id = Guid.NewGuid(),
        Name = newMed.Name,
        Lab = newMed.Lab,
        Type = newMed.Type,
        Dosage = newMed.Dosage,
        Notes = newMed.Notes,
        Img = newMed.Img,
        Start = DateOnly.Parse(newMed.Start),
        End = DateOnly.Parse(newMed.End),
        FrequencyType = (FrequencyType)newMed.FrequencyType,
        Recurrency = newMed.Recurrency,
        DoctorId = newMed.DoctorId,
    };

    List<NewTimeDTO> newMedTimes = newMed.Times;
    List<Times> timesToAdd = new List<Times>();

    switch (newMed.FrequencyType)
    {
        case 0:
            timesToAdd = dateTimeService.CreateDailyTimes(medication.Id, medication.Start, medication.End, newMedTimes);
            break;
        case 1:
            timesToAdd = dateTimeService.CreateWeeklyTimes(medication.Id, medication.Start, medication.End, newMedTimes);
            break;
        case 2:
            timesToAdd = dateTimeService.CreateMonthlyAndYearlyTimes(medication.Id, medication.Start, medication.End, newMedTimes);
            break;
        case 3:
            timesToAdd = dateTimeService.CreateMonthlyAndYearlyTimes(medication.Id, medication.Start, medication.End, newMedTimes);
            break;
    }

    context.Medications.Add(medication);

    foreach (var item in timesToAdd)
    {
        context.Times.Add(item);
    }

    await context.SaveChangesAsync();

    return Results.Created($"/medications/{medication.Id}", medication);
});

app.Run();

