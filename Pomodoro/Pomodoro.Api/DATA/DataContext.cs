using Microsoft.EntityFrameworkCore;
using Pomodoro.Shared.Entities;

namespace Pomodoro.API.DATA
{
    public class DataContext : DbContext
    {
        // Constructor que pasa las opciones al constructor base
        public DataContext(DbContextOptions<DataContext> options) : base(options) { }

        // DbSet para cada entidad (tablas)
        public DbSet<Proyecto> Proyectos { get; set; }
        public DbSet<Tarea> Tareas { get; set; }
        public DbSet<SesionPomodoro> SesionesPomodoro { get; set; }
        public DbSet<TecnicaEstudio> TecnicasEstudio { get; set; }
        public DbSet<Recompensa> Recompensas { get; set; }
        public DbSet<HistorialSesion> HistorialSesiones { get; set; }

        // Método OnModelCreating para configurar relaciones y restricciones
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Relación entre Proyecto y Tarea (con cascada)
            modelBuilder.Entity<Tarea>()
                .HasOne(t => t.Proyecto)
                .WithMany(p => p.Tareas)
                .HasForeignKey(t => t.ProyectoId)
                .OnDelete(DeleteBehavior.Cascade);

            // Relación entre Proyecto y SesionPomodoro (con cascada)
            modelBuilder.Entity<SesionPomodoro>()
                .HasOne(s => s.Proyecto)
                .WithMany(p => p.SesionesPomodoro)
                .HasForeignKey(s => s.ProyectoId)
                .OnDelete(DeleteBehavior.Cascade);

            // Relación entre Proyecto y Recompensa (con cascada)
            modelBuilder.Entity<Recompensa>()
                .HasOne(r => r.Proyecto)
                .WithMany(p => p.Recompensas)
                .HasForeignKey(r => r.ProyectoId)
                .OnDelete(DeleteBehavior.Cascade);

            // Relación entre Proyecto y HistorialSesion (con cascada)
            modelBuilder.Entity<HistorialSesion>()
                .HasOne(h => h.Proyecto)
                .WithMany(p => p.HistorialSesiones)
                .HasForeignKey(h => h.ProyectoId)
                .OnDelete(DeleteBehavior.Cascade);

            // Relación entre Tarea y SesionPomodoro (sin eliminación en cascada para evitar múltiples rutas de cascada)
            modelBuilder.Entity<SesionPomodoro>()
                .HasOne(s => s.Tarea)
                .WithMany(t => t.SesionesPomodoro)
                .HasForeignKey(s => s.TareaId)
                .OnDelete(DeleteBehavior.NoAction);

            // Relación entre HistorialSesion y SesionPomodoro (sin eliminación en cascada para evitar múltiples rutas de cascada)
            modelBuilder.Entity<HistorialSesion>()
                .HasOne(h => h.SesionPomodoro)
                .WithMany()
                .HasForeignKey(h => h.SesionId)
                .OnDelete(DeleteBehavior.NoAction);

            // Relación entre SesionPomodoro y TecnicasEstudio (muchos a muchos)
            modelBuilder.Entity<SesionPomodoro>()
                .HasMany(s => s.TecnicasEstudio)
                .WithMany(t => t.SesionesPomodoro);

            // Índices únicos para evitar duplicados en nombres
            modelBuilder.Entity<TecnicaEstudio>()
                .HasIndex(t => t.Nombre)
                .IsUnique();

            modelBuilder.Entity<Recompensa>()
                .HasIndex(r => r.Nombre)
                .IsUnique();

            modelBuilder.Entity<Proyecto>()
                .HasIndex(p => p.Nombre)
                .IsUnique();
        }
    }
}
