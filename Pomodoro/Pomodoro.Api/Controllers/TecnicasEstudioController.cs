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
        
        // Obtiene todas las técnicas de estudio
        [HttpGet]
        public async Task<ActionResult<IEnumerable<TecnicaEstudioDto>>> GetTecnicasEstudio()
        {
            var tecnicas = await _context.TecnicasEstudio
                .Select(t => new TecnicaEstudioDto
                {
                    Id = t.Id,
                    Nombre = t.Nombre,
                    Descripcion = t.Descripcion,
                    Beneficios = t.Beneficios
                })
                .ToListAsync();

            return Ok(tecnicas);
        }

        // Obtiene una técnica de estudio por id
        [HttpGet("{id}")]
        public async Task<ActionResult<TecnicaEstudioDto>> GetTecnicaEstudio(int id)
        {
            var tecnica = await _context.TecnicasEstudio
                .FirstOrDefaultAsync(t => t.Id == id); // Busca la técnica por id

            if (tecnica == null) return NotFound(); // Si no existe, retorna 404

            var tecnicaDto = new TecnicaEstudioDto
            {
                Id = tecnica.Id,
                Nombre = tecnica.Nombre,
                Descripcion = tecnica.Descripcion,
                Beneficios = tecnica.Beneficios
            };

            return Ok(tecnicaDto); // Retorna la técnica encontrada
        }

        // Crea una nueva técnica de estudio
        [HttpPost]
        public async Task<ActionResult> PostTecnicaEstudio(CrearTecnicaEstudioDto tecnicaEstudioDto)
        {
            var tecnica = new TecnicaEstudio
            {
                Nombre = tecnicaEstudioDto.Nombre,
                Descripcion = tecnicaEstudioDto.Descripcion,
                Beneficios = tecnicaEstudioDto.Beneficios
            };

            _context.TecnicasEstudio.Add(tecnica);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetTecnicaEstudio), new { id = tecnica.Id }, tecnicaEstudioDto);
        }

        // Actualiza una técnica de estudio existente
        [HttpPut("{id}")]
        public async Task<IActionResult> PutTecnicaEstudio(int id, ActualizarTecnicaEstudioDto tecnicaEstudioDto)
        {
            var tecnica = await _context.TecnicasEstudio.FindAsync(id);
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
