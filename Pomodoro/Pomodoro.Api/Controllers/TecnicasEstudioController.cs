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
    [Route("/api/tecnicas-estudio")] // Ruta base del controlador para "TecnicasEstudio"
    public class TecnicasEstudioController : ControllerBase
    {
        private readonly DataContext _context; // Contexto de base de datos

        public TecnicasEstudioController(DataContext context) // Constructor con inyección de dependencias
        {
            _context = context;
        }

        // Obtiene todas las técnicas de estudio, incluyendo las sesiones asociadas
        [HttpGet]
        public async Task<ActionResult<IEnumerable<TecnicaEstudioDto>>> GetTecnicasEstudio()
        {
            var tecnicas = await _context.TecnicasEstudio
                .Include(t => t.SesionesPomodoro)
                .Select(t => new TecnicaEstudioDto
                {
                    Id = t.Id,
                    Nombre = t.Nombre,
                    Descripcion = t.Descripcion,
                    Beneficios = t.Beneficios,
                    SesionesPomodoro = t.SesionesPomodoro.Select(s => new SesionPomodoroDto
                    {
                        Id = s.Id,
                        Duracion = s.Duracion,
                        FechaInicio = s.FechaInicio,
                        FechaFin = s.FechaFin,
                        Estado = s.Estado,
                        ProyectoId = s.ProyectoId,
                        TareaId = s.TareaId
                    }).ToList()
                })
                .ToListAsync();

            return Ok(tecnicas);
        }

        // Obtiene una técnica de estudio por id, incluyendo las sesiones asociadas
        [HttpGet("{id}")]
        public async Task<ActionResult<TecnicaEstudioDto>> GetTecnicaEstudio(int id)
        {
            var tecnica = await _context.TecnicasEstudio
                .Include(t => t.SesionesPomodoro)
                .FirstOrDefaultAsync(t => t.Id == id); // Busca la técnica por id

            if (tecnica == null) return NotFound(); // Si no existe, retorna 404

            var tecnicaDto = new TecnicaEstudioDto
            {
                Id = tecnica.Id,
                Nombre = tecnica.Nombre,
                Descripcion = tecnica.Descripcion,
                Beneficios = tecnica.Beneficios,
                SesionesPomodoro = tecnica.SesionesPomodoro.Select(s => new SesionPomodoroDto
                {
                    Id = s.Id,
                    Duracion = s.Duracion,
                    FechaInicio = s.FechaInicio,
                    FechaFin = s.FechaFin,
                    Estado = s.Estado,
                    ProyectoId = s.ProyectoId,
                    TareaId = s.TareaId
                }).ToList()
            };

            return Ok(tecnicaDto);
        }

        [HttpPost]
        public async Task<ActionResult> PostTecnicaEstudio(CrearTecnicaEstudioDto tecnicaEstudioDto)
        {
            // Buscar las sesiones Pomodoro asociadas por sus IDs
            var sesionesPomodoro = await _context.SesionesPomodoro
                .Where(s => tecnicaEstudioDto.SesionesPomodoroIds.Contains(s.Id))
                .ToListAsync();

            if (sesionesPomodoro.Count != tecnicaEstudioDto.SesionesPomodoroIds.Count)
            {
                return BadRequest("Algunas sesiones Pomodoro no existen.");
            }

            var tecnica = new TecnicaEstudio
            {
                Nombre = tecnicaEstudioDto.Nombre,
                Descripcion = tecnicaEstudioDto.Descripcion,
                Beneficios = tecnicaEstudioDto.Beneficios,
                SesionesPomodoro = sesionesPomodoro // Asignar las sesiones Pomodoro encontradas
            };

            _context.TecnicasEstudio.Add(tecnica);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetTecnicaEstudio), new { id = tecnica.Id }, tecnicaEstudioDto);
        }

        // Actualiza una técnica de estudio existente
        [HttpPut("{id}")]
        public async Task<IActionResult> PutTecnicaEstudio(int id, ActualizarTecnicaEstudioDto tecnicaEstudioDto)
        {
            var tecnica = await _context.TecnicasEstudio.Include(t => t.SesionesPomodoro).FirstOrDefaultAsync(t => t.Id == id);
            if (tecnica == null) return NotFound();

            tecnica.Nombre = tecnicaEstudioDto.Nombre;
            tecnica.Descripcion = tecnicaEstudioDto.Descripcion;
            tecnica.Beneficios = tecnicaEstudioDto.Beneficios;

            _context.Entry(tecnica).State = EntityState.Modified;
            await _context.SaveChangesAsync();

            return NoContent();
        }

        // Elimina una técnica de estudio por id
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteTecnicaEstudio(int id)
        {
            var tecnica = await _context.TecnicasEstudio.FindAsync(id);
            if (tecnica == null) return NotFound();

            _context.TecnicasEstudio.Remove(tecnica);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}