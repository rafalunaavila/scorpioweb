using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace scorpioweb.Models
{
    public partial class penas2Context : DbContext
    {
        public virtual DbSet<Firmas> Firmas { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseMySql("server=10.11.118.60;port=3306;user=mandamientos;password=3j3cuc10n;database=penas2");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Firmas>(entity =>
            {
                entity.HasKey(e => e.Idfirmas);

                entity.ToTable("firmas");

                entity.Property(e => e.Idfirmas)
                    .HasColumnName("idfirmas")
                    .HasColumnType("int(11)")
                    .ValueGeneratedNever();

                entity.Property(e => e.Fecha)
                    .HasColumnName("fecha")
                    .HasColumnType("datetime");

                entity.Property(e => e.Foja)
                    .HasColumnName("foja")
                    .HasMaxLength(45);

                entity.Property(e => e.Libro)
                    .HasColumnName("libro")
                    .HasMaxLength(45);

                entity.Property(e => e.Nombre)
                    .HasColumnName("nombre")
                    .HasMaxLength(200);
            });
        }
    }
}
