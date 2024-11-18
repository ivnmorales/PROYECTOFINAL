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
    [Route("/api/proyectos")]
    public class ProyectosController : ControllerBase
    {
        private readonly DataContext _context;

        public ProyectosController(DataContext context)
        {
            _context = context;
        }
        [AllowAnonymous]
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ProyectoDto>>> GetProyectos()
        {
            var proyectos = await _context.Proyectos
                .Include(p => p.Tareas)
                .Include(p => p.SesionesPomodoro)
                .Include(p => p.Recompensas)
                .Include(p => p.HistorialSesiones)
                .Select(p => new ProyectoDto
                {
                    Id = p.Id,
                    Nombre = p.Nombre,
                    Descripcion = p.Descripcion,
                    FechaCreacion = p.FechaCreacion
                })
                .ToListAsync();

            return Ok(proyectos);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<ProyectoDto>> GetProyecto(int id)
        {
            var proyecto = await _context.Proyectos
                .Include(p => p.Tareas)
                .Include(p => p.SesionesPomodoro)
                .Include(p => p.Recompensas)
                .Include(p => p.HistorialSesiones)
                .FirstOrDefaultAsync(p => p.Id == id);

            if (proyecto == null) return NotFound();

            var proyectoDto = new ProyectoDto
            {
                Id = proyecto.Id,
                Nombre = proyecto.Nombre,
                Descripcion = proyecto.Descripcion,
                FechaCreacion = proyecto.FechaCreacion
            };

            return Ok(proyectoDto);
        }

        [HttpPost]
        public async Task<ActionResult> PostProyecto(ProyectoDto proyectoDto)
        {
            var proyecto = new Proyecto
            {
                Nombre = proyectoDto.Nombre,
                Descripcion = proyectoDto.Descripcion,
                FechaCreacion = DateTime.UtcNow
            };

            _context.Proyectos.Add(proyecto);
            await _context.SaveChangesAsync();

            return Ok(proyectoDto);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> PutProyecto(int id, ProyectoDto proyectoDto)
        {
            if (id != proyectoDto.Id) return BadRequest();

            var proyecto = await _context.Proyectos.FindAsync(id);
            if (proyecto == null) return NotFound();

            proyecto.Nombre = proyectoDto.Nombre;
            proyecto.Descripcion = proyectoDto.Descripcion;

            await _context.SaveChangesAsync();
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteProyecto(int id)
        {
            var proyecto = await _context.Proyectos.FindAsync(id);
            if (proyecto == null) return NotFound();

            _context.Proyectos.Remove(proyecto);
            await _context.SaveChangesAsync();
            return NoContent();
        }
    }
}
