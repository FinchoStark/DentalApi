
using System.Collections.Generic;
using System.Threading.Tasks;

public interface IAppointmentService
{
    Task<List<AppointmentResponseDto>> GetAllAppointmentsAsync();

    Task<AppointmentResponseDto?> GetAppointmentByIdAsync(int id);

    Task<List<AppointmentResponseDto>> GetExpensiveAppointmentsAsync();

    Task<List<string>> GetTreatmentsAsync();

    Task<(AppointmentResponseDto? Appointment, List<string> Errors)>
    CreateAppointmentAsync(CreateAppointmentDto appointment);

    Task<(AppointmentResponseDto? Appointment, List<string> Errors, bool NotFound)>
    UpdateAppointmentAsync(int id, UpdateAppointmentDto appointment);

    Task<bool> DeleteAppointmentAsync(int id);
}
