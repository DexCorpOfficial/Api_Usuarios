using Microsoft.EntityFrameworkCore;
using Api_Usuarios.Models;

namespace Api_Post.Data
{
    public class MyDbContext : DbContext
    {
        public MyDbContext(DbContextOptions<MyDbContext> options) : base(options)
        {
        }

        public DbSet<Cuenta> Cuenta { get; set; }
        public DbSet<Interactuan> Interactuan { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Configuración de la tabla 'Cuenta'
            modelBuilder.Entity<Cuenta>(entity =>
            {
                entity.ToTable("cuenta");
                entity.HasKey(e => e.ID);
            });
 
            modelBuilder.Entity<Interactuan>(entity =>
            {
                entity.ToTable("interactuan");

                // Configuración de la clave primaria compuesta
                entity.HasKey(e => new { e.IDdeEmisor, e.IDdeReceptor, e.Tipo });

                // Relaciones con la tabla 'Cuenta'
                entity.HasOne<Cuenta>()
                    .WithMany()
                    .HasForeignKey(i => i.IDdeEmisor)
                    .OnDelete(DeleteBehavior.Restrict);

                entity.HasOne<Cuenta>()
                    .WithMany()
                    .HasForeignKey(i => i.IDdeReceptor)
                    .OnDelete(DeleteBehavior.Restrict);
            });
        }
    }
}
