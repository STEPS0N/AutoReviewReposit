using AutoReview.Classes;
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
        public DbSet<User> Users { get; set; }
        public DbSet<Car> Car { get; set; }
        public DbSet<Manufacturer> Manufacturer { get; set; }
        public DbSet<Engine> Engine { get; set; }
        public DbSet<Equipment> Equipment { get; set; }
        public DbSet<Feedback> Feedback { get; set; }
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

            modelBuilder.Entity<Manufacturer>(entity =>
            {
                entity.ToTable("Manufacturer");
                entity.HasKey(e => e.Id_Manufacturer);
                entity.Property(e => e.Id_Manufacturer)
                    .HasColumnName("id_manufacturer");
                entity.Property(e => e.Title_Brand)
                    .IsRequired()
                    .HasMaxLength(30)
                    .HasColumnName("title_brand");
                entity.HasIndex(e => e.Title_Brand).IsUnique();
                entity.Property(e => e.Country_Brand)
                    .IsRequired()
                    .HasMaxLength(50)
                    .HasColumnName("country_brand");
            });

            modelBuilder.Entity<Engine>(entity =>
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
                    .HasColumnType("decimal(3,1)")
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
                    .HasMaxLength(50)
                    .HasColumnName("model_car");
                entity.Property(e => e.Body_Type)
                    .IsRequired()
                    .HasMaxLength(50)
                    .HasColumnName("body_type");
                entity.Property(e => e.Price_Car)
                    .HasColumnType("decimal(10,2)")
                    .HasColumnName("price_car");
                entity.Property(e => e.Year_Release)
                    .IsRequired()
                    .HasColumnName("year_release");

                // Внешний ключ
                entity.Property(e => e.Manufacturer_Id)
                    .HasColumnName("id_manufacturer");
                entity.Property(e => e.Engine_Id)
                    .HasColumnName("id_engine");

                entity.HasOne(c => c.Manufacturer)
                      .WithMany(m => m.Cars)
                      .HasForeignKey(c => c.Manufacturer_Id)
                      .OnDelete(DeleteBehavior.Cascade);

                entity.HasOne(c => c.Engine)
                      .WithMany(e => e.Cars)
                      .HasForeignKey(c => c.Engine_Id);
            });

            modelBuilder.Entity<Equipment>(entity =>
            {
                entity.ToTable("Equipment");
                entity.HasKey(e => e.Id_Equipment);
                entity.Property(e => e.Id_Equipment)
                    .HasColumnName("id_equipment");
                entity.Property(e => e.Title_Equipment)
                    .IsRequired()
                    .HasMaxLength(50)
                    .HasColumnName("title_equipment");
                entity.Property(e => e.Equipment_Level)
                    .IsRequired()
                    .HasMaxLength(50)
                    .HasColumnName("equipment_level");

                // Внешний ключ
                entity.Property(e => e.Car_Id)
                    .HasColumnName("id_car");

                entity.HasOne(e => e.Car)
                      .WithMany(c => c.Equipments)
                      .HasForeignKey(e => e.Car_Id)
                      .OnDelete(DeleteBehavior.Cascade);
            });

            modelBuilder.Entity<User>(entity =>
            {
                entity.ToTable("Users");
                entity.HasKey(e => e.Id_user);
                entity.Property(e => e.Id_user)
                    .HasColumnName("id_user");
                entity.Property(e => e.Login)
                    .IsRequired()
                    .HasMaxLength(50)
                    .HasColumnName("login");
                entity.Property(e => e.Password)
                    .IsRequired()
                    .HasMaxLength(100)
                    .HasColumnName("password");
                entity.Property(e => e.Email_User)
                    .IsRequired()
                    .HasMaxLength(50)
                    .HasColumnName("email_user");
                entity.HasIndex(e => e.Email_User).IsUnique();
                entity.Property(e => e.Date_Registration)
                    .HasDefaultValueSql("GETDATE()")
                    .HasColumnName("date_registration");
            });

            modelBuilder.Entity<Feedback>(entity =>
            {
                entity.ToTable("Feedback");
                entity.HasKey(e => e.Id_Feedback);
                entity.Property(e => e.Id_Feedback)
                    .HasColumnName("id_feedback");
                entity.Property(e => e.Review_Feedback)
                    .IsRequired()
                    .HasColumnName("review_feedback");
                entity.Property(e => e.Rating_Feedback)
                    .IsRequired()
                    .HasColumnName("rating_feedback");
                entity.Property(e => e.Date_Feedback)
                    .HasDefaultValueSql("GETDATE()")
                    .HasColumnName("date_feedback");

                entity.Property(e => e.User_Id)
                    .HasColumnName("id_user");
                entity.Property(e => e.Car_Id)
                    .HasColumnName("id_car");

                // Внешний ключ
                entity.HasOne(f => f.User)
                      .WithMany(u => u.Feedbacks)
                      .HasForeignKey(f => f.User_Id)
                      .OnDelete(DeleteBehavior.Cascade);

                entity.HasOne(f => f.Car)
                      .WithMany(c => c.Feedbacks)
                      .HasForeignKey(f => f.Car_Id)
                      .OnDelete(DeleteBehavior.Cascade);

                entity.ToTable(tb => tb.HasCheckConstraint("CHK_Feedback_Rating", "rating_feedback BETWEEN 1 AND 5"));
            });
        }
    }
}