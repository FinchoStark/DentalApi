
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

public class AppointmentService : IAppointmentService
{
    private readonly DentalApiContext db;

    public AppointmentService(DentalApiContext db)
    {
        this.db = db;
    }

    public async Task<List<AppointmentResponseDto>> GetAllAppointmentsAsync()
    {
        return await db.Appointments
            .Select(AppointmentMappings.ToResponseDto)
            .ToListAsync();
    }

    public async Task<AppointmentResponseDto?> GetAppointmentByIdAsync(int id)
    {
        return await db.Appointments
            .Where(appointment => appointment.Id == id)
            .Select(AppointmentMappings.ToResponseDto)
            .FirstOrDefaultAsync();
    }

    public async Task<List<AppointmentResponseDto>> GetExpensiveAppointmentsAsync()
    {
        return await db.Appointments
            .Where(appointment => appointment.Price > 100m)
            .Select(AppointmentMappings.ToResponseDto)
            .ToListAsync();
    }

    public async Task<List<string>> GetTreatmentsAsync()
    {
        return await db.Appointments
            .Select(appointment => appointment.Treatment)
            .Distinct()
            .OrderBy(treatment => treatment)
            .ToListAsync();
    }

    public async Task<(AppointmentResponseDto? Appointment, List<string> Errors)> CreateAppointmentAsync(
        CreateAppointmentDto newAppointmentDto)
    {
        var newAppointment = new Appointment
        {
            PatientName = newAppointmentDto.PatientName,
            Treatment = newAppointmentDto.Treatment,
            Date = newAppointmentDto.Date,
            Price = newAppointmentDto.Price
        };

        var errors = ValidateAppointment(newAppointment);

        if (errors.Count > 0)
            {
            return (null, errors);
        }

        db.Appointments.Add(newAppointment);
        await db.SaveChangesAsync();

        return (AppointmentMappings.ToResponseDtoObject(newAppointment), new List<string>());
    }

    public async Task<(AppointmentResponseDto? Appointment, List<string> Errors, bool NotFound)> UpdateAppointmentAsync(
        int id,
        UpdateAppointmentDto updatedAppointmentDto)
    {
        var existingAppointment = await db.Appointments
            .FirstOrDefaultAsync(appointment => appointment.Id == id);

        if (existingAppointment is null)
        {
            return (null, new List<string>(), true);
        }

        existingAppointment.PatientName = updatedAppointmentDto.PatientName;
        existingAppointment.Treatment = updatedAppointmentDto.Treatment;
        existingAppointment.Date = updatedAppointmentDto.Date;
        existingAppointment.Price = updatedAppointmentDto.Price;
        var errors = ValidateAppointment(existingAppointment);

        if (errors.Count > 0)
        {
            return (null, errors, false);
        }
        await db.SaveChangesAsync();

        return (AppointmentMappings.ToResponseDtoObject(existingAppointment), new List<string>(), false);
    }

    public async Task<bool> DeleteAppointmentAsync(int id)
    {
        var appointmentToDelete = await db.Appointments
            .FirstOrDefaultAsync(appointment => appointment.Id == id);

        if (appointmentToDelete is null)
        {
            return false;
        }

        db.Appointments.Remove(appointmentToDelete);
        await db.SaveChangesAsync();

        return true;
    }

    private static List<string> ValidateAppointment(Appointment appointment)
{
    AppointmentValidator.Validate(appointment, out List<string> errors);
    return errors;
}
}
