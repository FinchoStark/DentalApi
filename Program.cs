
var builder = WebApplication.CreateBuilder(args);

// Configuración de servicios
builder.Services.AddOpenApi();

var app = builder.Build();

// Documentación OpenAPI en desarrollo
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

//app.UseHttpsRedirection();

// Datos de ejemplo: citas de la clínica dental
// Más adelante sustituiremos esta lista por una base de datos MySQL.
var appointments = new List<Appointment>
{
    new()
    {
        Id = 1,
        PatientName = "Ana",
        Treatment = "Limpieza",
        Date = DateOnly.FromDateTime(DateTime.Today.AddDays(1)),
        Price = 60m
    },
    new()
    {
        Id = 2,
        PatientName = "Luis",
        Treatment = "Empaste",
        Date = DateOnly.FromDateTime(DateTime.Today.AddDays(2)),
        Price = 90m
    },
    new()
    {
        Id = 3,
        PatientName = "Marta",
        Treatment = "Ortodoncia",
        Date = DateOnly.FromDateTime(DateTime.Today.AddDays(3)),
        Price = 180m
    },
    new()
    {
        Id = 4,
        PatientName = "Carlos",
        Treatment = "Implante",
        Date = DateOnly.FromDateTime(DateTime.Today.AddDays(5)),
        Price = 850m
    }
};

// GET: todas las citas
app.MapGet("/api/appointments", () =>
{
    return appointments;
})
.WithName("GetAppointments");

// GET: citas con precio superior a 100 €
app.MapGet("/api/appointments/expensive", () =>
{
    return appointments
        .Where(appointment => appointment.Price > 100m)
        .ToList();
})
.WithName("GetExpensiveAppointments");

// GET: una cita por su ID
app.MapGet("/api/appointments/{id:int}", (int id) =>
{
    var appointment = appointments
        .FirstOrDefault(appointment => appointment.Id == id);

    if (appointment is null)
    {
        return Results.NotFound();
    }

    return Results.Ok(appointment);
})
.WithName("GetAppointmentById");

// GET: tratamientos disponibles en los datos de ejemplo
app.MapGet("/api/treatments", () =>
{
    return appointments
        .Select(appointment => appointment.Treatment)
        .Distinct()
        .OrderBy(treatment => treatment)
        .ToList();
})
.WithName("GetTreatments");

// POST: crear una nueva cita
app.MapPost("/api/appointments", (Appointment newAppointment) =>
{
    newAppointment.Id = appointments.Max(a => a.Id) + 1;
    appointments.Add(newAppointment);
    return Results.Created($"/api/appointments/{newAppointment.Id}", newAppointment);
}).WithName("CreateAppointment");

// PUT: actualizar una cita existente
app.MapPut("/api/appointments/{id:int}", (int id, Appointment updatedAppointment) =>
{
    var existingAppointment = appointments.FirstOrDefault(a => a.Id == id);
    if (existingAppointment is null)
    {
        return Results.NotFound();
    }

    existingAppointment.PatientName = updatedAppointment.PatientName;
    existingAppointment.Treatment = updatedAppointment.Treatment;
    existingAppointment.Date = updatedAppointment.Date;
    existingAppointment.Price = updatedAppointment.Price;

    return Results.NoContent();
}).WithName("UpdateAppointment");

// DELETE: eliminar una cita por su ID
app.MapDelete("/api/appointments/{id:int}", (int id) =>
{
    var appointmentToDelete = appointments.FirstOrDefault(a => a.Id == id);
    if (appointmentToDelete is null)
    {
        return Results.NotFound();
    }

    appointments.Remove(appointmentToDelete);
    return Results.NoContent();
}).WithName("DeleteAppointment");

app.Run();

// Modelo de datos de una cita dental
public class Appointment
{
    public int Id { get; set; }
    public string PatientName { get; set; } = "";
    public string Treatment { get; set; } = "";
    public DateOnly Date { get; set; }
    public decimal Price { get; set; }
}
