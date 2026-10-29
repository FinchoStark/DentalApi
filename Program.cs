using Endpoints;


using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<DentalApiContext>(options =>
    options.UseMySql(
        builder.Configuration.GetConnectionString("DentalApi"),
        ServerVersion.AutoDetect(
            builder.Configuration.GetConnectionString("DentalApi")
        )
    )
);

builder.Services.AddScoped<IAppointmentService, AppointmentService>();
// Configuración de servicios
builder.Services.AddOpenApi();
builder.Services.AddValidation();

var app = builder.Build();

// Documentación OpenAPI en desarrollo
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

//app.UseHttpsRedirection();

// Datos de ejemplo: citas de la clínica dental


app.MapAppointmentEndpoints();



app.Run();


