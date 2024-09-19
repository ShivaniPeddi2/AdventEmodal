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
                Password = "admin_password", // Plain text password (not secure)
                Email = "admin@example.com",
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow,
                IsAdmin = true // Admin role
            }
        );

        
        modelBuilder.Entity<Appointment>()
            .HasOne(a => a.TruckCompany)
            .WithMany(tc => tc.Appointments)
            .HasForeignKey(a => a.CompanyId)
            .OnDelete(DeleteBehavior.Restrict);  // Disable cascade delete

        modelBuilder.Entity<Appointment>()
            .HasOne(a => a.Terminal)
            .WithMany(t => t.Appointments)
            .HasForeignKey(a => a.TerminalId)
            .OnDelete(DeleteBehavior.Restrict);  // Disable cascade delete

        modelBuilder.Entity<Appointment>()
            .HasOne(a => a.Driver)
            .WithMany(d => d.Appointments)
            .HasForeignKey(a => a.DriverId)
            .OnDelete(DeleteBehavior.Restrict);  // Disable cascade delete

        modelBuilder.Entity<Driver>()
            .HasOne(d => d.TruckCompany)
            .WithMany(tc => tc.Drivers)
            .HasForeignKey(d => d.TruckCompanyId)
            .OnDelete(DeleteBehavior.Restrict);  // Disable cascade delete

        modelBuilder.Entity<Container>()
            .HasOne(c => c.TruckCompany)
            .WithMany(tc => tc.Containers)
            .HasForeignKey(c => c.CompanyId)
            .OnDelete(DeleteBehavior.Restrict);  // Disable cascade delete

        modelBuilder.Entity<Appointment>()
        .Property(a => a.StartDate)
        .HasColumnName("StartDate")
        .HasColumnType("datetime2");  // Ensure this matches your database schema
    
    modelBuilder.Entity<Appointment>()
        .Property(a => a.CreatedAt)
        .HasColumnName("CreatedAt")
        .HasColumnType("datetime2");
    
    modelBuilder.Entity<Appointment>()
        .Property(a => a.CheckIn)
        .HasColumnName("CheckIn")
        .HasColumnType("datetime2");

    }
}
