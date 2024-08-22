using Helena_Minimal.Properties.Models;
using Npgsql;
using System.Text.Json;

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



var app = builder.Build();


var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors();

app.UseHttpsRedirection();

app.MapGet("/medications", async () =>
{
    await using var dataSource = NpgsqlDataSource.Create(connectionString);

    await using var cmd = dataSource.CreateCommand("SELECT * FROM medication");
    await using var reader = await cmd.ExecuteReaderAsync();

    var medications = new List<Medication>();

    while (await reader.ReadAsync())
    {
        var medication = new Medication
        {
            Id = reader.GetGuid(reader.GetOrdinal("Id")),
            Name = reader.GetString(reader.GetOrdinal("Name")),
            Lab = reader.GetString(reader.GetOrdinal("Lab")),
            Type = reader.GetString(reader.GetOrdinal("Type")),
            Dosage = reader.GetString(reader.GetOrdinal("Dosage")),
            Notes = reader.GetString(reader.GetOrdinal("Notes")),
            Img = reader.GetString(reader.GetOrdinal("Img")),
            Start = reader.GetFieldValue<DateOnly>(reader.GetOrdinal("Start")),
            End = reader.GetFieldValue<DateOnly>(reader.GetOrdinal("End")),
            FrequencyType = Enum.Parse<FrequencyType>(reader.GetString(reader.GetOrdinal("FrequencyType"))),
            Recurrency = reader.GetInt32(reader.GetOrdinal("Recurrency"))
        };

        medications.Add(medication);
    }

    //var json = JsonSerializer.Serialize(medications);
    return Results.Json(medications);
});

// get by day
app.MapGet("/medications/{date}", async (DateOnly date) =>
{

    Console.WriteLine("Acessado");
    await using var dataSource = NpgsqlDataSource.Create(connectionString);

    var query = @"
        SELECT t.*, m.name, m.dosage, m.notes
        FROM times t
        JOIN medication m ON t.medicationId = m.Id
        WHERE t.date_time::date = @Date";

    await using var cmd = dataSource.CreateCommand(query);
    cmd.Parameters.AddWithValue("Date", date);

    await using var reader = await cmd.ExecuteReaderAsync();

    var results = new List<object>();

    while (await reader.ReadAsync())
    {
        var time = new
        {
            Id = reader.GetGuid(reader.GetOrdinal("Id")),
            MedicationId = reader.GetGuid(reader.GetOrdinal("MedicationId")),
            Date = reader.GetFieldValue<DateTime>(reader.GetOrdinal("date_time")),
            Medication = new
            {
                Name = reader.GetString(reader.GetOrdinal("Name")),
                Dosage = reader.GetString(reader.GetOrdinal("Dosage")),
                Notes = reader.GetString(reader.GetOrdinal("Notes")),
            }
        };

        results.Add(time);
    }

    return Results.Json(results);
});


app.Run();

