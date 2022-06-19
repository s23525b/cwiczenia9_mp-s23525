using cwiczenia9_mp_s23525.Models;
using Microsoft.EntityFrameworkCore;
using System;

namespace cwiczenia9_mp_s23525.Models
{
    public class MainDbContext : DbContext
    {
        public MainDbContext(DbContextOptions options) : base(options)
        {
        }

        protected MainDbContext()
        {
        }

        public DbSet<Patient> Patients { get; set; }
        public DbSet<Doctor> Doctors { get; set; }
        public DbSet<Prescription> Prescriptions { get; set; }
        public DbSet<Medicament> Medicaments { get; set; }
        public DbSet<PrescriptionMedicament> PrescriptionMedicaments { get; set; }
        public DbSet<User> Users { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Patient>(p =>
            {
                p.HasKey(e => e.IdPatient);
                p.Property(e => e.FirstName).IsRequired().HasMaxLength(100);
                p.Property(e => e.LastName).IsRequired().HasMaxLength(100);
                p.Property(e => e.BirthDate).IsRequired();
                p.HasData(
                    new Patient { IdPatient=1, FirstName = "Jan", LastName = "Kowalski", BirthDate = DateTime.Parse("2000-01-15")},
                    new Patient { IdPatient = 2, FirstName = "Grzegorz", LastName = "Korosta", BirthDate = DateTime.Parse("1970-11-01") }
                    );
            });

            
            modelBuilder.Entity<Doctor>(p =>
            {
                p.HasKey(e => e.IdDoctor);
                p.Property(e => e.FirstName).IsRequired().HasMaxLength(100);
                p.Property(e => e.LastName).IsRequired().HasMaxLength(100);
                p.Property(e => e.Email).IsRequired().HasMaxLength(100);
                p.HasData(
                    new Doctor { IdDoctor = 1, FirstName = "Joanna", LastName = "Pszczółkowska", Email = "j.pszczola@gmail.com" },
                    new Doctor { IdDoctor = 2, FirstName = "Leonora", LastName = "Minaj", Email = "leonora.minaj@o2.pl" },
                    new Doctor { IdDoctor = 3, FirstName = "Paweł", LastName = "Starygnat", Email = "paweł.stary@leki.pl" }
                    );
            });

            
            modelBuilder.Entity<Prescription>(p =>
            {
                p.HasKey(e => e.IdPrescription);
                p.Property(e => e.Date).IsRequired();
                p.Property(e => e.DueDate).IsRequired();
                p.HasOne(e => e.Patient).WithMany(e => e.Prescriptions).HasForeignKey(e => e.IdPatient);
                p.HasOne(e => e.Doctor).WithMany(e => e.Prescriptions).HasForeignKey(e => e.IdDoctor);
                p.HasData(
                    new Prescription { IdPrescription = 1, Date = DateTime.Parse("2020-03-03"), DueDate = DateTime.Parse("2020-04-02"), IdDoctor = 1, IdPatient = 2},
                    new Prescription { IdPrescription = 2, Date = DateTime.Parse("2021-11-01"), DueDate = DateTime.Parse("2021-12-22"), IdDoctor = 2, IdPatient = 1 }
                    );
            });

            
            modelBuilder.Entity<Medicament>(p =>
            {
                p.HasKey(e => e.IdMedicament);
                p.Property(e => e.Name).IsRequired().HasMaxLength(100);
                p.Property(e => e.Description).IsRequired().HasMaxLength(100);
                p.Property(e => e.Type).IsRequired().HasMaxLength(100);
                p.HasData(
                    new Medicament { IdMedicament = 1, Name = "Viagra", Description = "Blue, round", Type = "pill"},
                    new Medicament { IdMedicament = 2, Name = "Vexodozaonin", Description = "Red/White, square", Type = "pill" }
                    );
            });
            

            modelBuilder.Entity<PrescriptionMedicament>(p =>
            {
                p.HasKey(k => new { k.IdMedicament, k.IdPrescription });
                p.Property(e => e.Dose).IsRequired();
                p.Property(e => e.Details).IsRequired().HasMaxLength(100);
                p.HasOne(e => e.Medicament).WithMany(e => e.PrescriptionMedicaments).HasForeignKey(e => e.IdMedicament);
                p.HasOne(e => e.Prescription).WithMany(e => e.PrescriptionMedicaments).HasForeignKey(e => e.IdPrescription);
                p.HasData(
                    new PrescriptionMedicament { IdMedicament = 1, IdPrescription = 1, Dose = 5, Details = "Yes"},
                    new PrescriptionMedicament { IdMedicament = 2, IdPrescription = 2, Dose = 1, Details = "Yes" }
                    );
                
            });

            modelBuilder.Entity<User>(p =>
            {
                p.HasKey(e => e.IdUser);
                p.Property(e => e.Password).IsRequired().HasMaxLength(16);
                p.Property(e => e.RefreshToken);
                p.Property(e => e.RefreshTokenExpiration);
            });
        }

    }
}
