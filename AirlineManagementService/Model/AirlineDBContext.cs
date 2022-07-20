using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace AirlineManagementService.Model
{
    public partial class AirlineDBContext : DbContext
    {
        public AirlineDBContext()
        {
        }

        public AirlineDBContext(DbContextOptions<AirlineDBContext> options)
            : base(options)
        {
        }

        public virtual DbSet<AirlineDetail> AirlineDetails { get; set; } = null!;
        public virtual DbSet<FlightDetail> FlightDetails { get; set; } = null!;
        public virtual DbSet<FlightScheduleDetail> FlightScheduleDetails { get; set; } = null!;

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
                optionsBuilder.UseSqlServer("Data Source=(localdb)\\MSSQLLocalDB;Initial Catalog=AirlineDB;Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<AirlineDetail>(entity =>
            {
                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.AirlineCode).HasMaxLength(50);

                entity.Property(e => e.AirlineName).HasMaxLength(200);

                entity.Property(e => e.CreatedOn).HasColumnType("datetime");

                entity.Property(e => e.LastupdatedOn).HasColumnType("datetime");
            });

            modelBuilder.Entity<FlightDetail>(entity =>
            {
                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.CreatedOn).HasColumnType("datetime");

                entity.Property(e => e.FlightCode).HasMaxLength(50);

                entity.Property(e => e.LastupdatedOn).HasColumnType("datetime");
            });

            modelBuilder.Entity<FlightScheduleDetail>(entity =>
            {
                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.CreatedOn).HasColumnType("datetime");

                entity.Property(e => e.Destination).HasMaxLength(200);

                entity.Property(e => e.FromDate).HasColumnType("datetime");

                entity.Property(e => e.LastUpdatedOn).HasColumnType("datetime");

                entity.Property(e => e.Price).HasColumnType("decimal(18, 2)");

                entity.Property(e => e.ScheduledDays).HasMaxLength(500);

                entity.Property(e => e.Source).HasMaxLength(200);

                entity.Property(e => e.ToDate).HasColumnType("datetime");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
