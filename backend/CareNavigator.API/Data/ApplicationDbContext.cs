using CareNavigator.API.Models;
using Microsoft.EntityFrameworkCore;

namespace CareNavigator.API.Data;

public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(
        DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
    }

    public DbSet<User> Users => Set<User>();

    public DbSet<Patient> Patients => Set<Patient>();

    public DbSet<Provider> Providers => Set<Provider>();

    public DbSet<Appointment> Appointments => Set<Appointment>();

    public DbSet<MedicalReport> MedicalReports => Set<MedicalReport>();

    public DbSet<Prescription> Prescriptions => Set<Prescription>();

    public DbSet<CarePlan> CarePlans => Set<CarePlan>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<Patient>()
            .HasOne(p => p.User)
            .WithMany()
            .HasForeignKey(p => p.UserId);

        modelBuilder.Entity<Provider>()
            .HasOne(p => p.User)
            .WithMany()
            .HasForeignKey(p => p.UserId);

        modelBuilder.Entity<Appointment>()
            .HasOne(a => a.Patient)
            .WithMany()
            .HasForeignKey(a => a.PatientId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<Appointment>()
            .HasOne(a => a.Provider)
            .WithMany()
            .HasForeignKey(a => a.ProviderId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<MedicalReport>()
            .HasOne(m => m.Patient)
            .WithMany()
            .HasForeignKey(m => m.PatientId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<Prescription>()
            .HasOne(p => p.Patient)
            .WithMany()
            .HasForeignKey(p => p.PatientId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<Prescription>()
            .HasOne(p => p.Provider)
            .WithMany()
            .HasForeignKey(p => p.ProviderId)
            .OnDelete(DeleteBehavior.Restrict);
        
        modelBuilder.Entity<CarePlan>()
            .HasOne(c => c.Patient)
            .WithMany()
            .HasForeignKey(c => c.PatientId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<CarePlan>()
            .HasOne(c => c.Provider)
            .WithMany()
            .HasForeignKey(c => c.ProviderId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}
    