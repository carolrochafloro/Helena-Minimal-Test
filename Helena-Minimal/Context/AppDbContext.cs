using Helena_Minimal.Models;
using Microsoft.EntityFrameworkCore;

namespace Helena_Minimal.Context;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

    public DbSet<Medication> Medications { get; set; }
    public DbSet<Doctor> Doctors { get; set; }
    public DbSet<Times> Times { get; set; }
}
