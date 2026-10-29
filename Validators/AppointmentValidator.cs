public static class AppointmentValidator
{
    public static bool Validate(Appointment appointment, out List<string> errors)
    {
        errors = new List<string>();

        if (string.IsNullOrWhiteSpace(appointment.PatientName))
        {
            errors.Add("El nombre del paciente es obligatorio.");
        }

        if (string.IsNullOrWhiteSpace(appointment.Treatment))
        {
            errors.Add("El tratamiento es obligatorio.");
        }

        if (appointment.Date < DateOnly.FromDateTime(DateTime.Today))
        {
            errors.Add("La fecha de la cita no puede ser en el pasado.");
        }

        if (appointment.Price < 0)
        {
            errors.Add("El precio debe ser mayor o igual que cero.");
        }

        return errors.Count == 0;
    }
}