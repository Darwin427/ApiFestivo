using ApiFestivo.Dominio;
using Microsoft.EntityFrameworkCore;

namespace ApiFestivo.InfraestructuraPersistencia.Contexto
{
        public class APIFestivosContext : DbContext
        {
            public APIFestivosContext(DbContextOptions<APIFestivosContext> options) : base(options)
            {
            }
            public DbSet<Festivo> Festivos { get; set; }
            public DbSet<Tipo> Tipo { get; set; }

            protected override void OnModelCreating(ModelBuilder modelBuilder)
            {
                modelBuilder.Entity<Festivo>(festivo =>
                {
                    festivo.HasKey(s => s.id);
                    festivo.HasIndex(s => s.Nombre).IsUnique();
                    festivo.Property(s => s.Dia).IsRequired();
                    festivo.Property(s => s.Mes).IsRequired();
                    festivo.Property(s => s.DiasPascua).IsRequired();
                    festivo.Property(s => s.IdTipo).IsRequired();
                });

                modelBuilder.Entity<Tipo>(tipo =>
                {
                    tipo.HasKey(s => s.Id);
                    tipo.HasIndex(s => s.tipo).IsUnique();
                });

                modelBuilder.Entity<Festivo>()
                    .HasOne(c => c.tipo)
                    .WithMany()
                    .HasForeignKey(c => c.IdTipo);
            }

        }
}
