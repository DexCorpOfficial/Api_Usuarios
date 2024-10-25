using Microsoft.EntityFrameworkCore;
using Api_Post.Models;

namespace Api_Post.Data
{
    public class MyDbContext : DbContext
    {
        public MyDbContext(DbContextOptions<MyDbContext> options) : base(options)
        {
        }

        public DbSet<Cuenta> Cuenta { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Configuración adicional para la tabla 'cuenta' si es necesario
            modelBuilder.Entity<Cuenta>(entity =>
            {
                entity.ToTable("cuenta"); // Nombre de la tabla en la base de datos
                entity.HasKey(e => e.Id); // Llave primaria
            });
        }
    }
}
