namespace Pomodoro.API.Dtos
{
    public class SesionPomodoroDto
    {
        public int Id { get; set; }
        public int Duracion { get; set; }
        public DateTime FechaInicio { get; set; }
        public DateTime FechaFin { get; set; }
        public string Estado { get; set; }
        public int ProyectoId { get; set; }
        public int TareaId { get; set; }
    }

    public class CrearSesionPomodoroDto
    {
        public int Duracion { get; set; }
        public DateTime FechaInicio { get; set; }
        public DateTime FechaFin { get; set; }
        public string Estado { get; set; }
        public int ProyectoId { get; set; }
        public int TareaId { get; set; }
    }

    public class ActualizarSesionPomodoroDto
    {
        public int Duracion { get; set; }
        public DateTime FechaInicio { get; set; }
        public DateTime FechaFin { get; set; }
        public string Estado { get; set; }
    }
}
