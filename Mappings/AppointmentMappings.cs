using System.Linq.Expressions;


public static class AppointmentMappings
{
    public static  Expression<Func<Appointment, AppointmentResponseDto>> ToResponseDto
    {
        get{
        return appointment => new AppointmentResponseDto
        {
            Id = appointment.Id,
            PatientName = appointment.PatientName,
            Treatment = appointment.Treatment,
            Date = appointment.Date
        };
        }
    }

    public static AppointmentResponseDto ToResponseDtoObject(Appointment appointment)
    {
    // Tu código aquí
        return new AppointmentResponseDto
        {
            Id = appointment.Id,
            PatientName = appointment.PatientName,
            Treatment = appointment.Treatment,
            Date = appointment.Date
        };
    }
  
}