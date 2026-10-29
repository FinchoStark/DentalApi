// Modelo de datos de una cita dental
public class Appointment
{
    public int Id { get; set; }
    public string PatientName { get; set; } = "";
    public string Treatment { get; set; } = "";
    public DateOnly Date { get; set; }
    public decimal Price { get; set; }
}