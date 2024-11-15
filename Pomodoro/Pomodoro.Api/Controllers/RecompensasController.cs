using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Pomodoro.API.DATA;
using Pomodoro.Shared.Entities;
using Pomodoro.Shared.Dtos;

namespace Pomodoro.API.Controllers
{
    [ApiController]
    [Route("/api/recompensas")] // Ruta base del controlador para "Recompensas"
    public class RecompensasController : ControllerBase
    {
        private readonly DataContext _context; // Contexto de base de datos

        public RecompensasController(DataContext context) // Constructor con inyección de dependencias
        {
            _context = context;
        }

        // Obtiene todas las recompensas
        [HttpGet]
        public async Task<ActionResult<IEnumerable<RecompensaDto>>> GetRecompensas()
        {
            var recompensas = await _context.Recompensas
                .Include(r => r.Proyecto) // Carga el proyecto asociado, si existe
                .Select(r => new RecompensaDto
                {
                    Id = r.Id,
                    Nombre = r.Nombre,
                    Descripcion = r.Descripcion,
                    ProyectoId = r.ProyectoId
                })
                .ToListAsync();

            return Ok(recompensas);
        }

        // Obtiene una recompensa por id
        [HttpGet("{id}")]
        public async Task<ActionResult<RecompensaDto>> GetRecompensa(int id)
        {
            var recompensa = await _context.Recompensas
                .Include(r => r.Proyecto) // Carga el proyecto asociado, si existe
                .FirstOrDefaultAsync(r => r.Id == id); // Busca la recompensa por id

            if (recompensa == null) return NotFound(); // Si no existe, retorna 404

            var recompensaDto = new RecompensaDto
            {
                Id = recompensa.Id,
                Nombre = recompensa.Nombre,
                Descripcion = recompensa.Descripcion,
                ProyectoId = recompensa.ProyectoId
            };

            return Ok(recompensaDto); // Retorna la recompensa encontrada
        }

        // Crea una nueva recompensa
        [HttpPost]
        public async Task<ActionResult> PostRecompensa(CrearRecompensaDto recompensaDto)
        {
            var recompensa = new Recompensa
            {
                Nombre = recompensaDto.Nombre,
                Descripcion = recompensaDto.Descripcion,
                ProyectoId = recompensaDto.ProyectoId
            };

            _context.Recompensas.Add(recompensa);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetRecompensa), new { id = recompensa.Id }, recompensaDto);
        }

        // Actualiza una recompensa existente
        [HttpPut("{id}")]
        public async Task<IActionResult> PutRecompensa(int id, ActualizarRecompensaDto recompensaDto)
        {
            var recompensa = await _context.Recompensas.FindAsync(id);
            if (recompensa == null) return NotFound();

            recompensa.Nombre = recompensaDto.Nombre;
            recompensa.Descripcion = recompensaDto.Descripcion;

            _context.Entry(recompensa).State = EntityState.Modified;
            await _context.SaveChangesAsync();

            return NoContent();
        }

        // Elimina una recompensa por id
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteRecompensa(int id)
        {
            var recompensa = await _context.Recompensas.FindAsync(id);
            if (recompensa == null) return NotFound();

            _context.Recompensas.Remove(recompensa);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}
