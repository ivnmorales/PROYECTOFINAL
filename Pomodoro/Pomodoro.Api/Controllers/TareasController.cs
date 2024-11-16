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
    [Route("/api/tareas")]
    public class TareasController : ControllerBase
    {
        private readonly DataContext _context;

        public TareasController(DataContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<TareaDto>>> GetTareas()
        {
            var tareas = await _context.Tareas
                .Select(t => new TareaDto
                {
                    Id = t.Id,
                    Titulo = t.Titulo,
                    Descripcion = t.Descripcion,
                    FechaLimite = t.FechaLimite,
                    ProyectoId = t.ProyectoId
                })
                .ToListAsync();

            return Ok(tareas);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<TareaDto>> GetTarea(int id)
        {
            var tarea = await _context.Tareas.FindAsync(id);
            if (tarea == null) return NotFound();

            var tareaDto = new TareaDto
            {
                Id = tarea.Id,
                Titulo = tarea.Titulo,
                Descripcion = tarea.Descripcion,
                FechaLimite = tarea.FechaLimite,
                ProyectoId = tarea.ProyectoId
            };

            return Ok(tareaDto);
        }

        [HttpPost]
        public async Task<ActionResult> PostTarea(TareaDto tareaDto)
        {
            if (tareaDto.ProyectoId <= 0)
                return BadRequest("ProyectoId is required and should be greater than 0.");

            var tarea = new Tarea
            {
                Titulo = tareaDto.Titulo,
                Descripcion = tareaDto.Descripcion,
                FechaLimite = tareaDto.FechaLimite,
                ProyectoId = tareaDto.ProyectoId
            };

            _context.Tareas.Add(tarea);
            await _context.SaveChangesAsync();

            return Ok(tareaDto);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> PutTarea(int id, TareaDto tareaDto)
        {
            if (id != tareaDto.Id) return BadRequest();

            var tarea = await _context.Tareas.FindAsync(id);
            if (tarea == null) return NotFound();

            tarea.Titulo = tareaDto.Titulo;
            tarea.Descripcion = tareaDto.Descripcion;
            tarea.FechaLimite = tareaDto.FechaLimite;
            tarea.ProyectoId = tareaDto.ProyectoId;

            await _context.SaveChangesAsync();
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteTarea(int id)
        {
            var tarea = await _context.Tareas.FindAsync(id);
            if (tarea == null) return NotFound();

            _context.Tareas.Remove(tarea);
            await _context.SaveChangesAsync();
            return NoContent();
        }
    }
}
