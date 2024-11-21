namespace Pomodoro.Shared.Dtos
{
    public class TecnicaEstudioDto
    {
        public int Id { get; set; }
        public string Nombre { get; set; }
        public string Descripcion { get; set; }
        public string? Beneficios { get; set; }
        public List<SesionPomodoroDto> SesionesPomodoro { get; set; } = new List<SesionPomodoroDto>();
    }

    public class CrearTecnicaEstudioDto
    {
        public string Nombre { get; set; }
        public string Descripcion { get; set; }
        public string? Beneficios { get; set; }
        public List<int> SesionesPomodoroIds { get; set; } = new List<int>(); // Solo IDs
    }

    public class ActualizarTecnicaEstudioDto
    {
        public string Nombre { get; set; }
        public string Descripcion { get; set; }
        public string? Beneficios { get; set; }
    }
}