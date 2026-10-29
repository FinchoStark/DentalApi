using System.ComponentModel.DataAnnotations;

public class CreateAppointmentDto
{
    [Required]
    public string PatientName { get; set; } = "";
    [Required]
    public string Treatment { get; set; } = "";
    public DateOnly Date { get; set; }
    [Range(0, double.MaxValue, ErrorMessage = "El precio debe ser mayor o igual que cero.")]
    public decimal Price { get; set; }
}