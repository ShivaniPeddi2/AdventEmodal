using Microsoft.EntityFrameworkCore;

public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }

    public DbSet<User> Users { get; set; }
    public DbSet<TruckCompany> TruckCompanies { get; set; }
    public DbSet<Driver> Drivers { get; set; }
    public DbSet<Terminal> Terminals { get; set; }
    public DbSet<Container> Containers { get; set; }
    public DbSet<Appointment> Appointments { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // Seed admin user with plain text password
        modelBuilder.Entity<User>().HasData(
            new User
            {
                UserId = 1,
                Username = "admin",
                Password = "password", // Plain text password (not secure)
                Email = "admin@example.com",
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow,
                IsAdmin = true // Admin role
            }
        );
    }
}
