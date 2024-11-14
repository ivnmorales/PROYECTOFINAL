namespace Pomodoro.API.Dtos
{
    public class RecompensaDto
    {
        public int Id { get; set; }
        public string Nombre { get; set; }
        public string? Descripcion { get; set; }
        public int? ProyectoId { get; set; }
    }

    public class CrearRecompensaDto
    {
        public string Nombre { get; set; }
        public string? Descripcion { get; set; }
        public int? ProyectoId { get; set; }
    }

    public class ActualizarRecompensaDto
    {
        public string Nombre { get; set; }
        public string? Descripcion { get; set; }
    }
}
