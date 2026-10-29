
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;

namespace Endpoints;

public static class AppointmentEndpoints
{
    public static void MapAppointmentEndpoints(this WebApplication app)
    {
        // GET: obtener todas las citas
        app.MapGet("/api/appointments", async (IAppointmentService service) =>
        {
            return await service.GetAllAppointmentsAsync();
        })
        .WithName("GetAppointments");

        // GET: obtener citas con precio superior a 100 €
        app.MapGet("/api/appointments/expensive", async (IAppointmentService service) =>
        {
            return await service.GetExpensiveAppointmentsAsync();
        })
        .WithName("GetExpensiveAppointments");

        // GET: obtener una cita por su ID
        app.MapGet("/api/appointments/{id:int}", async (
            int id,
            IAppointmentService service) =>
        {
            var appointment = await service.GetAppointmentByIdAsync(id);

            return appointment is null
                ? Results.NotFound()
                : Results.Ok(appointment);
        })
        .WithName("GetAppointmentById");

        // GET: obtener los tratamientos disponibles
        app.MapGet("/api/treatments", async (IAppointmentService service) =>
        {
            return await service.GetTreatmentsAsync();
        })
        .WithName("GetTreatments");

        // POST: crear una nueva cita
        app.MapPost("/api/appointments", async (
            CreateAppointmentDto newAppointmentDto,
            IAppointmentService service) =>
        {
            var result = await service.CreateAppointmentAsync(newAppointmentDto);

            if (result.Errors.Count > 0)
            {
                return Results.BadRequest(result.Errors);
            }

            return Results.Created(
                $"/api/appointments/{result.Appointment!.Id}",
                result.Appointment
            );
        })
        .WithName("CreateAppointment");

        // PUT: actualizar una cita existente
        app.MapPut("/api/appointments/{id:int}", async (
            int id,
            UpdateAppointmentDto updatedAppointmentDto,
            IAppointmentService service) =>
        {
            var result = await service.UpdateAppointmentAsync(id, updatedAppointmentDto);

            if (result.NotFound)
            {
                return Results.NotFound();
            }

            if (result.Errors.Count > 0)
            {
                return Results.BadRequest(result.Errors);
            }

            return Results.Ok(result.Appointment);
        })
        .WithName("UpdateAppointment");

        // DELETE: eliminar una cita por su ID
        app.MapDelete("/api/appointments/{id:int}", async (
            int id,
            IAppointmentService service) =>
        {
            var deleted = await service.DeleteAppointmentAsync(id);

            return deleted
                ? Results.NoContent()
                : Results.NotFound();
        })
        .WithName("DeleteAppointment");
    }
}
