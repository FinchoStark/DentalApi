public class AppointmentResponseDto
{
    public int Id { get; set; }
    public string PatientName { get; set; } = "";
    public string Treatment { get; set; } = "";
    public DateOnly Date { get; set; }
    
}