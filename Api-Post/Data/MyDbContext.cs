using Microsoft.EntityFrameworkCore;
using Api_Usuarios.Models;

namespace Api_Usuarios.Data
{
    public class MyDbContext : DbContext
    {
        public MyDbContext(DbContextOptions<MyDbContext> options) : base(options)
        {
        }

        public DbSet<Cuenta> Cuenta { get; set; }
        public DbSet<Interactuan> Interactuan { get; set; }
        public DbSet<Instrumentos> Instrumentos { get; set; } // Agregado el DbSet para Instrumento

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Configuración de la tabla 'Cuenta'
            modelBuilder.Entity<Cuenta>(entity =>
            {
                entity.ToTable("cuenta");
                entity.HasKey(e => e.ID);
            });

            // Configuración de la tabla 'Interactuan'
            modelBuilder.Entity<Interactuan>(entity =>
            {
                entity.ToTable("interactuan");

                // Configuración de la clave primaria compuesta
                entity.HasKey(e => new { e.IDdeEmisor, e.IDdeReceptor, e.Tipo });

                // Relaciones con la tabla 'Cuenta' para las propiedades de navegación
                entity.HasOne(i => i.Emisor)
                    .WithMany()
                    .HasForeignKey(i => i.IDdeEmisor)
                    .OnDelete(DeleteBehavior.Restrict);

                entity.HasOne(i => i.Receptor)
                    .WithMany()
                    .HasForeignKey(i => i.IDdeReceptor)
                    .OnDelete(DeleteBehavior.Restrict);
            });

            // Configuración de la tabla 'Instrumentos'
            modelBuilder.Entity<Instrumentos>(entity =>
            {
                entity.ToTable("Instrumentos");

                // Configuración de la clave primaria compuesta
                entity.HasKey(e => new { e.IDdeCuenta, e.Instrumento });

                // Relaciones con la tabla 'Cuenta'
                entity.HasOne(i => i.Cuenta)
                    .WithMany()
                    .HasForeignKey(i => i.IDdeCuenta);
            });
        }

    }
}
