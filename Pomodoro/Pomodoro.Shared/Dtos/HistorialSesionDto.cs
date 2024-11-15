namespace Pomodoro.Shared.Dtos
{
    public class HistorialSesionDto
    {
        public int Id { get; set; }
        public DateTime Fecha { get; set; }
        public int Duracion { get; set; }
        public int? ProyectoId { get; set; }
        public int? SesionId { get; set; }
    }

    public class CrearHistorialSesionDto
    {
        public DateTime Fecha { get; set; }
        public int Duracion { get; set; }
        public int? ProyectoId { get; set; }
        public int SesionId { get; set; }
    }

    public class ActualizarHistorialSesionDto
    {
        public DateTime Fecha { get; set; }
        public int Duracion { get; set; }
    }
}
