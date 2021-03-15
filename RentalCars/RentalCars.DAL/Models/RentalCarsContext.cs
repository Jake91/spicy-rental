using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace Jake.RentalCars.DAL.Models
{
    public partial class RentalCarsContext : DbContext
    {
        public RentalCarsContext()
        {
        }

        public RentalCarsContext(DbContextOptions<RentalCarsContext> options)
            : base(options)
        {
        }

        public virtual DbSet<CarCategory> CarCategory { get; set; }
        public virtual DbSet<Customer> Customer { get; set; }
        public virtual DbSet<Rental> Rental { get; set; }
        public virtual DbSet<RentalCar> RentalCar { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlite("DataSource=C:\\github\\spicy-rental\\RentalCars\\RentalCars.DAL\\DB\\RentalCars.db"); // todo extract to setting...
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<CarCategory>(entity =>
            {
                entity.HasKey(e => e.IdCarCategory);

                entity.HasIndex(e => e.Name)
                    .IsUnique();

                entity.Property(e => e.IdCarCategory)
                    .HasColumnName("Id_CarCategory")
                    .ValueGeneratedOnAdd();

                entity.Property(e => e.DayPriceMultiplier).HasColumnType("DOUBLE");

                entity.Property(e => e.KilometerPriceMultiplier).HasColumnType("DOUBLE");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasColumnType("NVARCHAR(100)");
            });

            modelBuilder.Entity<Customer>(entity =>
            {
                entity.HasKey(e => e.IdCustomer);

                entity.HasIndex(e => e.Email)
                    .HasName("customer_email");

                entity.Property(e => e.IdCustomer)
                    .HasColumnName("Id_Customer")
                    .ValueGeneratedOnAdd();

                entity.Property(e => e.DateOfBirth)
                    .IsRequired()
                    .HasColumnType("DATETIME");

                entity.Property(e => e.Email)
                    .IsRequired()
                    .HasColumnType("NVARCHAR(350)");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasColumnType("NVARCHAR(100)");
            });

            modelBuilder.Entity<Rental>(entity =>
            {
                entity.HasKey(e => e.IdRental);

                entity.HasIndex(e => e.BookingNumber)
                    .HasName("rental_bookingnumber");

                entity.Property(e => e.IdRental)
                    .HasColumnName("Id_Rental")
                    .ValueGeneratedOnAdd();

                entity.Property(e => e.BookingNumber)
                    .IsRequired()
                    .HasColumnType("NVARCHAR(36)");

                entity.Property(e => e.From)
                    .IsRequired()
                    .HasColumnType("DATETIME");

                entity.Property(e => e.PaidPrice).HasColumnType("DOUBLE");

                entity.Property(e => e.ReturnedAt).HasColumnType("DATETIME");

                entity.Property(e => e.To)
                    .IsRequired()
                    .HasColumnType("DATETIME");

                entity.HasOne(d => d.Customer)
                    .WithMany(p => p.Rental)
                    .HasForeignKey(d => d.CustomerId)
                    .OnDelete(DeleteBehavior.ClientSetNull);

                entity.HasOne(d => d.RentalCar)
                    .WithMany(p => p.Rental)
                    .HasForeignKey(d => d.RentalCarId)
                    .OnDelete(DeleteBehavior.ClientSetNull);
            });

            modelBuilder.Entity<RentalCar>(entity =>
            {
                entity.HasKey(e => e.IdRentalCar);

                entity.Property(e => e.IdRentalCar)
                    .HasColumnName("Id_RentalCar")
                    .ValueGeneratedOnAdd();

                entity.HasOne(d => d.CarCategory)
                    .WithMany(p => p.RentalCar)
                    .HasForeignKey(d => d.CarCategoryId)
                    .OnDelete(DeleteBehavior.ClientSetNull);
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
