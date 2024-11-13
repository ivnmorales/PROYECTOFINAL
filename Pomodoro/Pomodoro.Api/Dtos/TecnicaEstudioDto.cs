namespace Pomodoro.API.Dtos
{
    public class TecnicaEstudioDto
    {
        public int Id { get; set; }
        public string Nombre { get; set; }
        public string Descripcion { get; set; }
        public string? Beneficios { get; set; }
    }

    public class CrearTecnicaEstudioDto
    {
        public string Nombre { get; set; }
        public string Descripcion { get; set; }
        public string? Beneficios { get; set; }
    }

    public class ActualizarTecnicaEstudioDto
    {
        public string Nombre { get; set; }
        public string Descripcion { get; set; }
        public string? Beneficios { get; set; }
    }
}
