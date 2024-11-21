using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Pomodoro.API.DATA;
using Pomodoro.Shared.Entities;
using Pomodoro.Shared.Dtos;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;

namespace Pomodoro.API.Controllers
{//requiere autenticacion JWT
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [ApiController]
    [Route("/api/tareas")]//ruta para usar el controlador
    public class TareasController : ControllerBase
    {
        private readonly DataContext _context;

        public TareasController(DataContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<TareaDto>>> GetTareas()
        {//consulta las tareas y las proyecta a DTOs
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

            return Ok(tareas);//devuelve la lista de tareas en JSON
        }
        //obtiene una tarea especifica por su id 
        [HttpGet("{id}")]
        public async Task<ActionResult<TareaDto>> GetTarea(int id)
        {
            var tarea = await _context.Tareas.FindAsync(id);
            if (tarea == null) return NotFound();//por si no encuentra la tarea devuelve el error 204
            
            var tareaDto = new TareaDto//proyecta la tarea encontrada en un dto
            {
                Id = tarea.Id,
                Titulo = tarea.Titulo,
                Descripcion = tarea.Descripcion,
                FechaLimite = tarea.FechaLimite,
                ProyectoId = tarea.ProyectoId
            };

            return Ok(tareaDto);//devuelve la tarea
        }
        //crea la nueva tarea
        [HttpPost]
        public async Task<ActionResult> PostTarea(TareaDto tareaDto)
        {
            if (tareaDto.ProyectoId <= 0)//verifica que el proyecto sea valido
                return BadRequest("ProyectoId is required and should be greater than 0.");

            var tarea = new Tarea//crea una nueva tarea apartir del DTO
            {
                Titulo = tareaDto.Titulo,
                Descripcion = tareaDto.Descripcion,
                FechaLimite = tareaDto.FechaLimite,
                ProyectoId = tareaDto.ProyectoId
            };

            _context.Tareas.Add(tarea);//agrega la tarea
            await _context.SaveChangesAsync();//guarda los cambios

            return Ok(tareaDto);
        }

        [HttpPut("{id}")]//actualizar una tarea existente
        public async Task<IActionResult> PutTarea(int id, TareaDto tareaDto)
        {
            if (id != tareaDto.Id) return BadRequest();

            var tarea = await _context.Tareas.FindAsync(id);
            if (tarea == null) return NotFound();//retorna un error 404 si no la encuentra
            //Actualiza sus propiedades
            tarea.Titulo = tareaDto.Titulo;
            tarea.Descripcion = tareaDto.Descripcion;
            tarea.FechaLimite = tareaDto.FechaLimite;
            tarea.ProyectoId = tareaDto.ProyectoId;

            await _context.SaveChangesAsync();//guarda los cambios
            return NoContent();//retorna un error 204 si no tiene contenido
        }

        [HttpDelete("{id}")]//elimina la tarea
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
