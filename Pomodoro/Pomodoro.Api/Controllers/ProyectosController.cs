using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Pomodoro.API.DATA;
using Pomodoro.Shared.Entities;
using Pomodoro.Shared.Dtos;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;


namespace Pomodoro.API.Controllers
{
    // Requiere autenticación con JWT para acceder a los métodos del controlador.
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [ApiController]
    [Route("/api/proyectos")]//ruta para acceder al controlador
    public class ProyectosController : ControllerBase
    {
        private readonly DataContext _context;

        public ProyectosController(DataContext context)
        {
            _context = context;
        }
<<<<<<< HEAD

        //obtiene la lista de proyectos
=======
        [AllowAnonymous]
>>>>>>> origin/main
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ProyectoDto>>> GetProyectos()
        {
            //consulta los proyectos y sus relaciones
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

            return Ok(proyectos); //devuelve la lista en formato JSON
        }

        //obtiene los detalles especificos del proyecto
        [HttpGet("{id}")]
        public async Task<ActionResult<ProyectoDto>> GetProyecto(int id)
        {//busca el proyecto y sus relaciones por id
            var proyecto = await _context.Proyectos
                .Include(p => p.Tareas)
                .Include(p => p.SesionesPomodoro)
                .Include(p => p.Recompensas)
                .Include(p => p.HistorialSesiones)
                .FirstOrDefaultAsync(p => p.Id == id);

            if (proyecto == null) return NotFound();
            //convierte la entidad en un dto
            var proyectoDto = new ProyectoDto
            {
                Id = proyecto.Id,
                Nombre = proyecto.Nombre,
                Descripcion = proyecto.Descripcion,
                FechaCreacion = proyecto.FechaCreacion
            };

            return Ok(proyectoDto);//devuelve el proyecto encontrado
        }
        //crea un nuevo proyecto
        [HttpPost]
        public async Task<ActionResult> PostProyecto(ProyectoDto proyectoDto)
        {
            var proyecto = new Proyecto
            {
                Nombre = proyectoDto.Nombre,
                Descripcion = proyectoDto.Descripcion,
                FechaCreacion = DateTime.UtcNow//establece la fecha de creacion como la fecha actual
            };

            _context.Proyectos.Add(proyecto);//agrega el proyecto al contexto
            await _context.SaveChangesAsync();//guarda los cambios

            return Ok(proyectoDto); //devuelve el proyecto
        }
        //actualiza uno existente
        [HttpPut("{id}")]
        public async Task<IActionResult> PutProyecto(int id, ProyectoDto proyectoDto)
        {
            if (id != proyectoDto.Id) return BadRequest();//verifica que los ID coincidan

            var proyecto = await _context.Proyectos.FindAsync(id);//busca el proyecto
            if (proyecto == null) return NotFound();//en caso que no exista devuelve error 404
            //si existe actualiza sus propiedades
            proyecto.Nombre = proyectoDto.Nombre;
            proyecto.Descripcion = proyectoDto.Descripcion;

            await _context.SaveChangesAsync();//guarda los cambios
            return NoContent();//devuelve un estado 204 (sin contenido)
        }
        //elimina uno existente
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteProyecto(int id)
        {
            var proyecto = await _context.Proyectos.FindAsync(id);
            if (proyecto == null) return NotFound();

            _context.Proyectos.Remove(proyecto);//Elimina el proyecto
            await _context.SaveChangesAsync();//guarda los cambios
            return NoContent();
        }
    }
}
