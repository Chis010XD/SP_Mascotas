using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace masco.Models
{
    public partial class MASCOTASContext : DbContext
    {
        public MASCOTASContext()
        {
        }

        public MASCOTASContext(DbContextOptions<MASCOTASContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Perro> Perros { get; set; } = null!;

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            IConfigurationRoot configuration = new ConfigurationBuilder()
                    .SetBasePath(Directory.GetCurrentDirectory())
                    .AddJsonFile("appsettings.json")
                    .Build();


            var connectionString = configuration.GetConnectionString("con");
            optionsBuilder.UseSqlServer(connectionString);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Perro>(entity =>
            {
                entity.HasKey(e => e.IdMasc);

                entity.Property(e => e.IdMasc)
                    .HasColumnType("numeric(18, 0)")
                    .ValueGeneratedOnAdd()
                    .HasColumnName("idMasc");

                entity.Property(e => e.Color).HasMaxLength(20);

                entity.Property(e => e.Nombre).HasMaxLength(20);

                entity.Property(e => e.Raza).HasMaxLength(20);

                entity.Property(e => e.Tamanio)
                    .HasColumnType("decimal(18, 0)")
                    .HasColumnName("tamanio");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
