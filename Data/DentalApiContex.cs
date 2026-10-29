
using Microsoft.EntityFrameworkCore;

public class DentalApiContext : DbContext
{
    public DentalApiContext(DbContextOptions<DentalApiContext> options)
        : base(options)
    {
    }

    public DbSet<Appointment> Appointments => Set<Appointment>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
{
    modelBuilder.Entity<Appointment>()
        .Property(a => a.Price)
        .HasPrecision(10, 2);
}
}
