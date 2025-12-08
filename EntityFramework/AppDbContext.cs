using AutoReview.Classes;
using AutoReview.Pages;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutoReview.EntityFramework
{
    public class AppDbContext : DbContext
    {
        public DbSet<Owner> Owners { get; set; }
        public DbSet<Car> Car { get; set; }
        public DbSet<Classes.Manufacturer> Manufacturer { get; set; }
        public DbSet<Classes.Engine> Engine { get; set; }
        public DbSet<Classes.Equipment> Equipment { get; set; }
        public AppDbContext() => Database.EnsureCreated();

        public string connectionPath;
        public AppDbContext(string connectionPath)
        {
            this.connectionPath = connectionPath;
            Database.EnsureCreated();
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseMySql(connectionPath, ServerVersion.AutoDetect(connectionPath));
            //optionsBuilder.UseSqlServer(connectionPath);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Owner>(entity =>
            {
                entity.ToTable("Owners");
                entity.HasKey(e => e.Id_owner);
                entity.Property(e => e.Id_owner)
                    .HasColumnName("id_owner");
                entity.Property(e => e.Fio)
                    .IsRequired()
                    .HasMaxLength(100)
                    .HasColumnName("fio");
                entity.Property(e => e.Owner_Email)
                    .IsRequired()
                    .HasMaxLength(100)
                    .HasColumnName("owner_email");
                entity.HasIndex(e => e.Owner_Email).IsUnique();
                entity.Property(e => e.Phone_number)
                    .HasMaxLength(20)
                    .HasColumnName("phone_number");
            });

            modelBuilder.Entity<Classes.Manufacturer>(entity =>
            {
                entity.ToTable("Manufacturer");
                entity.HasKey(e => e.Id_Manufacturer);
                entity.Property(e => e.Id_Manufacturer)
                    .HasColumnName("id_manufacturer");
                entity.Property(e => e.Title_Brand)
                    .IsRequired()
                    .HasMaxLength(50)
                    .HasColumnName("title_brand");
                entity.HasIndex(e => e.Title_Brand).IsUnique();
                entity.Property(e => e.Country_Brand)
                    .IsRequired()
                    .HasMaxLength(50)
                    .HasColumnName("country_brand");
                entity.Property(e => e.Owner_Email)
                    .IsRequired()
                    .HasMaxLength(100)
                    .HasColumnName("owner_email");

                entity.HasOne(m => m.Owner)
                      .WithMany(o => o.Manufacturers)
                      .HasForeignKey(m => m.Owner_Email)
                      .HasPrincipalKey(o => o.Owner_Email)
                      .OnDelete(DeleteBehavior.Restrict);
            });

            modelBuilder.Entity<Classes.Engine>(entity =>
            {
                entity.ToTable("Engine");
                entity.HasKey(e => e.Id_Engine);
                entity.Property(e => e.Id_Engine)
                    .HasColumnName("id_engine");
                entity.Property(e => e.Type_Engine)
                    .IsRequired()
                    .HasMaxLength(50)
                    .HasColumnName("type_engine");
                entity.Property(e => e.Capacity_Engine)
                    .HasColumnType("decimal(4,1)")
                    .HasColumnName("capacity_engine");
                entity.Property(e => e.Power_Engine)
                    .IsRequired()
                    .HasColumnName("power_engine");
            });

            modelBuilder.Entity<Car>(entity =>
            {
                entity.ToTable("Car");
                entity.HasKey(e => e.Id_Car);
                entity.Property(e => e.Id_Car)
                    .HasColumnName("id_car");
                entity.Property(e => e.Model_Car)
                    .IsRequired()
                    .HasMaxLength(100)
                    .HasColumnName("model_car");
                entity.Property(e => e.Body_Type)
                    .IsRequired()
                    .HasMaxLength(50)
                    .HasColumnName("body_type");
                entity.Property(e => e.Price_Car)
                    .HasColumnType("decimal(12,2)")
                    .HasColumnName("price_car");
                entity.Property(e => e.Year_Release)
                    .IsRequired()
                    .HasColumnName("year_release");

                entity.Property(e => e.Manufacturer_Id)
                    .HasColumnName("id_manufacturer");
                entity.Property(e => e.Engine_Id)
                    .HasColumnName("id_engine");

                entity.HasOne(c => c.Manufacturer)
                      .WithMany(m => m.Cars)
                      .HasForeignKey(c => c.Manufacturer_Id)
                      .OnDelete(DeleteBehavior.Restrict);

                entity.HasOne(c => c.Engine)
                      .WithMany(e => e.Cars)
                      .HasForeignKey(c => c.Engine_Id)
                      .OnDelete(DeleteBehavior.Restrict);
            });

            modelBuilder.Entity<Classes.Equipment>(entity =>
            {
                entity.ToTable("Equipment");
                entity.HasKey(e => e.Id_Equipment);
                entity.Property(e => e.Id_Equipment)
                    .HasColumnName("id_equipment");
                entity.Property(e => e.Title_Equipment)
                    .IsRequired()
                    .HasMaxLength(100)
                    .HasColumnName("title_equipment");
                entity.Property(e => e.Equipment_Level)
                    .IsRequired()
                    .HasMaxLength(50)
                    .HasColumnName("equipment_level");
                entity.Property(e => e.Car_Id)
                    .HasColumnName("id_car");

                entity.HasOne(e => e.Car)
                      .WithMany(c => c.Equipments)
                      .HasForeignKey(e => e.Car_Id)
                      .OnDelete(DeleteBehavior.Cascade);
            });
        }
    }
}