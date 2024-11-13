using Microsoft.EntityFrameworkCore;
using Pomodoro.Shared.Entities;
using System.Threading.Tasks;

namespace Pomodoro.API.DATA
{
    public class SeedDb
    {
        private readonly DataContext _context;

        public SeedDb(DataContext  context)
        {
            _context = context;
        }

        public async Task SeedAsync()
        {
            await _context.Database.EnsureCreatedAsync();
            await CheckProyectosAsync();
            await CheckTareasAsync();
            await CheckTecnicasEstudioAsync();
            await CheckSesionesPomodoroAsync();
            await CheckRecompensasAsync();
            await CheckHistorialSesionesAsync();
        }

        private async Task CheckProyectosAsync()
        {
            if (!_context.Proyectos.Any(p => p.Nombre == "Proyecto de Matemáticas"))
            {
                _context.Proyectos.Add(new Proyecto
                {
                    Nombre = "Proyecto de Matemáticas",
                    Descripcion = "Estudiar conceptos avanzados de matemáticas.",
                    FechaCreacion = DateTime.Parse("2024-01-01T09:00:00")
                });
            }

            if (!_context.Proyectos.Any(p => p.Nombre == "Proyecto de Historia"))
            {
                _context.Proyectos.Add(new Proyecto
                {
                    Nombre = "Proyecto de Historia",
                    Descripcion = "Investigar y analizar la historia antigua.",
                    FechaCreacion = DateTime.Parse("2024-01-15T10:00:00")
                });
            }

            await _context.SaveChangesAsync();
        }

        private async Task CheckTareasAsync()
        {
            var proyecto = await _context.Proyectos.FirstOrDefaultAsync(p => p.Nombre == "Proyecto de Matemáticas");
            if (proyecto != null && !_context.Tareas.Any(t => t.Titulo == "Estudiar Algebra"))
            {
                _context.Tareas.Add(new Tarea
                {
                    Titulo = "Estudiar Algebra",
                    Descripcion = "Revisar conceptos básicos y avanzados de álgebra.",
                    FechaLimite = DateTime.Parse("2024-02-01T09:00:00"),
                    ProyectoId = proyecto.Id
                });
            }

            await _context.SaveChangesAsync();
        }

        private async Task CheckTecnicasEstudioAsync()
        {
            if (!_context.TecnicasEstudio.Any(t => t.Nombre == "Mapa Mental"))
            {
                _context.TecnicasEstudio.Add(new TecnicaEstudio
                {
                    Nombre = "Mapa Mental",
                    Descripcion = "Organización visual de conceptos clave.",
                    Beneficios = "Mejora la comprensión y retención de información."
                });
            }

            if (!_context.TecnicasEstudio.Any(t => t.Nombre == "Método Pomodoro"))
            {
                _context.TecnicasEstudio.Add(new TecnicaEstudio
                {
                    Nombre = "Método Pomodoro",
                    Descripcion = "Estudio en bloques de tiempo con descansos.",
                    Beneficios = "Incrementa el enfoque y productividad."
                });
            }

            await _context.SaveChangesAsync();
        }

        private async Task CheckSesionesPomodoroAsync()
        {
            var tarea = await _context.Tareas.FirstOrDefaultAsync(t => t.Titulo == "Estudiar Algebra");
            if (tarea != null && !_context.SesionesPomodoro.Any(s => s.FechaInicio == DateTime.Parse("2024-02-01T10:00:00")))
            {
                _context.SesionesPomodoro.Add(new SesionPomodoro
                {
                    FechaInicio = DateTime.Parse("2024-02-01T10:00:00"),
                    FechaFin = DateTime.Parse("2024-02-01T10:25:00"),
                    Duracion = 25,
                    Estado = "Completado",
                    TareaId = tarea.Id,
                    ProyectoId = tarea.ProyectoId
                });
            }

            await _context.SaveChangesAsync();
        }

        private async Task CheckRecompensasAsync()
        {
            var proyecto = await _context.Proyectos.FirstOrDefaultAsync(p => p.Nombre == "Proyecto de Matemáticas");
            if (proyecto != null && !_context.Recompensas.Any(r => r.Nombre == "Día libre"))
            {
                _context.Recompensas.Add(new Recompensa
                {
                    Nombre = "Día libre",
                    Descripcion = "Un día sin tareas como recompensa.",
                    ProyectoId = proyecto.Id
                });
            }

            await _context.SaveChangesAsync();
        }

        private async Task CheckHistorialSesionesAsync()
        {
            var sesionPomodoro = await _context.SesionesPomodoro.FirstOrDefaultAsync(s => s.Estado == "Completado");
            if (sesionPomodoro != null && !_context.HistorialSesiones.Any(h => h.SesionId == sesionPomodoro.Id))
            {
                _context.HistorialSesiones.Add(new HistorialSesion
                {
                    Fecha = DateTime.Parse("2024-02-01T10:00:00"),
                    Duracion = sesionPomodoro.Duracion,
                    ProyectoId = sesionPomodoro.ProyectoId,
                    SesionId = sesionPomodoro.Id
                });
            }

            await _context.SaveChangesAsync();
        }
    }
}
