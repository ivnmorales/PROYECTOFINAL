using Pomodoro.API.Dtos;

namespace Pomodoro.API.Dtos
{
    public class TareaDto
    {
        public int Id { get; set; }
        public string Titulo { get; set; }
        public string? Descripcion { get; set; }
        public DateTime FechaLimite { get; set; }
        public int ProyectoId { get; set; }
    }

    public class CrearTareaDto
    {
        public string Titulo { get; set; }
        public string? Descripcion { get; set; }
        public DateTime FechaLimite { get; set; }
        public int ProyectoId { get; set; }
    }

    public class ActualizarTareaDto
    {
        public string Titulo { get; set; }
        public string? Descripcion { get; set; }
        public DateTime FechaLimite { get; set; }
    }
}

