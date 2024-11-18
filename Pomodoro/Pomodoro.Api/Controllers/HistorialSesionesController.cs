using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Pomodoro.API.DATA;
using Pomodoro.Shared.Entities;
using Pomodoro.Shared.Dtos;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;

namespace Pomodoro.API.Controllers
{
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [ApiController]
    [Route("/api/historialSesiones")] // Ruta base del controlador para "HistorialSesiones"
    public class HistorialSesionesController : ControllerBase
    {
        private readonly DataContext _context; // Contexto de base de datos

        public HistorialSesionesController(DataContext context) // Constructor con inyección de dependencias
        {
            _context = context;
        }

       
        // Obtiene todos los historiales de sesiones
        [HttpGet]
        public async Task<ActionResult<IEnumerable<HistorialSesionDto>>> GetHistorialSesiones()
        {
            var historialSesiones = await _context.HistorialSesiones
                .Include(h => h.SesionPomodoro) // Carga la sesión Pomodoro asociada
                .Include(h => h.Proyecto) // Carga el proyecto asociado, si existe
                .Select(h => new HistorialSesionDto
                {
                    Id = h.Id,
                    Fecha = h.Fecha,
                    Duracion = h.Duracion,
                    ProyectoId = h.ProyectoId,
                    SesionId = h.SesionId
                })
                .ToListAsync();

            return Ok(historialSesiones);
        }

        // Obtiene un historial de sesión por id
        [HttpGet("{id}")]
        public async Task<ActionResult<HistorialSesionDto>> GetHistorialSesion(int id)
        {
            var historialSesion = await _context.HistorialSesiones
                .Include(h => h.SesionPomodoro) // Carga la sesión Pomodoro asociada
                .Include(h => h.Proyecto) // Carga el proyecto asociado, si existe
                .FirstOrDefaultAsync(h => h.Id == id); // Busca el historial por id

            if (historialSesion == null) return NotFound(); // Si no existe, retorna 404

            var historialSesionDto = new HistorialSesionDto
            {
                Id = historialSesion.Id,
                Fecha = historialSesion.Fecha,
                Duracion = historialSesion.Duracion,
                ProyectoId = historialSesion.ProyectoId,
                SesionId = historialSesion.SesionId
            };

            return Ok(historialSesionDto); // Retorna el historial de sesión encontrado
        }

        // Crea un nuevo historial de sesión
        [HttpPost]
        public async Task<ActionResult> PostHistorialSesion(CrearHistorialSesionDto historialSesionDto)
        {
            var sesionPomodoro = await _context.SesionesPomodoro.FindAsync(historialSesionDto.SesionId);
            if (sesionPomodoro == null)
            {
                return BadRequest("La sesión Pomodoro especificada no existe.");
            }

            var historialSesion = new HistorialSesion
            {
                Fecha = historialSesionDto.Fecha,
                Duracion = historialSesionDto.Duracion,
                ProyectoId = historialSesionDto.ProyectoId,
                SesionId = historialSesionDto.SesionId
            };

            _context.HistorialSesiones.Add(historialSesion);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetHistorialSesion), new { id = historialSesion.Id }, historialSesionDto);
        }

        // Actualiza un historial de sesión existente
        [HttpPut("{id}")]
        public async Task<IActionResult> PutHistorialSesion(int id, ActualizarHistorialSesionDto historialSesionDto)
        {
            var historialSesion = await _context.HistorialSesiones.FindAsync(id);
            if (historialSesion == null) return NotFound();

            historialSesion.Fecha = historialSesionDto.Fecha;
            historialSesion.Duracion = historialSesionDto.Duracion;

            _context.Entry(historialSesion).State = EntityState.Modified;
            await _context.SaveChangesAsync();

            return NoContent();
        }

        // Elimina un historial de sesión por id
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteHistorialSesion(int id)
        {
            var historialSesion = await _context.HistorialSesiones.FindAsync(id);
            if (historialSesion == null) return NotFound();

            _context.HistorialSesiones.Remove(historialSesion);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}
