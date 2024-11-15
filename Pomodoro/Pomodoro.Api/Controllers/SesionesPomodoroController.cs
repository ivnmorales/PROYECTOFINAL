using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Pomodoro.API.DATA;
using Pomodoro.Shared.Entities;
using Pomodoro.Shared.Dtos;

namespace Pomodoro.API.Controllers
{
    [ApiController]
    [Route("/api/sesionesPomodoro")] // Ruta base del controlador para "Sesiones Pomodoro"
    public class SesionesPomodoroController : ControllerBase
    {
        private readonly DataContext _context; // Contexto de base de datos

        public SesionesPomodoroController(DataContext context) // Constructor con inyección de dependencias
        {
            _context = context;
        }

        // Obtiene todas las sesiones Pomodoro
        [HttpGet]
        public async Task<ActionResult<IEnumerable<SesionPomodoroDto>>> GetSesionesPomodoro()
        {
            var sesiones = await _context.SesionesPomodoro
                .Include(s => s.Proyecto) // Carga el proyecto asociado
                .Include(s => s.Tarea) // Carga la tarea asociada
                .Select(s => new SesionPomodoroDto
                {
                    Id = s.Id,
                    Duracion = s.Duracion,
                    FechaInicio = s.FechaInicio,
                    FechaFin = s.FechaFin,
                    Estado = s.Estado,
                    ProyectoId = s.ProyectoId,
                    TareaId = s.TareaId
                })
                .ToListAsync();

            return Ok(sesiones);
        }

        // Obtiene una sesión Pomodoro por id
        [HttpGet("{id}")]
        public async Task<ActionResult<SesionPomodoroDto>> GetSesionPomodoro(int id)
        {
            var sesion = await _context.SesionesPomodoro
                .Include(s => s.Proyecto) // Carga el proyecto asociado
                .Include(s => s.Tarea) // Carga la tarea asociada
                .FirstOrDefaultAsync(s => s.Id == id); // Busca la sesión por id

            if (sesion == null) return NotFound(); // Si no existe, retorna 404

            var sesionDto = new SesionPomodoroDto
            {
                Id = sesion.Id,
                Duracion = sesion.Duracion,
                FechaInicio = sesion.FechaInicio,
                FechaFin = sesion.FechaFin,
                Estado = sesion.Estado,
                ProyectoId = sesion.ProyectoId,
                TareaId = sesion.TareaId
            };

            return Ok(sesionDto); // Retorna la sesión encontrada
        }

        // Crea una nueva sesión Pomodoro
        [HttpPost]
        public async Task<ActionResult> PostSesionPomodoro(CrearSesionPomodoroDto sesionDto)
        {
            var proyecto = await _context.Proyectos.FindAsync(sesionDto.ProyectoId);
            if (proyecto == null)
            {
                return BadRequest("El proyecto especificado no existe.");
            }

            var tarea = await _context.Tareas.FindAsync(sesionDto.TareaId);
            if (tarea == null)
            {
                return BadRequest("La tarea especificada no existe.");
            }

            var sesion = new SesionPomodoro
            {
                Duracion = sesionDto.Duracion,
                FechaInicio = sesionDto.FechaInicio,
                FechaFin = sesionDto.FechaFin,
                Estado = sesionDto.Estado,
                ProyectoId = sesionDto.ProyectoId,
                TareaId = sesionDto.TareaId
            };

            _context.SesionesPomodoro.Add(sesion);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetSesionPomodoro), new { id = sesion.Id }, sesionDto);
        }

        // Actualiza una sesión Pomodoro existente
        [HttpPut("{id}")]
        public async Task<IActionResult> PutSesionPomodoro(int id, ActualizarSesionPomodoroDto sesionDto)
        {
            var sesion = await _context.SesionesPomodoro.FindAsync(id);
            if (sesion == null) return NotFound();

            sesion.Duracion = sesionDto.Duracion;
            sesion.FechaInicio = sesionDto.FechaInicio;
            sesion.FechaFin = sesionDto.FechaFin;
            sesion.Estado = sesionDto.Estado;

            _context.Entry(sesion).State = EntityState.Modified;
            await _context.SaveChangesAsync();

            return NoContent();
        }

        // Elimina una sesión Pomodoro por id
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteSesionPomodoro(int id)
        {
            var sesion = await _context.SesionesPomodoro.FindAsync(id);
            if (sesion == null) return NotFound();

            _context.SesionesPomodoro.Remove(sesion);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}
